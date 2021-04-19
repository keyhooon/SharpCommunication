using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using SharpCommunication.Channels;
using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec;
using SharpCommunication.Transport.SerialPort;

namespace GPSModule.ViewModels
{
    public class CachedChannelViewModel: BindableBase, INavigationAware
    {
        private readonly SerialPortDataTransport<Gps> _dataTransport;
        private bool _loaded;



        public CachedChannelViewModel(SerialPortDataTransport<Gps> dataTransport)
        {
            _dataTransport = dataTransport;
            ((INotifyCollectionChanged) dataTransport.Channels).CollectionChanged += (sender, args) =>
            {
                RaisePropertyChanged(nameof(CachedChannelDecorator));
                RaisePropertyChanged(nameof(MonitoredChannelDecorator));
            };
            _dataTransport.IsOpenChanged += (sender, args) =>
                RaisePropertyChanged(nameof(IsOpen));
            Loaded = false;

        }

        public CachedChannel<Gps> CachedChannelDecorator => _dataTransport.Channels.FirstOrDefault()?.ToCachedChannel();
        public MonitoredChannel<Gps> MonitoredChannelDecorator => _dataTransport.Channels.FirstOrDefault()?.ToMonitoredChannel();
        public IChannel<Gps> ChannelDecorator => _dataTransport.Channels.FirstOrDefault();
        public bool IsOpen => _dataTransport.IsOpen;

        public bool Loaded
        {
            get => _loaded;
            set => SetProperty(ref _loaded, value);
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await Task.Delay(2000);
            Loaded = true;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
