using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BangumiX.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabeledSwitch : ContentView
    {
        public static readonly BindableProperty TitleProperty =
               BindableProperty.Create(nameof(Title), typeof(string), typeof(LabeledSwitch));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }


        public static readonly BindableProperty OnTextProperty =
            BindableProperty.Create(nameof(OnText), typeof(string), typeof(LabeledSwitch));

        public string OnText
        {
            get => (string)GetValue(OnTextProperty);
            set => SetValue(OnTextProperty, value);
        }

        public static readonly BindableProperty OffTextProperty =
            BindableProperty.Create(nameof(OffText), typeof(string), typeof(LabeledSwitch));

        public string OffText
        {
            get => (string)GetValue(OffTextProperty);
            set => SetValue(OffTextProperty, value);
        }


        public static readonly BindableProperty IsOnProperty =
            BindableProperty.Create(nameof(IsOn), typeof(bool), typeof(LabeledSwitch));

        public bool IsOn
        {
            get => (bool)GetValue(IsOnProperty);
            set 
            { 
                SetValue(IsOnProperty, value);
                OnPropertyChanged(nameof(IsOff));
            }
        }

        public bool IsOff => !IsOn;

        public LabeledSwitch()
        {
            InitializeComponent();
        }

        private void OnTapped(object sender, EventArgs e)
        {
            IsOn = !IsOn;
        }
    }
}