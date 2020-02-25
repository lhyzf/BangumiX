using Bangumi.Api.Models;
using BangumiX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BangumiX.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : TabbedPage
    {
        public CalendarViewModel viewModel { get; private set; }

        public CalendarPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CalendarViewModel();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is SubjectForCalendar item)
            {
                await Navigation.PushAsync(new DetailPage(new DetailViewModel(new SubjectLarge
                {
                    NameCn = item.NameCn,
                    Name = item.Name,
                    Id = item.Id
                })));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.RefreshCommand.Execute(null);
        }
    }
}