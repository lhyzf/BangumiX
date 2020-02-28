using Bangumi.Api;
using Bangumi.Api.Exceptions;
using Bangumi.Api.Models;
using BangumiX.Models;
using BangumiX.ViewModels;
using System;

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
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is WatchProgress item)
            {
                await Navigation.PushAsync(new EpisodePage(new EpisodeViewModel(new SubjectLarge
                {
                    NameCn = item.NameCn,
                    Name = item.Name,
                    Id = item.SubjectId
                })));
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!BangumiApi.BgmOAuth.IsLogin)
            {
                await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
                return;
            }
            if (viewModel.Items.Count == 0)
            {
                if (!BangumiApi.BgmCache.IsUpdatedToday)
                {
                    try
                    {
                        await BangumiApi.BgmOAuth.CheckToken();
                    }
                    catch (BgmUnauthorizedException)
                    {
                        // 授权过期，返回登录界面
                        await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
                        return;
                    }
                    catch (Exception)
                    {

                    }
                }
                viewModel.RefreshCommand.Execute(null);
            }
        }

        private async void NextEpButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var item = button?.BindingContext as WatchProgress;
            if (item == null || viewModel.IsBusy)
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