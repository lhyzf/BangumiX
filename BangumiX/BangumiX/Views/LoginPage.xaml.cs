using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Bangumi.Api.Services;
using Bangumi.Api;

namespace BangumiX.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            LoginWebView.Source = $"{BgmOAuth.OAuthHOST}/authorize?client_id={BgmOAuth.ClientId}&response_type=code";
            LoginWebView.Navigated += LoginWebView_Navigated;
        }

        private async void LoginWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if (e.Url.StartsWith($"{BgmOAuth.OAuthHOST}/{BgmOAuth.RedirectUrl}?code="))
            {
                var code = e.Url.Replace($"{BgmOAuth.OAuthHOST}/{BgmOAuth.RedirectUrl}?code=", "");
                try
                {
                    await BangumiApi.BgmOAuth.GetToken(code);
                    Application.Current.MainPage = new MainPage();
                    //await Navigation.PopModalAsync();
                }
                catch (Exception)
                {
                    LoginWebView.Source = $"{BgmOAuth.OAuthHOST}/authorize?client_id={BgmOAuth.ClientId}&response_type=code";
                    LoginWebView.Reload();
                }
            }
        }

        private void RefreshToolbarItem_Clicked(object sender, EventArgs e)
        {
            LoginWebView.Source = $"{BgmOAuth.OAuthHOST}/authorize?client_id={BgmOAuth.ClientId}&response_type=code";
            LoginWebView.Reload();
        }
    }
}