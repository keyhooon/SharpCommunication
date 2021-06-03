using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class CachedChannelViewModel : BindableBase, INavigationAware
    {
        public class CachedMonitoredChannel
        {
            public CachedMonitoredChannel(IChannel<Gps> channel, CachedChannel<Gps> cached, MonitoredChannel<Gps> monitored)
            {
                Cached = cached;
                Channel = channel;
                Monitored = monitored;
            }
            public IChannel<Gps> Channel { get; set; }
            public CachedChannel<Gps> Cached { get; set; }
            public MonitoredChannel<Gps> Monitored { get; set; }

        }
        private readonly SerialPortDataTransport<Gps> _dataTransport;
        private bool _loaded;
        private List<CachedMonitoredChannel> _channelsList;
        private CachedMonitoredChannel channel;

        public CachedChannelViewModel(SerialPortDataTransport<Gps> dataTransport)
        {
            _dataTransport = dataTransport;
            ((INotifyCollectionChanged)dataTransport.Channels).CollectionChanged += (sender, args) =>
           {
               ChannelsList = dataTransport.Channels.Select(o => new CachedMonitoredChannel(o, o.ToCachedChannel(), o.ToMonitoredChannel())).ToList();
               Channel = ChannelsList.FirstOrDefault();
           };
            _dataTransport.IsOpenChanged += (sender, args) =>
                RaisePropertyChanged(nameof(IsOpen));
            Loaded = false;
        }

        public List<CachedMonitoredChannel> ChannelsList { get => _channelsList; private set => SetProperty(ref _channelsList, value); }

        public CachedMonitoredChannel Channel { get => channel; set => channel = value; }
        public bool IsOpen => _dataTransport.IsOpen;

        public bool Loaded
        {
            get => _loaded;
            set => SetProperty(ref _loaded, value);
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
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
