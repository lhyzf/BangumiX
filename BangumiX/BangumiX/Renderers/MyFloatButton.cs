using System;
using Xamarin.Forms;

namespace BangumiX.Renderers
{
    public class MyFloatButton : View
    {
        public Command Command
        {
            get => (Command)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(Command), typeof(MyFloatButton), null);
    }
}
