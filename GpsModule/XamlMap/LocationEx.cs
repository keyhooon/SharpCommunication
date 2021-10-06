using System;
using System.Text;
using MapControl;

namespace GPSModule.XamlMap
{
    public static class  LocationEx
    {
        public static string GetPrettyString(this Location val)
        {
            var latsign = val.Latitude >= 0;
            var lonsign = val.Longitude >= 0;
            var location = new Location(Math.Abs(val.Latitude), Math.Abs(val.Longitude));

            var lat1 = decimal.ToInt32((decimal)location.Latitude);
            Math.DivRem(decimal.ToInt32((decimal)location.Latitude * 60), 60, out var lat2);
            Math.DivRem(decimal.ToInt32((decimal)location.Latitude * 3600), 60, out var lat3);
            var lon1 = decimal.ToInt32((decimal)location.Longitude);
            Math.DivRem(decimal.ToInt32((decimal)location.Longitude * 60), 60, out var lon2);
            Math.DivRem(decimal.ToInt32((decimal)location.Longitude * 3600), 60, out var lon3);

            var sb = new StringBuilder($"{lat1}°    {lat2}'   {lat3}\" ");
            var latSignString = latsign ? "N" : "S";
            sb.Append($"    {lon1}°    {lon2}'   {lon3}\" ");
            var lonSignString = lonsign ? "E" : "W";
            return $"{lat1:000}°  {lat2:00}'  {lat3:00}\"{latSignString}  {lon1:000}°  {lon2:00}'  {lon3:00}\"{lonSignString}";
        }
    }
}
