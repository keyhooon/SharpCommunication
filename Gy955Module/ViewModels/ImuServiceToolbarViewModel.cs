using SharpCommunication.Codec;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport.SerialPort;

namespace Gy955Module.ViewModels
{
    public class ImuServiceToolbarViewModel : TransportViewModel<Gy955>
    {
        public ImuServiceToolbarViewModel(SerialPortDataTransport<Gy955> dataTransport) : base(dataTransport)
        {

        }
    }
}
