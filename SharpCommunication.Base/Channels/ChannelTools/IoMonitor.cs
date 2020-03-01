using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;

namespace SharpCommunication.Base.Channels.ChannelTools
{
    public class IoMonitor<TPacket> where TPacket : IPacket, new()
    {

        public IChannel<TPacket> MonitoredChannel { get; }

        public IoMonitor(IChannel<TPacket> monitoredChannel)
        {
            MonitoredChannel = monitoredChannel;
            MonitorBeginTime = DateTime.Now;
            monitoredChannel.DataReceived += (sender, arg) => { DataReceivedCount++; };
        }
        public DateTime MonitorBeginTime { get; set; }
        public int DataReceivedCount { get; set; }
    }
}
