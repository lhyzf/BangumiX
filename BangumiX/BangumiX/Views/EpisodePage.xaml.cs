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
    public partial class EpisodePage : ContentPage
    {
        EpisodeViewModel viewModel;
        public Command EditSubjectStatusCommand { get; set; }

        public EpisodePage(EpisodeViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            MyFloatButton.Command = new Command(async () => await ExecuteEditSubjectStatusCommand());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.RefreshCommand.Execute(null);
        }

        private async Task ExecuteEditSubjectStatusCommand()
        {
            await Navigation.PushAsync(new EditSubjectStatusPage(new EditSubjectStatusViewModel(viewModel.SubjectId, viewModel.Name)));
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
            if (sender is SwipeItem item)
            {
                var ep = item.BindingContext as EpisodeWithEpStatus;
                switch (item.Text)
                {
                    case "看过":
                        viewModel.UpdateEpStatus(ep, EpStatusType.watched);
                        break;
                    //case "WatchedTo":
                    //    viewModel.UpdateEpStatusBatch(ep);
                    //    break;
                    case "想看":
                        viewModel.UpdateEpStatus(ep, EpStatusType.queue);
                        break;
                    case "抛弃":
                        viewModel.UpdateEpStatus(ep, EpStatusType.drop);
                        break;
                    case "撤销":
                        viewModel.UpdateEpStatus(ep, EpStatusType.remove);
                        break;
                    default:
                        break;
                }
            }
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DetailPage(viewModel.Detail));
        }
    }
}