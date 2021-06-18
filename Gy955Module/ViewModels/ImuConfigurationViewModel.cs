using Prism.Events;
using SharpCommunication.GY955.Codec;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport.SerialPort;

namespace ImuModule.ViewModels
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
