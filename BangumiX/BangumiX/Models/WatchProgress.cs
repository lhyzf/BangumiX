using Bangumi.Api;
using Bangumi.Api.Common;
using Bangumi.Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BangumiX.Models
{
    public class WatchProgress : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性变更通知
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// 检查属性值是否相同。
        /// 仅在不同时设置属性值。
        /// </summary>
        /// <typeparam name="T">属性类型。</typeparam>
        /// <param name="storage">可读写的属性。</param>
        /// <param name="value">属性值。</param>
        /// <param name="propertyName">属性名。可被支持 CallerMemberName 的编译器自动提供。</param>
        /// <returns>值改变则返回 true，未改变返回 false。</returns>
        protected bool Set<T>(ref T storage, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Name { get; set; }
        public string NameCn { get; set; }
        public int SubjectId { get; set; }
        public long LastTouch { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public SubjectType Type { get; set; }
        public string AirTime { get; set; }
        public string DisplayName => string.IsNullOrEmpty(NameCn) ? Name : NameCn;
        public string AirWeekday
        {
            get
            {
                if (DateTime.TryParse(AirTime, out var d))
                {
                    if (AirTime.Length > 10)
                    {
                        return $"{System.Globalization.CultureInfo.CreateSpecificCulture("zh-cn").DateTimeFormat.GetDayName(d.DayOfWeek)} {d.TimeOfDay:hh\\:mm}";
                    }
                    return System.Globalization.CultureInfo.CreateSpecificCulture("zh-cn").DateTimeFormat.GetDayName(d.DayOfWeek);
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 正片数量
        /// </summary>
        public int EpsCount => Eps?.Count(ep => ep.Type == EpisodeType.本篇) ?? -1;
        public List<EpisodeForSort> Eps { get; set; }

        private bool _isUpdating;
        public bool IsUpdating
        {
            get => _isUpdating;
            set
            {
                Set(ref _isUpdating, value);
                if (!value)
                {
                    OnPropertyChanged(nameof(AirEpsCount));
                    OnPropertyChanged(nameof(WatchedEpsCount));
                    OnPropertyChanged(nameof(WatchedAndAirEpsCountDesc));
                    OnPropertyChanged(nameof(NextEp));
                    OnPropertyChanged(nameof(NextEpDesc));
                    OnPropertyChanged(nameof(EpColor));
                }
            }
        }

        public int AirEpsCount => Eps?.Count(ep => ep.Type == EpisodeType.本篇 && Regex.IsMatch(ep.Status, "(Air|Today)")) ?? -1;
        public int WatchedEpsCount => Eps?.Count(ep => ep.Type == EpisodeType.本篇 && ep.EpStatus != EpStatusType.remove) ?? -1;
        public string WatchedAndAirEpsCountDesc
        {
            get
            {
                string air;
                if (EpsCount == -1)
                {
                    air = "无章节";
                }
                else if (AirEpsCount == 0)
                {
                    air = "尚未放送";
                }
                else if (AirEpsCount < EpsCount)
                {
                    air = "更新到" + AirEpsCount + "话";
                }
                else
                {
                    air = "全" + AirEpsCount + "话";
                }
                string watch;
                if (WatchedEpsCount == -1)
                {
                    return air;
                }
                else if (WatchedEpsCount == 0)
                {
                    watch = "尚未观看";
                }
                else
                {
                    watch = "看到" + WatchedEpsCount + "话";
                }
                return $"{watch} / {air}";
            }
        }
        public EpisodeForSort NextEp => Eps?.FirstOrDefault(ep => ep.Type == EpisodeType.本篇 && ep.EpStatus == EpStatusType.remove);
        public string NextEpDesc => $"EP.{NextEp?.Sort} {(string.IsNullOrEmpty(NextEp?.NameCn) ? NextEp?.Name : NextEp?.NameCn)}";
        public string EpColor => (NextEp == null || NextEp.Status == "NA") ? "Gray" : "#d26585";

        public static WatchProgress FromWatching(Watching w) => new WatchProgress
        {
            Name = w.Subject.Name,
            NameCn = w.Subject.NameCn,
            Image = w.Subject.Images.Common,
            SubjectId = w.SubjectId,
            Url = w.Subject.Url,
            LastTouch = w.LastTouch, // 该条目上次修改时间
            AirTime = w.Subject.AirDate,
            Type = w.Subject.Type,
            IsUpdating = false,
        };

        public void ProcessEpisode(SubjectLarge subject)
        {
            if (subject?.Eps != null)
            {
                Eps = new List<EpisodeForSort>();
                foreach (var ep in subject.Eps)
                {
                    Eps.Add(EpisodeForSort.FromEpisode(ep));
                }
            }
            var first = Eps?.FirstOrDefault(ep => ep.Type == EpisodeType.本篇)?.AirDate;
            var last = Eps?.LastOrDefault(ep => ep.Type == EpisodeType.本篇 && !Regex.IsMatch(ep.Status, "(NA)"))?.AirDate;
            //if (SettingHelper.UseBangumiDataAirTime &&
            //    BangumiData.GetAirTimeByBangumiId(subject.Id.ToString())?.ToLocalTime() is DateTimeOffset date)
            //{
            //    if (first != null && last != null)
            //    {
            //        AirTime = date.AddTicks(last.Value.Ticks).AddTicks(-first.Value.Ticks).ToString("yyyy-MM-dd HH:mm");
            //    }
            //    else
            //    {
            //        AirTime = date.ToString("yyyy-MM-dd HH:mm");
            //    }
            //}
            //else if (DateTime.TryParse(AirTime, out var airDate))
            //{
            //    if (first != null && last != null)
            //    {
            //        AirTime = airDate.AddTicks(last.Value.Ticks).AddTicks(-first.Value.Ticks).ToString("yyyy-MM-dd");
            //    }
            //    else
            //    {
            //        AirTime = airDate.ToString("yyyy-MM-dd");
            //    }
            //}
        }

        public void ProcessProgress(Progress progress)
        {
            if (progress?.Eps != null)
            {
                // 填充用户观看状态
                foreach (var ep in Eps)
                {
                    var prog = progress.Eps.Where(p => p.Id == ep.Id).FirstOrDefault();
                    ep.EpStatus = prog?.Status?.Id ?? EpStatusType.remove;
                }
            }
        }

        /// <summary>
        /// 标记下一话为看过
        /// </summary>
        public async Task MarkNextEpWatched()
        {
            if (NextEp == null)
            {
                return;
            }
            var next = NextEp;
            try
            {
                IsUpdating = true;
                if (await BangumiApi.BgmApi.UpdateProgress(next.Id.ToString(), EpStatusType.watched))
                {
                    next.EpStatus = EpStatusType.watched;
                    LastTouch = DateTime.Now.ToJsTick();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
            finally
            {
                IsUpdating = false;
            }
        }
    }

    public class EpisodeForSort : EpisodeWithEpStatus
    {
        public new DateTime AirDate { get; set; }

        public new static EpisodeForSort FromEpisode(Episode ep) => new EpisodeForSort
        {
            Id = ep.Id,
            Url = ep.Url,
            Type = ep.Type,
            Sort = ep.Sort,
            Name = ep.Name,
            NameCn = ep.NameCn,
            Duration = ep.Duration,
            AirDate = DateTime.TryParse(ep.AirDate, out var d) ? d : d,
            Comment = ep.Comment,
            Desc = ep.Desc,
            Status = ep.Status,
            EpStatus = EpStatusType.remove
        };
    }
}
