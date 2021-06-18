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
using SharpCommunication.GY955.Codec;
using SharpCommunication.Transport.SerialPort;

namespace ImuModule.ViewModels
{
    public class CachedChannelViewModel : BindableBase, INavigationAware
    {
        public class ChannelInfo : BindableBase
        {
            private DateTime lastPacketTime;
            private DateTime firstPacketTime;
            private int dataReceivedCount;
            private ObservableCollection<PacketCacheInfo<Gy955>> internalPacketsList;
            public ChannelInfo(IChannel<Gy955> channel)
            {
                
                Channel = channel;
                var Cached = channel.ToCachedChannel();
                var Monitored = channel.ToMonitoredChannel();
                Monitored.DataReceived += (sender, e) =>
                {
                    Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        DataReceivedCount = Monitored.DataReceivedCount;
                        FirstPacketTime = Monitored.FirstPacketTime;
                        LastPacketTime = Monitored.LastPacketTime;
                    });
                };
                ((INotifyCollectionChanged)Cached.Packet).CollectionChanged += (sender, e) =>
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
                 };
                internalPacketsList = new ObservableCollection<PacketCacheInfo<Gy955>>();
                PacketsList = new ReadOnlyObservableCollection<PacketCacheInfo<Gy955>>(internalPacketsList);
            }
            public ReadOnlyObservableCollection<PacketCacheInfo<Gy955>> PacketsList { get; set; }
            public int DataReceivedCount { get => dataReceivedCount; set => SetProperty(ref dataReceivedCount, value); }
            public DateTime FirstPacketTime { get => firstPacketTime; set => SetProperty(ref firstPacketTime, value); }
            public DateTime LastPacketTime { get => lastPacketTime; set => SetProperty(ref lastPacketTime, value); }
            public IChannel<Gy955> Channel { get; set; }
        }
        private readonly SerialPortDataTransport<Gy955> _dataTransport;
        private bool _loaded;
        private List<ChannelInfo> _channelInfosList;


        public CachedChannelViewModel(SerialPortDataTransport<Gy955> dataTransport)
        {
            _dataTransport = dataTransport;
            ((INotifyCollectionChanged)dataTransport.Channels).CollectionChanged += (sender, args) =>
           {
               ChannelInfosList = dataTransport.Channels.Select(o => new ChannelInfo(o)).ToList();

           };
            _dataTransport.IsOpenChanged += (sender, args) =>
                RaisePropertyChanged(nameof(IsOpen));
            Loaded = false;
        }

        public List<ChannelInfo> ChannelInfosList { get => _channelInfosList; private set => SetProperty(ref _channelInfosList, value); }

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
