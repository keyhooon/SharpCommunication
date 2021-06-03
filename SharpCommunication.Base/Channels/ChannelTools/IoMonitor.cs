using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec.Packets;
using System;
using System.ComponentModel;

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
            get { return MonitoredChannel.FirstPacketTime; } 
            set { MonitoredChannel.FirstPacketTime = value; } 
        }
        public DateTime LastPacketTime { 
            get { return MonitoredChannel.LastPacketTime; }
            set { MonitoredChannel.LastPacketTime = value; } 
        }
        public int DataReceivedCount { 
            get { return MonitoredChannel.DataReceivedCount; } 
            set { MonitoredChannel.DataReceivedCount = value; } 
        }

public event EventHandler ParameterChanged;
    }
}
