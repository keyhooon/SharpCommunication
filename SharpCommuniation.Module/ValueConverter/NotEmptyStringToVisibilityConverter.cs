using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SharpCommunication.Module.ValueConverter
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NotEmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is string && ((string)value) != string.Empty) ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
