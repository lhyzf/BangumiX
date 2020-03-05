using Bangumi.Api;
using Bangumi.Api.Common;
using Bangumi.Api.Models;
using BangumiX.Helper;
using BangumiX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BangumiX.Services
{
    public class ProgressDataStore : IDataStore<WatchProgress>
    {
        private readonly List<WatchProgress> items;

        public ProgressDataStore()
        {
            items = new List<WatchProgress>();
            items.AddRange(SortWatchProgress(CachedWatchProgress()));
        }

        public Task<bool> AddItemAsync(WatchProgress item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<WatchProgress> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WatchProgress>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                items.Clear();
                // 加载缓存，后面获取新数据后比较需要使用
                var cachedWatchings = BangumiApi.BgmCache.Watching();
                var cachedWatchStatus = CachedWatchProgress().ToList();

                // 加载新的收视进度
                var newWatchStatus = new List<WatchProgress>();
                var newWatching = await BangumiApi.BgmApi.Watching();
                var subjectTasks = new List<Task<SubjectLarge>>();
                var progressTasks = new List<Task<Progress>>();
                SubjectLarge[] newSubjects = null;
                Progress[] newProgresses = null;
                // 新的收视进度与缓存的不同或未缓存的条目
                var watchingsNotCached = BangumiApi.BgmCache.IsUpdatedToday ?
                                         newWatching.Where(it => cachedWatchings.All(it2 => !it2.EqualsExT(it))).ToList() :
                                         newWatching;
                using (var semaphore = new SemaphoreSlim(10))
                {
                    foreach (var item in watchingsNotCached)
                    {
                        await semaphore.WaitAsync();
                        subjectTasks.Add(BangumiApi.BgmApi.SubjectEp(item.SubjectId.ToString())
                            .ContinueWith(t =>
                            {
                                semaphore.Release();
                                return t.Result;
                            }));
                        await semaphore.WaitAsync();
                        progressTasks.Add(BangumiApi.BgmApi.Progress(item.SubjectId.ToString())
                            .ContinueWith(t =>
                            {
                                semaphore.Release();
                                return t.Result;
                            }));
                    }
                    newSubjects = await Task.WhenAll(subjectTasks);
                    newProgresses = await Task.WhenAll(progressTasks);
                }
                foreach (var watching in newWatching)
                {
                    WatchProgress item;
                    if (watchingsNotCached.Any(it => it.SubjectId == watching.SubjectId))
                    {
                        item = WatchProgress.FromWatching(watching);
                        item.ProcessEpisode(newSubjects.FirstOrDefault(it => it.Id == item.SubjectId));
                        item.ProcessProgress(newProgresses.FirstOrDefault(it => it?.SubjectId == item.SubjectId));
                    }
                    else
                    {
                        item = cachedWatchStatus.Find(it => it.SubjectId == watching.SubjectId);
                    }
                    newWatchStatus.Add(item);
                }
                BangumiApi.BgmCache.IsUpdatedToday = true;
                items.AddRange(SortWatchProgress(newWatchStatus));
            }
            return await Task.FromResult(items);
        }

        public Task<bool> UpdateItemAsync(WatchProgress item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 缓存的收视进度
        /// </summary>
        private IEnumerable<WatchProgress> CachedWatchProgress()
        {
            foreach (var watching in BangumiApi.BgmCache.Watching())
            {
                var subject = BangumiApi.BgmCache.Subject(watching.SubjectId.ToString());
                var progress = BangumiApi.BgmCache.Progress(watching.SubjectId.ToString());

                var item = WatchProgress.FromWatching(watching);
                item.ProcessEpisode(subject);
                item.ProcessProgress(progress);
                if (subject == null || progress == null)
                {
                    // 标记以重新加载
                    watching.Subject.Eps = -1;
                }
                yield return item;
            }
        }

        /// <summary>
        /// 对条目进行排序
        /// </summary>
        private IEnumerable<WatchProgress> SortWatchProgress(IEnumerable<WatchProgress> watchingStatuses)
        {
            if (SettingHelper.Setting.OrderByAirTime)
            {
                return watchingStatuses.OrderBy(p => p.EpColor)
                    .ThenBy(p => p.WatchedEpsCount == 0)
                    .ThenBy(p => p.AirEpsCount - p.WatchedEpsCount)
                    .ThenBy(p => p.Eps?.LastOrDefault(ep => ep.Type == EpisodeType.本篇 && !Regex.IsMatch(ep.Status, "(NA)"))?.AirDate)
                    .ThenBy(p => p.AirTime);
            }
            return watchingStatuses.OrderByDescending(w => w.LastTouch);
        }

    }
}
