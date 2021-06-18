using System;
using System.Globalization;
using System.Windows.Data;
using NetTopologySuite.Geometries;
using Location = MapControl.Location;

namespace GPSModule.XamlMap
{
    public class DbGeographyToLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Point)value;
            return val != null ? new Location (val.X, val.Y) : new Location();
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            var val = (Location)value;

            return val != null ? new Point(val.Latitude, val.Longitude) : new Point(0, 0);
        }


    }
}