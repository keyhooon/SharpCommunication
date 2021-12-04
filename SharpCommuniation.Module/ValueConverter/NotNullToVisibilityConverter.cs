using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SharpCommunication.Module.ValueConverter
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullableToVisibilityConverterConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Invert)
                return (value != null) ? Visibility.Collapsed: Visibility.Visible;
            else
                return (value != null) ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
