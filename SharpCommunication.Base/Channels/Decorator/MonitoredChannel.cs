using System;
using System.IO;
using SharpCommunication.Base.Channels.ChannelTools;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace SharpCommunication.Base.Channels.Decorator
{
    public class MonitoredChannel<TPacket> : ChannelDecorator<TPacket> where TPacket : IPacket, new()
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
