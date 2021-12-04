using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using MapControl;

namespace GPSModule.Infrastructure.XamlMap
{
    public class LocationToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Hidden;

            if (values.Length != 2 || values[0] is not MapBase || values[1] is not Point) 
                return visibility;
            var parentMap = (MapBase)values[0];
            var position = (Point?)values[1];

            if (position.Value.X >= 0d && position.Value.X <= parentMap.ActualWidth &&
                position.Value.Y >= 0d && position.Value.Y <= parentMap.ActualHeight)
            {
                visibility = Visibility.Visible;
            }

            return visibility;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
