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
        public DetailViewModel viewModel;
        public DetailPage(DetailViewModel viewModel)
        {
            BindingContext = this.viewModel = viewModel;
            InitializeComponent();
        }
    }
}