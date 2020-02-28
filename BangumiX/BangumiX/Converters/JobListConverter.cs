using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace BangumiX.Converters
{
    public class JobListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var jobs = value as List<string>;
            if (jobs != null && jobs.Count != 0)
            {
                return string.Join("、", jobs);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
