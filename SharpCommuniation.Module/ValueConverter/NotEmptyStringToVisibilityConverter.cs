using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SharpCommunication.Module.ValueConverter
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NotEmptyStringToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Invert)
                return (value is "") ? Visibility.Visible : Visibility.Collapsed;

            else
                return (value is string s && s != string.Empty) ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
