
using BangumiX.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BangumiX.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        SettingViewModel viewModel;
        public SettingPage()
        {
            InitializeComponent();
            BindingContext = this.viewModel = new SettingViewModel();
        }

        private async void UpdateBangumiDataButton_Clicked(object sender, System.EventArgs e)
        {
            await viewModel.UpdateBangumiData();
        }

        private void MinusIntervalButton_Clicked(object sender, System.EventArgs e)
        {
            viewModel.BangumiDataCheckInterval--;
        }

        private void AddIntervalButton_Clicked(object sender, System.EventArgs e)
        {
            viewModel.BangumiDataCheckInterval++;
        }
    }
}