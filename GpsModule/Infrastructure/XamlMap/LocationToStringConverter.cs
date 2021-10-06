using System;
using System.Globalization;
using System.Windows.Data;
using MapControl;

namespace GPS_Module.Infrastructure.XamlMap
{
    public class LocationToStringConverter : IValueConverter
    {
        public bool IsVertical { get; set; } = true;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Location val = (Location)value;
            return val.GetPrettyString();
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }


    }
}
