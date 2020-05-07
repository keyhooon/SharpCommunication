using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.ComponentModel;

namespace SharpCommunication.Base.Channels.ChannelTools
{
    public class IoMonitor<TPacket>: INotifyPropertyChanged where TPacket : IPacket
    {

        public IChannel<TPacket> MonitoredChannel { get; }

        public IoMonitor(IChannel<TPacket> monitoredChannel)
        {
            MonitoredChannel = monitoredChannel;
            MonitorBeginTime = DateTime.Now;
            monitoredChannel.DataReceived += (sender, arg) => { DataReceivedCount++; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DataReceivedCount))); };
        }
        public DateTime MonitorBeginTime { get; protected set; }
        public int DataReceivedCount { get; protected set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
