using GPSModule.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SharpCommunication.Codec;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport;
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
