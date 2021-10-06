using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GPS_Module.Infrastructure.XamlMap
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility;
            if (string.IsNullOrEmpty((string)values))
            {
                visibility = Invert ? Visibility.Collapsed: Visibility.Visible;
            }
            else
                visibility = Invert? Visibility.Visible: Visibility.Collapsed;

            return visibility;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
