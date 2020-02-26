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
    public partial class SearchPage : ContentPage
    {
        public SearchViewModel viewModel { get; private set; }

        public SearchPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SearchViewModel();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is SubjectForSearch item)
            {
                await Navigation.PushAsync(new DetailPage(new DetailViewModel(new SubjectLarge
                {
                    NameCn = item.NameCn,
                    Name = item.Name,
                    Id = item.Id
                })));
            }
        }

        private void SubjectTypeToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (sender is ToolbarItem item)
            {
                SubjectType? subjectType;
                switch (item.Text)
                {
                    case "动画":
                        subjectType = SubjectType.Anime;
                        break;
                    case "书籍":
                        subjectType = SubjectType.Book;
                        break;
                    case "音乐":
                        subjectType = SubjectType.Music;
                        break;
                    case "游戏":
                        subjectType = SubjectType.Game;
                        break;
                    case "三次元":
                        subjectType = SubjectType.Real;
                        break;
                    case "全部":
                    default:
                        subjectType = null;
                        break;
                }
                viewModel.SwitchSubjectTypeCommand.Execute(subjectType);
            }
        }
    }
}