using Bangumi.Api;
using Bangumi.Api.Common;
using Bangumi.Api.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BangumiX.ViewModels
{
    public class EpisodeViewModel : BaseViewModel
    {
        public readonly string SubjectId;
        public ObservableCollection<GroupedEpisode> GroupedEps { get; private set; } = new ObservableCollection<GroupedEpisode>();

        private string _subjectType;
        public string SubjectType
        {
            get => _subjectType;
            set => SetProperty(ref _subjectType, value);
        }

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _airDate;
        public string AirDate
        {
            get => _airDate;
            set => SetProperty(ref _airDate, value);
        }

        private string _airWeekDay;
        public string AirWeekDay
        {
            get => _airWeekDay;
            set => SetProperty(ref _airWeekDay, value);
        }

        private string _summary;
        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        private double _score;
        public double Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        private string _rank;
        public string Rank
        {
            get => _rank;
            set => SetProperty(ref _rank, value);
        }

        // 详情
        public DetailViewModel Detail { get; private set; }

        public Command RefreshCommand { get; set; }

        public EpisodeViewModel(SubjectLarge subject)
        {
            Title = "章节";
            Name = string.IsNullOrEmpty(subject.NameCn) ? subject.Name : subject.NameCn;
            SubjectId = subject.Id.ToString();
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());
        }

        /// <summary>
        /// 更新章节状态
        /// </summary>
        /// <param name="ep"></param>
        /// <param name="status">状态</param>
        public async Task UpdateEpStatus(EpisodeWithEpStatus ep, EpStatusType status)
        {
            if (ep != null)
            {
                try
                {
                    IsBusy = true;
                    if (await BangumiApi.BgmApi.UpdateProgress(ep.Id.ToString(), status))
                    {
                        ep.EpStatus = status;
                    }
                }
                catch (Exception e)
                {
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        /// <summary>
        /// 加载详情和章节，
        /// 用户进度，收藏状态。
        /// </summary>
        public async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var subject = BangumiApi.BgmApi.Subject(SubjectId);
                ProcessSubject(BangumiApi.BgmCache.Subject(SubjectId));
                // 检查用户登录状态
                if (BangumiApi.BgmOAuth.IsLogin)
                {
                    var progress = BangumiApi.BgmApi.Progress(SubjectId);
                    ProcessProgress(BangumiApi.BgmCache.Progress(SubjectId));
                    ProcessSubject(await subject);
                    ProcessProgress(await progress);
                }
                else
                {
                    ProcessSubject(await subject);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }



        /// <summary>
        /// 处理条目信息
        /// </summary>
        /// <param name="subject"></param>
        private void ProcessSubject(SubjectLarge subject)
        {
            if (subject == null || subject.Id.ToString() != SubjectId)
            {
                return;
            }
            // 条目标题
            Name = string.IsNullOrEmpty(subject?.NameCn) ? subject?.Name : subject?.NameCn;
            // 条目图片
            ImageSource = subject.Images?.Common;
            // 放送日期
            AirDate = subject.AirDate;
            AirWeekDay = subject.AirWeekdayCn;
            if (subject.Rating != null)
            {
                Score = subject.Rating.Score;
            }
            if (subject.Rank == 0)
            {
                Rank = "???";
            }
            else
            {
                Rank = "#" + subject.Rank.ToString();
            }
            SubjectType = subject.Type.GetDesc();
            // 详情
            string info = "作品分类：" + subject.Type.GetDesc();
            info += subject.AirDate == "0000-00-00" ? "" : "\n放送开始：" + subject.AirDate;
            info += subject.AirWeekday == 0 ? "" : "\n放送星期：" + subject.AirWeekdayCn;
            info += subject.Eps == null ? "" : "\n话数：" + subject.Eps.Count;
            Detail = new DetailViewModel
            {
                Name = subject.Name,
                Summary = subject.Summary,
                Info = info,
                Characters = subject.Characters,
                Staffs = subject.Staff,
                Blogs = subject.Blogs,
                Topics = subject.Topics
            };

            // 章节
            if (subject.Eps != null)
            {
                var eps = new List<EpisodeWithEpStatus>();
                foreach (var ep in subject.Eps)
                {
                    eps.Add(EpisodeWithEpStatus.FromEpisode(ep));
                }
                var groupEps = eps.GroupBy(ep => ep.Type)
                    .OrderBy(g => g.Key)
                    .Select(g => new GroupedEpisode(g) { Key = g.Key.GetDesc() });
                if (!groupEps.SequenceEqualExT(GroupedEps))
                {
                    GroupedEps.Clear();
                    foreach (var item in groupEps)
                    {
                        GroupedEps.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 处理用户章节状态
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="progress"></param>
        private void ProcessProgress(Progress progress)
        {
            if (progress?.Eps == null || progress.SubjectId.ToString() != SubjectId)
            {
                return;
            }
            foreach (var group in GroupedEps) //用户观看状态
            {
                foreach (EpisodeWithEpStatus ep in group)
                {
                    var prog = progress.Eps?.Where(p => p.Id == ep.Id).FirstOrDefault();
                    ep.EpStatus = prog?.Status?.Id ?? EpStatusType.remove;
                }
            }
        }

    }
    public class GroupedEpisode : List<EpisodeWithEpStatus>
    {
        public GroupedEpisode(IEnumerable<EpisodeWithEpStatus> items) : base(items)
        {
        }
        public string Key { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var g = (GroupedEpisode)obj;
            return Key == g.Key &&
                   this.SequenceEqualExT(g);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
