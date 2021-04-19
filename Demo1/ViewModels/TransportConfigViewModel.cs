using System.Collections.ObjectModel;
using System.IO.Ports;
using GPSModule.Events;
using GPSModule.Services;
using MaterialDesignUnityBootStrap.Services.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Module.ViewModels;
using SharpCommunication.Transport;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.ViewModels
{
    public class TransportConfigViewModel : TransportConfigurationViewModel<Gps>
    {
        private readonly IEventAggregator _eventAggregator;

        public TransportConfigViewModel(SerialPortDataTransport<Gps> dataTransport, IEventAggregator eventAggregator) : base(dataTransport)
        {
            _eventAggregator = eventAggregator;
        }
    }
}
