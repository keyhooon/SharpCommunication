using SharpCommunication.Codec;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.ViewModels
{
    public class GpsServiceToolbarViewModel : TransportViewModel<Gps>
    {
        public GpsServiceToolbarViewModel(SerialPortDataTransport<Gps> dataTransport) : base(dataTransport)
        {

        }
    }
}
