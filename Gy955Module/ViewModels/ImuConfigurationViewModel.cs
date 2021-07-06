using Prism.Events;
using SharpCommunication.Codec;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport.SerialPort;

namespace Gy955Module.ViewModels
{
    public class ImuConfigurationViewModel : SerialPortTransportConfigurationViewModel<Gy955>
    {
        private readonly IEventAggregator _eventAggregator;

        public ImuConfigurationViewModel(SerialPortDataTransport<Gy955> dataTransport, IEventAggregator eventAggregator) : base(dataTransport)
        {
            _eventAggregator = eventAggregator;
        }
    }
}
