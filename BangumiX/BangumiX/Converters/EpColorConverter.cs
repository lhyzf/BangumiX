using Bangumi.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BangumiX.Converters
{
    public class EpColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var epStatus = (EpStatusType)value;
            switch (epStatus)
            {
                case EpStatusType.watched:
                    return Color.LightSkyBlue;
                case EpStatusType.queue:
                    return Color.LightPink;
                case EpStatusType.drop:
                    return Color.LightGray;
                case EpStatusType.remove:
                    return Color.FromHex("#fafafa");
                default:
                    return Color.FromHex("#fafafa");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
