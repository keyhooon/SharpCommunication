using SharpCommunication.Base.Codec.Packets;
using System;

namespace SharpCommunication.Base.Channels.ChannelTools
{
    public class IoMonitor<T> where T : IPacket, new()
    {

        public Channel<T> MonitoredChannel { get; }

        public IoMonitor(Channel<T> monitoredChannel)
        {
            MonitoredChannel = monitoredChannel;
            MonitorBeginTime = DateTime.Now;
            monitoredChannel.DataReceived += (sender, arg) => { DataReceivedCount++; };
        }
        public DateTime MonitorBeginTime { get; set; }
        public int DataReceivedCount { get; set; }
    }
}
