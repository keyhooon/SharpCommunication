using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SharpCommunication.Codec;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.ViewModels
{
    public class ServiceToolbar1ViewModel : BindableBase
    {
        private readonly SerialPortDataTransport<Gps> _dataTransport;
        private readonly IEventAggregator _eventAggregator;
        private DelegateCommand _openCommand;
        private DelegateCommand _closeCommand;
        
        public ServiceToolbar1ViewModel(SerialPortDataTransport<Gps> dataTransport,IEventAggregator eventAggregator)
        {
            _dataTransport = dataTransport;
            _eventAggregator = eventAggregator;
            _dataTransport.CanOpenChanged += delegate { OpenCommand.RaiseCanExecuteChanged(); };
            _dataTransport.CanCloseChanged += delegate { CloseCommand.RaiseCanExecuteChanged(); };
            _dataTransport.IsOpenChanged += delegate { RaisePropertyChanged(nameof(IsOpen)); };
        }
        
        public DelegateCommand OpenCommand =>
            _openCommand ??= new DelegateCommand(_dataTransport.Open, () => _dataTransport.CanOpen);
        public DelegateCommand CloseCommand =>
            _closeCommand ??= new DelegateCommand(_dataTransport.Close, () => _dataTransport.CanClose);
        
        public bool IsOpen => _dataTransport.IsOpen;
    }
}
