using System;
using System.IO.Ports;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport.SerialPort;

namespace SharpCommunication.Module.ViewModels
{
    public class TransportConfigurationViewModel<T> : BindableBase where T : IPacket
    {
        private readonly SerialPortDataTransport<T> _dataTransport;

        private string _portName;
        private int _baudRate;
        private Parity _parity;
        private StopBits _stopBits;
        private int _dataBits;
        private int _readTimeout;
        private DelegateCommand _openCommand;
        private DelegateCommand _closeCommand;
        private SerialPortDataTransportSettings serialPortDataTransportSettings;
        public TransportConfigurationViewModel(SerialPortDataTransport<T> dataTransport )
        {
            _dataTransport = dataTransport;
            _dataTransport.CanOpenChanged += delegate { OpenCommand.RaiseCanExecuteChanged();  };
            _dataTransport.CanCloseChanged += delegate { CloseCommand.RaiseCanExecuteChanged(); };
            
            serialPortDataTransportSettings = (SerialPortDataTransportSettings) _dataTransport.Settings;
            PortName = serialPortDataTransportSettings.PortName;
            BaudRate = serialPortDataTransportSettings.BaudRate;
            Parity = serialPortDataTransportSettings.Parity;
            StopBits = serialPortDataTransportSettings.StopBits;
            DataBits = serialPortDataTransportSettings.DataBits;
            ReadTimeout = serialPortDataTransportSettings.ReadTimeout;
            _dataTransport.IsOpenChanged += (sender, args) =>
            {
                RaisePropertyChanged(nameof(IsOpen));
            };
        }

        public bool IsOpen => _dataTransport.IsOpen;

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

        public DelegateCommand OpenCommand =>
            _openCommand ??= new DelegateCommand(_dataTransport.Open, () => _dataTransport.CanOpen);
        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(_dataTransport.Close, () => _dataTransport.CanClose);

    }
}
