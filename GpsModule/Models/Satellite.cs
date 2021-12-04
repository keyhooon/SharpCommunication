using SharpCommunication.Codec;

namespace GPSModule.Models
{
    public class Satellite
    {
        public int Id { get; }

        public Satellite(int satelliteId)
        {
            Id = satelliteId;
        }

        /// <summary>
        /// Satellite system
        /// </summary>
        public SatelliteSystem System 
        {
            get
            {
                if (Id >= 1 && Id <= 32)
                    return SatelliteSystem.Gps;
                if (Id >= 33 && Id <= 64)
                    return SatelliteSystem.Waas;
                if (Id >= 65 && Id <= 96)
                    return SatelliteSystem.Glonass;
                return SatelliteSystem.Unknown;
            }
        }
    }


}
