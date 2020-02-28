using Bangumi.Api;
using Bangumi.Api.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BangumiX.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            LoginWebView.Source = $"{BgmOAuth.OAuthHOST}/authorize?client_id={BangumiApi.BgmOAuth.ClientId}&response_type=code";
            LoginWebView.Navigated += LoginWebView_Navigated;
        }

        private async void LoginWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.StartsWith($"{BgmOAuth.OAuthHOST}/{BangumiApi.BgmOAuth.RedirectUrl}?code="))
            {
                var code = e.Url.Replace($"{BgmOAuth.OAuthHOST}/{BangumiApi.BgmOAuth.RedirectUrl}?code=", "");
                try
                {
                    await BangumiApi.BgmOAuth.GetToken(code);
                    Application.Current.MainPage = new MainPage();
                    //await Navigation.PopModalAsync();
                }
                catch (Exception)
                {
                    LoginWebView.Source = $"{BgmOAuth.OAuthHOST}/authorize?client_id={BangumiApi.BgmOAuth.ClientId}&response_type=code";
                    LoginWebView.Reload();
                }
            }
        }

        private void RefreshToolbarItem_Clicked(object sender, EventArgs e)
        {
            LoginWebView.Source = $"{BgmOAuth.OAuthHOST}/authorize?client_id={BangumiApi.BgmOAuth.ClientId}&response_type=code";
            LoginWebView.Reload();
        }
    }
}