using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SharpCommunication.Channels.ChannelTools
{
    public class IoMonitor<TPacket> : INotifyPropertyChanged where TPacket : IPacket
    {

        public IChannel<TPacket> MonitoredChannel { get; }

        public IoMonitor(IChannel<TPacket> monitoredChannel)
        {
            MonitoredChannel = monitoredChannel;
            monitoredChannel.DataReceived += (sender, arg) => {
                if (MonitorBeginTime == DateTime.MinValue)
                    MonitorBeginTime = DateTime.Now;
                DataReceivedCount++;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataReceivedCount)));
            };
        }
        public DateTime MonitorBeginTime { get; protected set; }
        public int DataReceivedCount { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
