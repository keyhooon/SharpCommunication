using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace SharpCommunication.Module.ValueConverter
{
    internal class NumberToStringConverter : IValueConverter
    {
        private readonly string[] _unit;

        public NumberToStringConverter()
        {
            _unit =new [] { "G", "M", "k", "", "m", "u", "n", "p" }; 
        }

        public string MeasurmentUnit { get; set; } = "Hz";
        public string Format { get; set; } = "0.000000";
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var number = (double)value;
            string str;
            if (number >= 1.0E9 || number <= -1.0E9)
            {
                str = $"G{MeasurmentUnit}";
                number /= 1.0E9;
            }
            else if (number >= 1.0E6 || number <= -1.0E6)
            {
                str = $"M{MeasurmentUnit}";
                number /= 1.0E6;
            }
            else if (number >= 1.0E3 || number <= -1.0E3)
            {
                str = $"k{MeasurmentUnit}";
                number /= 1.0E3;
            }
            else if (number >= 1.0E0 || number <= -1.0E0)
            {
                str = $"{MeasurmentUnit}";
                number /= 1.0E0;
            }
            else if (number >= 1.0E-3 || number <= -1.0E-3)
            {
                str = $"m{MeasurmentUnit}";
                number /= 1.0E-3;
            }
            else if (number >= 1.0E-6 || number <= -1.0E-6)
            {
                str = $"u{MeasurmentUnit}";
                number /= 1.0E-6;
            }
            else if (number >= 1.0E-9 || number <= -1.0E-9)
            {
                str = $"n{MeasurmentUnit}";
                number /= 1.0E-9;
            }
            else
            {
                str = $"{MeasurmentUnit}";
                number = 0;
            }
            return string.Format($"{{0:{Format}}} {str}", number);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;
            var pattern = @"([-+]|[+]*)?([0-9]*\.[0-9]+|[0-9]+)\s*([A-z]*)";
            var arr = Regex.Split(text, pattern).ToArray();

            if (double.TryParse(arr[2], out var val))
            {
                if (arr[3] != string.Empty)
                {
                    switch (arr[3].First())
                    {
                        case 'G':
                            val *= 1.0E9;
                            break;
                        case 'M':
                            val *= 1.0E6;
                            break;
                        case 'k':
                            val *= 1.0E3;
                            break;
                        case 'm':
                            val *= 1.0E-3;
                            break;
                        case 'u':
                            val *= 1.0E-6;
                            break;
                        case 'n':
                            val *= 1.0E-9;
                            break;
                    }
                    return val;
                }
                val *= arr[1] == "-" ? -1 : 1;
            }
            else
                val = 0;
            return val;
        }
    }
}
