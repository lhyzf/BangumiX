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
    public partial class DetailPage : ContentPage
    {
        DetailViewModel viewModel;
        public Command EditSubjectStatusCommand { get; set; }

        public DetailPage(DetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            MyFloatButton.Command = new Command(async () => await ExecuteEditSubjectStatusCommand());
        }

        private async Task ExecuteEditSubjectStatusCommand()
        {
            await Navigation.PushAsync(new EditSubjectStatusPage(new EditSubjectStatusViewModel(viewModel.Subject.Id.ToString(), viewModel.Title)));
        }
    }
}