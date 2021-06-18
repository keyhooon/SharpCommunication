using SharpCommunication.GY955.Codec;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport.SerialPort;

namespace ImuModule.ViewModels
{
    public class GpsServiceToolbarViewModel : TransportViewModel<Gy955>
    {
        public GpsServiceToolbarViewModel(SerialPortDataTransport<Gy955> dataTransport) : base(dataTransport)
        {

        }
    }
}
