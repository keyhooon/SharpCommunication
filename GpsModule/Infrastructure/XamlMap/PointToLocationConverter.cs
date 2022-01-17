using System;
using System.Globalization;
using System.Windows.Data;
using NetTopologySuite.Geometries;
using Location = MapControl.Location;

namespace GPSModule.Infrastructure.XamlMap
{
    public class PointToLocationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (Point)value;
            return val == null ? new Location() : new Location (val.X, val.Y);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            var val = (Location)value;
            return val == null ? new Point(0, 0) : new Point(val.Latitude, val.Longitude){ SRID = 4326 };
        }
    }
}