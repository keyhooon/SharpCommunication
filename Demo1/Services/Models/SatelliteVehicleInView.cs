using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSModule.Services.Models
{
    public class SatelliteVehicleInView : Satellite
    {
        public SatelliteVehicleInView(int satelliteId, double elevation, double azimuth, int signalToNoiseRatio = -1) :base(satelliteId)
        {

            Elevation = elevation;
            Azimuth = azimuth;
            SignalToNoiseRatio = signalToNoiseRatio;
        }

        /// <summary>
        /// Elevation in degrees, 90 maximum
        /// </summary>
        public double Elevation { get; }

        /// <summary>
        /// Azimuth, degrees from true north, 000 to 359
        /// </summary>
        public double Azimuth { get; }

        /// <summary>
        /// Signal-to-Noise ratio, 0-99 dB (-1 when not tracking) 
        /// </summary>
        public int SignalToNoiseRatio { get; }

    }


}
