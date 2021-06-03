using Prism.Commands;
using Prism.Mvvm;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;
using SharpCommunication.Transport.SerialPort;

namespace SharpCommunication.Module.ViewModels
{
    public abstract class TransportViewModel<T> : BindableBase where T : IPacket
    {
        private DelegateCommand _openCommand;
        private DelegateCommand _closeCommand;

        private readonly DataTransport<T> _dataTransport;

        public TransportViewModel(DataTransport<T> dataTransport)
        {
            _dataTransport = dataTransport;
            _dataTransport.CanOpenChanged += delegate { OpenCommand.RaiseCanExecuteChanged(); };
            _dataTransport.CanCloseChanged += delegate { CloseCommand.RaiseCanExecuteChanged(); };

            
            _dataTransport.IsOpenChanged += (sender, args) =>
            {
                RaisePropertyChanged(nameof(IsOpen));
            };
        }
        public DelegateCommand OpenCommand =>
            _openCommand ??= new DelegateCommand(()=>
            {
                _dataTransport.Open();
            }, () => _dataTransport.CanOpen);
        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(_dataTransport.Close, () => _dataTransport.CanClose);

        public bool IsOpen => _dataTransport.IsOpen;
    }
}
