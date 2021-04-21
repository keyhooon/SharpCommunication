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

        public TransportConfigurationViewModel(SerialPortDataTransport<T> dataTransport )
        {
            _dataTransport = dataTransport;
            _dataTransport.CanOpenChanged += delegate { OpenCommand.RaiseCanExecuteChanged();  };
            _dataTransport.CanCloseChanged += delegate { CloseCommand.RaiseCanExecuteChanged(); };
            PortName = ((SerialPortDataTransportOption) _dataTransport.Option).PortName;
            BaudRate = ((SerialPortDataTransportOption)_dataTransport.Option).BaudRate;
            Parity = ((SerialPortDataTransportOption) _dataTransport.Option).Parity;
            StopBits = ((SerialPortDataTransportOption)_dataTransport.Option).StopBits;
            DataBits = ((SerialPortDataTransportOption)_dataTransport.Option).DataBits;
            ReadTimeout = ((SerialPortDataTransportOption)_dataTransport.Option).ReadTimeout;
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
                () => { ((SerialPortDataTransportOption) _dataTransport.Option).PortName = value;});
        }

        public int BaudRate
        {
            get => _baudRate;
            set => SetProperty(ref _baudRate, value,
                () => { ((SerialPortDataTransportOption) _dataTransport.Option).BaudRate = value;});
        }

        public Parity Parity
        {
            get => _parity;
            set => SetProperty(ref _parity , value,
                () => { ((SerialPortDataTransportOption) _dataTransport.Option).Parity = value;});
        }

        public StopBits StopBits
        {
            get => _stopBits;
            set => SetProperty(ref _stopBits , value,
                () => { ((SerialPortDataTransportOption) _dataTransport.Option).StopBits = value;});
        }
        public int DataBits
        {
            get => _dataBits;
            set => SetProperty(ref _dataBits, value,
                () => { ((SerialPortDataTransportOption) _dataTransport.Option).DataBits = value;});
        }
        public int ReadTimeout
        {
            get => _readTimeout;
            set => SetProperty(ref _readTimeout , value,
                () => { ((SerialPortDataTransportOption) _dataTransport.Option).ReadTimeout = value;});
        }

        public DelegateCommand OpenCommand =>
            _openCommand ??= new DelegateCommand(_dataTransport.Open, () => _dataTransport.CanOpen);
        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(_dataTransport.Close, () => _dataTransport.CanClose);

    }
}
