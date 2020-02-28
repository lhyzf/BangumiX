using Bangumi.Api.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace BangumiX.Converters
{
    public class ActorListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var actors = value as List<Actor>;
            if (actors != null && actors.Count != 0)
            {
                return "CV：" + string.Join("、", actors.Select(a => a.Name));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
