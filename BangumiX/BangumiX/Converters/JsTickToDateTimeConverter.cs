using Bangumi.Api.Common;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BangumiX.Converters
{
    public class JsTickToDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var datetime = int.Parse(value.ToString());
            return datetime.ToDateTime().ToString("yyyy-MM-dd HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
