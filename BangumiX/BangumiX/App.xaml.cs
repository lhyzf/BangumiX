using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BangumiX.Services;
using BangumiX.Views;
using System.Threading.Tasks;
using System.IO;

namespace BangumiX
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal, Environment.SpecialFolderOption.Create);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            Bangumi.Api.BangumiApi.Init(folderPath,
                folderPath,
                async (s) => await Task.FromResult(System.Text.Encoding.Default.GetBytes(s)),
                async (b) => await Task.FromResult(System.Text.Encoding.Default.GetString(b)));
            DependencyService.Register<ProgressDataStore>();
            DependencyService.Register<CollectionDataStore>();
            DependencyService.Register<CalendarDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected async override void OnSleep()
        {
            await Bangumi.Api.BangumiApi.BgmCache.WriteToFile();
        }

        protected override void OnResume()
        {
        }
    }
}
