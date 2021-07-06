using SharpCommunication.Codec;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport.SerialPort;

namespace ImuModule.ViewModels
{
    public class ImuServiceToolbarViewModel : TransportViewModel<Gy955>
    {
        public ImuServiceToolbarViewModel(SerialPortDataTransport<Gy955> dataTransport) : base(dataTransport)
        {

        }
    }
}
