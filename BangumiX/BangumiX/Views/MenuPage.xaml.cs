using BangumiX.Models;
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
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Progress, Title="Progress" },
                new HomeMenuItem {Id = MenuItemType.Collection, Title="Collection" },
                new HomeMenuItem {Id = MenuItemType.Calendar, Title="Calendar" },
                new HomeMenuItem {Id = MenuItemType.Search, Title="Search" },
                new HomeMenuItem {Id = MenuItemType.Setting, Title="Setting" },
                new HomeMenuItem {Id = MenuItemType.About, Title="About" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.ItemTapped += async (sender, e) =>
            {
                if (e.Item == null)
                    return;

                var id = (int)((HomeMenuItem)e.Item).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (Bangumi.Api.BangumiApi.BgmOAuth.IsLogin)
            {
                UsernameLabel.Text = $"@{Bangumi.Api.BangumiApi.BgmOAuth.MyToken.UserId}";
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        var user = await Bangumi.Api.BangumiApi.BgmApi.User();
                        UsernameLabel.Text = $"@{user.UserName}";
                        UserAvatarImage.Source = user.Avatar.Medium;
                        break;
                    }
                    catch (Exception)
                    {
                        await Task.Delay(3000);
                    }
                }
            }
            else
            {
                UsernameLabel.Text = "未登录";
                UserAvatarImage.Source = null;
            }
        }
    }
}