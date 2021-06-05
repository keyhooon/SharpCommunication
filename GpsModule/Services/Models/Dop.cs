using SharpCommunication.Codec;

namespace GPSModule.Services.Models
{
    public class Dop
    {
        /// <summary>
        /// Mode
        /// </summary>
        public Gsa.ModeSelection GpsMode { get; private set; }

        /// <summary>
        /// Mode
        /// </summary>
        public Gsa.Mode FixMode { get; private set; }

        /// <summary>
        /// Dilution of precision
        /// </summary>
        public double Pdop { get; private set; }

        /// <summary>
        /// Vertical dilution of precision
        /// </summary>
        public double Vdop { get; private set; }

        /// <summary>
        /// Horizontal Dilution of Precision
        /// </summary>
        public double Hdop { get; private set; }

        public Dop(double hdop = double.NaN, double vdop = double.NaN, double pdop = double.NaN, Gsa.Mode fixMode = Gsa.Mode.NotAvailable, Gsa.ModeSelection gpsMode = Gsa.ModeSelection.Manual)
        {
            Hdop = hdop;
            Vdop = vdop;
            Pdop = pdop;
            FixMode = fixMode;
            GpsMode = gpsMode;
        }
    }


}
