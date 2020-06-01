using System;
using SharpCommunication.Channels.ChannelTools;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Channels.Decorator
{
    public class MonitoredChannel<TPacket> : ChannelDecorator<TPacket> where TPacket : IPacket
    {
        private IoMonitor< TPacket> ioMonitor { get; set; }
        public MonitoredChannel(Channel< TPacket> innerChannel) : base(innerChannel)
        {
            ioMonitor = new IoMonitor<TPacket>(innerChannel);
        }

        public int GetDataReceivedCount => ioMonitor.DataReceivedCount;
        public DateTime MonitorBeginTime => ioMonitor.MonitorBeginTime;

    }
}
