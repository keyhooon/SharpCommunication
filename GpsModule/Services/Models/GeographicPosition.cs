using SharpCommunication.Codec;
using System;

namespace GPSModule.Services.Models
{
    public class GeographicPosition
    {
        public GeographicPosition(TimeSpan universalTimeCoordinated,
            double latitude = double.NaN,
            double longitude = double.NaN,
            bool dataActive = false, 
            Gll.Mode modeIndicator = Gll.Mode.DataNotValid)
        {
            UniversalTimeCoordinated = universalTimeCoordinated;
            Latitude = latitude;
            Longitude = longitude;
            DataActive = dataActive;
            ModeIndicator = modeIndicator;
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
        /// Gets a value indicating whether data is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if data is active; otherwise, <c>false</c>.
        /// </value>
        public bool DataActive { get; private set; }

        /// <summary>
        /// Positioning system Mode Indicator
        /// </summary>
        public Gll.Mode ModeIndicator { get; private set; }

     
    }


}
