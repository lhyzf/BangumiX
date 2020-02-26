using Bangumi.Api.Models;
using BangumiX.Models;
using BangumiX.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BangumiX.ViewModels
{
    public class CalendarViewModel : BaseViewModel
    {
        public IDataStore<Calendar> DataStore => DependencyService.Get<IDataStore<Calendar>>();
        public ObservableCollection<Calendar> Items { get; set; }
        public Command RefreshCommand { get; set; }

        public CalendarViewModel()
        {
            Title = "时间表";
            Items = new ObservableCollection<Calendar>();
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());

        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (Items.Count == 0)
                {
                    var cache = await DataStore.GetItemsAsync();
                    foreach (var item in cache)
                    {
                        Items.Add(item);
                    }
                }
                var items = await DataStore.GetItemsAsync(true);
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
