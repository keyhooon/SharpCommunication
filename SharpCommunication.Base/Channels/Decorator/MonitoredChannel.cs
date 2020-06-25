using System;
using SharpCommunication.Channels.ChannelTools;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Channels.Decorator
{
    public class MonitoredChannel<TPacket> : ChannelDecorator<TPacket> where TPacket : IPacket
    {
        private IoMonitor< TPacket> ioMonitor { get; set; }
        public MonitoredChannel(Channel< TPacket> innerChannel) :base(innerChannel)
        {
            ioMonitor = new IoMonitor<TPacket>(innerChannel);
        }

        public int GetDataReceivedCount => ioMonitor.DataReceivedCount;
        public DateTime FirstPacketTime => ioMonitor.FirstPacketTime;
        public DateTime LastPacketTime => ioMonitor.LastPacketTime;
    }
    public static class MonitoredChannelExtension
    {
        public static MonitoredChannel<TPacket> ToMonitoredChannel<TPacket>(this IChannel<TPacket> channel) where TPacket : IPacket
        {
            while (channel is ChannelDecorator<TPacket> item)
            {
                if (item is MonitoredChannel<TPacket>)
                    return (MonitoredChannel<TPacket>)item;
                channel = item.innerChannel;
            }
            return null;
        }
    }
}
