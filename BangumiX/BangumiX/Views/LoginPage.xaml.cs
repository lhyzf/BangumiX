using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Auth;
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
                await BangumiApi.BgmOAuth.GetToken(code);
                await Navigation.PopModalAsync();
            }
        }
    }
}