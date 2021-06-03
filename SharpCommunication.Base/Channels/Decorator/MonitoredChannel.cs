using System;
using System.ComponentModel;
using SharpCommunication.Channels.ChannelTools;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Channels.Decorator
{
    public class MonitoredChannel<TPacket> : ChannelDecorator<TPacket> where TPacket : IPacket
    {


        private IoMonitor<TPacket> IoMonitor { get; set; }
        public MonitoredChannel(Channel<TPacket> innerChannel) : base(innerChannel)
        {
            IoMonitor = new IoMonitor<TPacket>(this);
        }

        public int DataReceivedCount => IoMonitor.DataReceivedCount;
        public DateTime FirstPacketTime => IoMonitor.FirstPacketTime;
        public DateTime LastPacketTime => IoMonitor.LastPacketTime;

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
