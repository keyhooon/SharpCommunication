using System;
using System.ComponentModel;
using SharpCommunication.Channels.ChannelTools;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Channels.Decorator
{
    public class MonitoredChannel<TPacket> : ChannelDecorator<TPacket>, INotifyPropertyChanged where TPacket : IPacket
    {
        private DateTime lastPacketTime;
        private DateTime firstPacketTime;
        private int dataReceivedCount;

        private IoMonitor<TPacket> IoMonitor { get; set; }
        public MonitoredChannel(Channel<TPacket> innerChannel) : base(innerChannel)
        {
            IoMonitor = new IoMonitor<TPacket>(this);
        }

        public int DataReceivedCount { 
            get => dataReceivedCount; 
            set
            {
                dataReceivedCount = value;
                OnPropertyChanged(nameof(DataReceivedCount));
            }
        }
        public DateTime FirstPacketTime { 
            get => firstPacketTime; 
            set
            {
                firstPacketTime = value;
                OnPropertyChanged(nameof(FirstPacketTime));
            }
        }
        public DateTime LastPacketTime { 
            get => lastPacketTime; 
            set
            {
                lastPacketTime = value;
                OnPropertyChanged(nameof(LastPacketTime));
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public static class MonitoredChannelExtension
    {
        public static MonitoredChannel<TPacket> ToMonitoredChannel<TPacket>(this IChannel<TPacket> channel) where TPacket : IPacket
        {
            while (channel is ChannelDecorator<TPacket> item)
            {
                if (item is MonitoredChannel<TPacket>)
                    return (MonitoredChannel<TPacket>)item;
                channel = item.InnerChannel;
            }
            return null;
        }
    }
}
