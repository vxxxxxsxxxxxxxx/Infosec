using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Dnevnik
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolenToVisibilityInverseConverter : IValueConverter
    {
        enum Parameters
        {
            Normal, Inverted
        }

        public object ConvertBack(object value)
        {
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
