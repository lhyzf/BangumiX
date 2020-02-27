using BangumiX.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BangumiX.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditSubjectStatusPage : ContentPage
    {
        public EditSubjectStatusViewModel viewModel;
        public EditSubjectStatusPage(EditSubjectStatusViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                await viewModel.EditSubjectStatus();
                await Navigation.PopAsync();
            }
            catch (Exception)
            {
                // Notify
            }
        }

        private void Slider_DragCompleted(object sender, EventArgs e)
        {
            if(sender is Slider slider)
            {
                slider.Value = Math.Round(slider.Value, MidpointRounding.AwayFromZero);
            }
        }
    }
}