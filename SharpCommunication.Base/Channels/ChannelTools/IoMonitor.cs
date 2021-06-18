using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec.Packets;
using System;

namespace SharpCommunication.Channels.ChannelTools
{
    public class IoMonitor<TPacket> where TPacket : IPacket
    {
        public MonitoredChannel<TPacket> MonitoredChannel { get; }

        public IoMonitor(MonitoredChannel<TPacket> monitoredChannel)
        {
            MonitoredChannel = monitoredChannel;
            LastPacketTime = DateTime.Now;
            monitoredChannel.DataReceived += (sender, arg) => {
                if (FirstPacketTime == DateTime.MinValue)
                {
                    FirstPacketTime = DateTime.Now;
                }
                DataReceivedCount++;
                LastPacketTime = DateTime.Now;
            };
        }
        public DateTime FirstPacketTime {
            get;
            set;
        }
        public DateTime LastPacketTime {
            get;
            set;
        }
        public int DataReceivedCount {
            get;
            set;
        }
    }
}
