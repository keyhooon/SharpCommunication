using System;
using SharpCommunication.Codec;

namespace GPSModule.Models
{
    public class GnssData
    {
        public GnssData(TimeSpan universalTimeCoordinated,
           double latitude = double.NaN,
           double longitude = double.NaN,
           Gns.Mode modeIndicator = Gns.Mode.NoFix,
           double hdop = double.NaN,
           int numberOfSatellites = 0,
           double altitude = double.NaN,
           double heightOfGeoid = double.NaN,
           TimeSpan timeSinceLastDgpsUpdate = default,
           int dgpsStationId = 0, 
           Gns.NavigationalStatus navigationalStatus = Gns.NavigationalStatus.NotValid)
        {
            UniversalTimeCoordinated = universalTimeCoordinated;
            Latitude = latitude;
            Longitude = longitude;
            ModeIndicator = modeIndicator;
            Hdop = hdop;
            NumberOfSatellites = numberOfSatellites;
            Altitude = altitude;
            HeightOfGeoid = heightOfGeoid;
            TimeSinceLastDgpsUpdate = timeSinceLastDgpsUpdate;
            DgpsStationId = dgpsStationId;
            NavigationalStatus = navigationalStatus;
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
        public Gns.Mode ModeIndicator{ get; private set; }

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
        /// Height of geoid (mean sea level) above WGS84
        /// </summary>
        public double HeightOfGeoid { get; private set; }

        /// <summary>
        /// Time since last DGPS update
        /// </summary>
        public TimeSpan TimeSinceLastDgpsUpdate { get; private set; }

        /// <summary>
        /// DGPS Station ID Number
        /// </summary>
        public int DgpsStationId { get; private set; }

        /// <summary>
        /// Navigational status
        /// </summary>
        public Gns.NavigationalStatus NavigationalStatus { get; private set; }
    }
}
