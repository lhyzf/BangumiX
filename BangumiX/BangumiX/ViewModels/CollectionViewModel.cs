using Bangumi.Api.Models;
using BangumiX.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BangumiX.ViewModels
{
    public class CollectionViewModel : BaseViewModel
    {
        public ITypeDataStore<Collection> DataStore => DependencyService.Get<ITypeDataStore<Collection>>();
        public ObservableCollection<Collection> Items { get; set; }
        public Command RefreshCommand { get; set; }
        private SubjectType subjectType = SubjectType.Anime;

        public CollectionViewModel()
        {
            Title = "收藏 - 动画";
            Items = new ObservableCollection<Collection>();
            RefreshCommand = new Command(async (args) => await ExecuteRefreshCommand(args));
        }

        async Task ExecuteRefreshCommand(object args)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            if (args != null)
            {
                subjectType = (SubjectType)args;
                Title = $"收藏 - {subjectType.GetDesc()}";
            }
            try
            {
                var cache = await DataStore.GetItemsAsync(subjectType.GetValue());
                if (Items.Count != cache.Count())
                {
                    Items.Clear();
                    foreach (var item in cache)
                    {
                        Items.Add(item);
                    }
                }
                var items = await DataStore.GetItemsAsync(subjectType.GetValue(), true);
                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(item);
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
    }
}
