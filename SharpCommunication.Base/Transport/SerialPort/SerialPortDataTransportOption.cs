using System.IO.Ports;

namespace SharpCommunication.Transport.SerialPort
{
    public class SerialPortDataTransportOption : DataTransportOption
    {
        private Parity _parity;
        private int _baudRate;
        private string _portName;
        private StopBits _stopBits;
        private int _dataBits;
        private int _readTimeout;

        public SerialPortDataTransportOption(string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, int readTimeout = 1000)
        {
            _stopBits = stopBits;
            _dataBits = dataBits;
            _portName = portName;
            _baudRate = baudRate;
            _parity = parity;
            _readTimeout = readTimeout;
        }

        public string PortName { get => _portName;
            set
            {
                if (_portName == value)
                    return;
                _portName = value;
                OnPropertyChanged();
            }
        }

        public int BaudRate { get => _baudRate;
            set
            {
                if (_baudRate == value)
                    return;
                _baudRate = value;
                OnPropertyChanged();
            }
        }

        public Parity Parity { get => _parity;
            set
            {
                if (_parity == value)
                    return;
                _parity = value;
                OnPropertyChanged();
            }
        }

        public StopBits StopBits
        {
            get => _stopBits;
            set
            {
                if (_stopBits == value)
                    return;
                _stopBits = value;
                OnPropertyChanged();
            }
        }

        public int DataBits
        {
            get => _dataBits;
            set
            {
                if (_dataBits == value)
                    return;
                _dataBits = value;
                OnPropertyChanged();
            } 
        }

        public int ReadTimeout 
        { 
            get => _readTimeout; 
            set {
                if (_readTimeout == value)
                    return;
                _readTimeout = value;
                OnPropertyChanged();
            } 
        }


    }
}
