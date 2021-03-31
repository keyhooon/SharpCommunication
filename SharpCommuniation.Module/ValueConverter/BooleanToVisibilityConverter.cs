using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SharpCommunication.Module.ValueConverter
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Inverse { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool ? ((bool)value == Inverse)? Visibility.Collapsed : Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
