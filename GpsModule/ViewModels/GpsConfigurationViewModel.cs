using Prism.Events;
using SharpCommunication.Codec;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.ViewModels
{
    public class GpsConfigurationViewModel : SerialPortTransportConfigurationViewModel<Gps>
    {
        private readonly IEventAggregator _eventAggregator;

        public GpsConfigurationViewModel(SerialPortDataTransport<Gps> dataTransport, IEventAggregator eventAggregator) : base(dataTransport)
        {
            _eventAggregator = eventAggregator;
        }
    }
}
