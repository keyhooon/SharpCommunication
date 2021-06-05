using SharpCommunication.Codec;
using System;

namespace GPSModule.Services.Models
{
    public class GpsData
    {
        public GpsData(TimeSpan universalTimeCoordinated,
            double latitude = double.NaN,
            double longitude = double.NaN,
            Gga.FixQuality status = Gga.FixQuality.Invalid,
            double hdop = double.NaN,
            int numberOfSatellites = 0, 
            double altitude = double.NaN,
            string altitudeUnits = "m",
            double heightOfGeoid = double.NaN,
            string heightOfGeoidUnits = "m",
            TimeSpan timeSinceLastDgpsUpdate = default,
            int dgpsStationId = 0)
        {
            UniversalTimeCoordinated = universalTimeCoordinated;
            Latitude = latitude;
            Longitude = longitude;
            Status = status;
            Hdop = hdop;
            AltitudeUnits = altitudeUnits;
            HeightOfGeoid = heightOfGeoid;
            HeightOfGeoidUnits = heightOfGeoidUnits;
            TimeSinceLastDgpsUpdate = timeSinceLastDgpsUpdate;
            DgpsStationId = dgpsStationId;
            NumberOfSatellites = numberOfSatellites;
            Altitude = altitude;
        }
        /// <summary>
        /// Time of day fix was taken
        /// </summary>
        public TimeSpan UniversalTimeCoordinated { get; private set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Longitude { get; private set; }


        /// <summary>
        /// Positioning system Mode Indicator
        /// </summary>
        public Gga.FixQuality Status { get; private set; }

                /// <summary>
        /// Horizontal Dilution of Precision
        /// </summary>
        public double Hdop { get; private set; }

        /// <summary>
        /// Number of satellites being tracked
        /// </summary>
        public int NumberOfSatellites { get; private set; }

        /// <summary>
        /// Altitude
        /// </summary>
        public double Altitude { get; private set; }
        /// <summary>
        /// Altitude units ('M' for Meters)
        /// </summary>
        public string AltitudeUnits { get; private set; }

        /// <summary>
        /// Height of geoid (mean sea level) above WGS84
        /// </summary>
        public double HeightOfGeoid { get; private set; }

        /// <summary>
        /// Altitude units ('M' for Meters)
        /// </summary>
        public string HeightOfGeoidUnits { get; private set; }

        /// <summary>
        /// Time since last DGPS update
        /// </summary>
        public TimeSpan TimeSinceLastDgpsUpdate { get; private set; }

        /// <summary>
        /// DGPS Station ID Number
        /// </summary>
        public int DgpsStationId { get; private set; }

    }


}
