using Bangumi.Api.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BangumiX.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public ObservableCollection<SubjectForSearch> Items { get; set; }
        public Command SwitchSubjectTypeCommand { get; set; }
        public Command RefreshCommand { get; set; }
        public Command LoadMoreCommand { get; set; }
        private SubjectType? subjectType = null;
        public string Keyword { get; set; }
        private int offset = 0;
        private int max = 20;
        private string searchType = string.Empty;
        public bool HasMoreItems => offset < max;

        public SearchViewModel()
        {
            Title = "搜索";
            Items = new ObservableCollection<SubjectForSearch>();
            SwitchSubjectTypeCommand = new Command((args) => ExecuteSwitchSubjectTypeCommand(args));
            RefreshCommand = new Command(async () => await ExecuteLoadMoreCommand(true));
            LoadMoreCommand = new Command(async () => await ExecuteLoadMoreCommand(false));
        }

        private void ExecuteSwitchSubjectTypeCommand(object args)
        {
            var type = args as SubjectType?;
            if (subjectType != type)
            {
                Items.Clear();
                subjectType = type;
                if (subjectType != null)
                {
                    Title = $"搜索 - {subjectType.Value.GetDesc()}";
                    searchType = ((int)subjectType).ToString();
                }
                else
                {
                    Title = "搜索";
                    searchType = string.Empty;
                }
            }
            RefreshCommand.Execute(null);
        }

        private async Task ExecuteLoadMoreCommand(bool forceRefresh)
        {
            if (IsBusy || string.IsNullOrEmpty(Keyword))
                return;

            IsBusy = true;
            if (forceRefresh)
            {
                Items.Clear();
                offset = 0;
                OnPropertyChanged(nameof(HasMoreItems));
            }
            try
            {
                var result = await Bangumi.Api.BangumiApi.BgmApi.Search(Keyword, searchType, offset, 20);
                max = result.ResultCount;
                var items = result.Results;
                foreach (var item in items)
                {
                    Items.Add(item);
                }
                offset += 20;
                OnPropertyChanged(nameof(HasMoreItems));
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

    }
}
