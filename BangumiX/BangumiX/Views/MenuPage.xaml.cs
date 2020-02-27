using BangumiX.Models;
using System;
using System.Collections.Generic;
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
                new HomeMenuItem {Id = MenuItemType.Progress, Title="进度" },
                new HomeMenuItem {Id = MenuItemType.Collection, Title="收藏" },
                new HomeMenuItem {Id = MenuItemType.Calendar, Title="时间表" },
                new HomeMenuItem {Id = MenuItemType.Search, Title="搜索" },
                new HomeMenuItem {Id = MenuItemType.Setting, Title="设置" },
                new HomeMenuItem {Id = MenuItemType.About, Title="关于" }
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