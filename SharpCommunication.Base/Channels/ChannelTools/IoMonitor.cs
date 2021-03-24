using SharpCommunication.Codec.Packets;
using System;
using System.ComponentModel;

namespace SharpCommunication.Channels.ChannelTools
{
    public class IoMonitor<TPacket> : INotifyPropertyChanged where TPacket : IPacket
    {

        public IChannel<TPacket> MonitoredChannel { get; }

        public IoMonitor(IChannel<TPacket> monitoredChannel)
        {
            MonitoredChannel = monitoredChannel;
            LastPacketTime = DateTime.Now;
            monitoredChannel.DataReceived += (sender, arg) => {
                if (FirstPacketTime == DateTime.MinValue)
                    FirstPacketTime = DateTime.Now;

                DataReceivedCount++;
                LastPacketTime = DateTime.Now;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataReceivedCount)));
            };
        }
        public DateTime FirstPacketTime { get; protected set; }
        public DateTime LastPacketTime { get; protected set; }
        public int DataReceivedCount { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
