using System;
using System.IO.Ports;
using Prism.Commands;
using Prism.Mvvm;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;

namespace SharpCommunication.Module.ViewModels
{
    public class TransportConfigurationViewModel<T> : BindableBase where T : IPacket, new()
    {
        private readonly DataTransport<T> _dataTransport;

        private string _portName;
        private int _baudRate;
        private Parity _parity;
        private StopBits _stopBits;
        private int _dataBits;
        private int _readTimeout;
        private DelegateCommand _openCommand;

        public TransportConfigurationViewModel(DataTransport<T> dataTransport )
        {
            _dataTransport = dataTransport;
            _dataTransport.CanOpenChanged += delegate { OpenCommand.RaiseCanExecuteChanged();  };
            _dataTransport.CanCloseChanged += delegate { CloseCommand.RaiseCanExecuteChanged(); };
        }

        public string PortName
        {
            get => _portName;
            set => SetProperty(ref _portName , value);
        }

        public int BaudRate
        {
            get => _baudRate;
            set => SetProperty(ref _baudRate, value);
        }

        public Parity Parity
        {
            get => _parity;
            set => SetProperty(ref _parity , value);
        }

        public StopBits StopBits
        {
            get => _stopBits;
            set => SetProperty(ref _stopBits , value);
        }
        public int DataBits
        {
            get => _dataBits;
            set => SetProperty(ref _dataBits, value);
        }
        public int ReadTimeout
        {
            get => _readTimeout;
            set => SetProperty(ref _readTimeout , value);
        }

        public DelegateCommand OpenCommand =>
            _openCommand ??= new DelegateCommand(_dataTransport.Open, () => _dataTransport.CanOpen);
        public DelegateCommand CloseCommand =>
            _openCommand ??= new DelegateCommand(_dataTransport.Close, () => _dataTransport.CanClose);

    }
}
