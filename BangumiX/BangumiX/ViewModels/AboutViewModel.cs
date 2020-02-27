using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BangumiX.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "关于";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://github.com/Teachoc/BangumiX"));
        }

        public ICommand OpenWebCommand { get; }
    }
}