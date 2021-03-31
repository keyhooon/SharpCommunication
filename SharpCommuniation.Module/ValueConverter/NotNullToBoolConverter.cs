using System;
using System.Globalization;
using System.Windows.Data;

namespace SharpCommunication.Module.ValueConverter
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class NotNullToBoolConverter : IValueConverter
    {
        public bool Invert { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Invert)
                return (value == null);
            return (value != null);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
