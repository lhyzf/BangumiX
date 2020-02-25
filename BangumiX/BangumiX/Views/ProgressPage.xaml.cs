using Bangumi.Api.Models;
using BangumiX.Models;
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
    public partial class ProgressPage : ContentPage
    {
        ProgressViewModel viewModel;

        public ProgressPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ProgressViewModel();
            if (!Bangumi.Api.BangumiApi.BgmOAuth.IsLogin)
            {
                Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            }
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as WatchProgress;
            if (item == null)
                return;

            await Navigation.PushAsync(new DetailPage(new DetailViewModel(new SubjectLarge
            {
                NameCn = item.NameCn,
                Name = item.Name,
                Id = item.SubjectId
            })));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Bangumi.Api.BangumiApi.BgmOAuth.IsLogin && viewModel.Items.Count == 0)
                viewModel.RefreshCommand.Execute(null);
        }

        private async void NextEpButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button?.BindingContext as WatchProgress;
            if (item == null)
                return;

            try
            {
                await item.MarkNextEpWatched();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}