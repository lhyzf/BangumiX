using Bangumi.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BangumiX.ViewModels
{
    public class EditSubjectStatusViewModel : BaseViewModel
    {
        private readonly string[] descs = { string.Empty, "不忍直视 1", "很差 2", "差 3", "较差 4", "不过不失 5", "还行 6", "推荐 7", "力荐 8", "神作 9", "超神作 10 (请谨慎评价)" };
        private readonly string SubjectId;
        public string Name { get; set; }

        public List<PickerStatus> PickerStatusList { get; set; }
        private PickerStatus _status;
        public PickerStatus Status
        {
            get { return _status; }
            set
            {
                SetProperty(ref _status, value);
                OnPropertyChanged(nameof(CanSubmit));
            }
        }
        public bool CanSubmit => Status != null;


        private double _rating = 0;
        public double Rating
        {
            get { return _rating; }
            set
            {
                SetProperty(ref _rating, value);
                OnPropertyChanged(nameof(RateDesc));
            }
        }
        public string RateDesc => descs[(int)Math.Round(Rating, MidpointRounding.AwayFromZero)];

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                SetProperty(ref _comment, value);
            }
        }

        private string _private = "0";
        public bool PrivateBool
        {
            get { return _private == "1"; }
            set
            {
                _private = value ? "1" : "0";
                OnPropertyChanged();
            }
        }





        public EditSubjectStatusViewModel(string id, string name)
        {
            Title = "收藏盒";
            SubjectId = id;
            Name = name;
            PickerStatusList = new List<PickerStatus>
            {
                new PickerStatus { Desc="想看", Status=  CollectionStatusType.Wish },
                new PickerStatus { Desc="看过", Status=  CollectionStatusType.Collect },
                new PickerStatus { Desc="在看", Status=  CollectionStatusType.Do },
                new PickerStatus { Desc="搁置", Status=  CollectionStatusType.OnHold },
                new PickerStatus { Desc="抛弃", Status=  CollectionStatusType.Dropped },
            };
            GetSubjectStatus();
        }

        private async Task GetSubjectStatus()
        {
            IsBusy = true;
            try
            {
                var status = await Bangumi.Api.BangumiApi.BgmApi.Status(SubjectId);
                Status = PickerStatusList.Find(it => it.Status == status?.Status?.Id);
                Rating = status.Rating;
                Comment = status.Comment;
                PrivateBool = status.Private == "1";
            }
            catch { }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task EditSubjectStatus()
        {
            IsBusy = true;
            try
            {
                await Bangumi.Api.BangumiApi.BgmApi.UpdateStatus(
                    SubjectId, Status.Status, Comment, ((int)Math.Round(Rating, MidpointRounding.AwayFromZero)).ToString(), _private);
            }
            catch
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public class PickerStatus
    {
        public string Desc { get; set; }
        public CollectionStatusType Status { get; set; }
    }
}
