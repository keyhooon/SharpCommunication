using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Prism.Mvvm;
using Prism.Regions;
using SharpCommunication.Channels;
using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec;
using SharpCommunication.Transport.SerialPort;

namespace Gy955Module.ViewModels
{
    public class CachedChannelViewModel : BindableBase, INavigationAware
    {
        public class ChannelInfo : BindableBase, IDisposable
        {
            private DateTime lastPacketTime;
            private DateTime firstPacketTime;
            private int dataReceivedCount;
            private ObservableCollection<PacketCacheInfo<Gy955>> internalPacketsList;
            public ChannelInfo(IChannel<Gy955> channel)
            {
                
                Channel = channel;
                Monitored = Channel.ToMonitoredChannel();
                Cached = Channel.ToCachedChannel();
                
                Monitored.DataReceived += Monitored_DataReceived;
                ((INotifyCollectionChanged)Cached.Packet).CollectionChanged += ChannelInfo_CollectionChanged;
                internalPacketsList = new ObservableCollection<PacketCacheInfo<Gy955>>();
                PacketsList = new ReadOnlyObservableCollection<PacketCacheInfo<Gy955>>(internalPacketsList);
            }
            public ReadOnlyObservableCollection<PacketCacheInfo<Gy955>> PacketsList { get; set; }
            public int DataReceivedCount { get => dataReceivedCount; set => SetProperty(ref dataReceivedCount, value); }
            public DateTime FirstPacketTime { get => firstPacketTime; set => SetProperty(ref firstPacketTime, value); }
            public DateTime LastPacketTime { get => lastPacketTime; set => SetProperty(ref lastPacketTime, value); }
            public MonitoredChannel<Gy955> Monitored { get; init; }
            public CachedChannel<Gy955> Cached { get; init; }
            public IChannel<Gy955> Channel { get; init; }

            private void Monitored_DataReceived(object sender, DataReceivedEventArg<Gy955> e)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    var Monitored = Channel.ToMonitoredChannel();
                    DataReceivedCount = Monitored.DataReceivedCount;
                    FirstPacketTime = Monitored.FirstPacketTime;
                    LastPacketTime = Monitored.LastPacketTime;
                });
            }

            private void ChannelInfo_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if ((e.NewItems?.Count ?? 0) != 0)
                    {
                        foreach (PacketCacheInfo<Gy955> item in e.NewItems)
                        {
                            internalPacketsList.Add(item);
                        }
                    }
                    if ((e.OldItems?.Count ?? 0) != 0)
                    {
                        foreach (PacketCacheInfo<Gy955> item in e.OldItems)
                        {
                            internalPacketsList.Remove(item);
                        }
                    }
                });
            }

            public void Dispose()
            {
                Monitored.Dispose();
                Cached.Dispose();
                Channel.Dispose();
                Monitored.DataReceived -= Monitored_DataReceived;
                ((INotifyCollectionChanged)Cached.Packet).CollectionChanged -= ChannelInfo_CollectionChanged;
                internalPacketsList.Clear();
                internalPacketsList = null;
                PacketsList = null;
            }
        }
        private readonly SerialPortDataTransport<Gy955> _dataTransport;
        private bool _loaded;
        private List<ChannelInfo> _channelInfosList;


        public CachedChannelViewModel(SerialPortDataTransport<Gy955> dataTransport)
        {
            _dataTransport = dataTransport;

            Loaded = false;
        }

        public List<ChannelInfo> ChannelInfosList { get => _channelInfosList; private set => SetProperty(ref _channelInfosList, value); }

        public bool IsOpen => _dataTransport.IsOpen;

        public bool Loaded
        {
            get => _loaded;
            set => SetProperty(ref _loaded, value);
        }

        private void CachedChannelViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ChannelInfosList = _dataTransport.Channels.Select(o => new ChannelInfo(o)).ToList();
        }

        private void _dataTransport_IsOpenChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(IsOpen));
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            
            ChannelInfosList = _dataTransport.Channels.Select(o => new ChannelInfo(o)).ToList();
            RaisePropertyChanged(nameof(IsOpen));
            ((INotifyCollectionChanged)_dataTransport.Channels).CollectionChanged += CachedChannelViewModel_CollectionChanged;
            _dataTransport.IsOpenChanged += _dataTransport_IsOpenChanged;
            Loaded = true;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            ((INotifyCollectionChanged)_dataTransport.Channels).CollectionChanged -= CachedChannelViewModel_CollectionChanged;
            _dataTransport.IsOpenChanged -= _dataTransport_IsOpenChanged;
            ChannelInfosList.ForEach(o=> o.Dispose());
            Loaded = false;
        }
    }
}
