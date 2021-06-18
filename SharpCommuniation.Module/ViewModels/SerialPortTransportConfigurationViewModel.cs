using System.IO.Ports;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport.SerialPort;

namespace SharpCommunication.Module.ViewModels
{
    public class SerialPortTransportConfigurationViewModel<T> : TransportViewModel<T> where T : IPacket
    {
        private readonly SerialPortDataTransport<T> _dataTransport;

        private string _portName;
        private int _baudRate;
        private Parity _parity;
        private StopBits _stopBits;
        private int _dataBits;
        private int _readTimeout;

        private readonly SerialPortDataTransportSettings serialPortDataTransportSettings;

        protected SerialPortTransportConfigurationViewModel(SerialPortDataTransport<T> dataTransport ) :base(dataTransport)
        {
            serialPortDataTransportSettings = (SerialPortDataTransportSettings)dataTransport.Settings;
            PortName = serialPortDataTransportSettings.PortName;
            BaudRate = serialPortDataTransportSettings.BaudRate;
            Parity = serialPortDataTransportSettings.Parity;
            StopBits = serialPortDataTransportSettings.StopBits;
            DataBits = serialPortDataTransportSettings.DataBits;
            ReadTimeout = serialPortDataTransportSettings.ReadTimeout;
        }



        public string PortName
        {
            get => _portName;
            set => SetProperty(ref _portName , value,
                () => { serialPortDataTransportSettings.PortName = value;});
        }

        public int BaudRate
        {
            get => _baudRate;
            set => SetProperty(ref _baudRate, value,
                () => { serialPortDataTransportSettings.BaudRate = value;});
        }

        public Parity Parity
        {
            get => _parity;
            set => SetProperty(ref _parity , value,
                () => { serialPortDataTransportSettings.Parity = value;});
        }

        public StopBits StopBits
        {
            get => _stopBits;
            set => SetProperty(ref _stopBits , value,
                () => { serialPortDataTransportSettings.StopBits = value;});
        }
        public int DataBits
        {
            get => _dataBits;
            set => SetProperty(ref _dataBits, value,
                () => { serialPortDataTransportSettings.DataBits = value;});
        }
        public int ReadTimeout
        {
            get => _readTimeout;
            set => SetProperty(ref _readTimeout , value,
                () => { serialPortDataTransportSettings.ReadTimeout = value;});
        }



    }
}
