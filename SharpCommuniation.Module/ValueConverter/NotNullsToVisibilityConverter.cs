using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SharpCommunication.Module.ValueConverter
{
    public class NotNullsToVisibilityConverter : IMultiValueConverter
    {
        public bool Invert { get; set; } = false;
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (Invert)
                return (values.All(o=>o == null)) ? Visibility.Visible : Visibility.Collapsed;
            else
                return (values.All(o => o != null)) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
