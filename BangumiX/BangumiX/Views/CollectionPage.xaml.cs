using Bangumi.Api.Models;
using BangumiX.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BangumiX.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CollectionPage : TabbedPage
    {
        public CollectionViewModel viewModel { get; private set; }

        public CollectionPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CollectionViewModel();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is SubjectBaseE item)
            {
                await Navigation.PushAsync(new EpisodePage(new EpisodeViewModel(new SubjectLarge
                {
                    NameCn = item.Subject.NameCn,
                    Name = item.Subject.Name,
                    Id = item.SubjectId
                })));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.RefreshCommand.Execute(null);
        }

        private void SubjectTypeToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (sender is ToolbarItem item)
            {
                SubjectType subjectType = SubjectType.Anime;
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
                    default:
                        break;
                }
                viewModel.RefreshCommand.Execute(subjectType);
            }
        }
    }
}