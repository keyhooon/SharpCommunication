using SharpCommunication.Codec.Packets;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SharpCommunication.Channels.ChannelTools
{
    public class IOCache<TPacket> : INotifyPropertyChanged where TPacket : IPacket
    {

        public IChannel<TPacket> CachededChannel { get; }
        public CachingManager<TPacket> CachingManager { get; }  
        public ObservableCollection<PacketCacheInfo<TPacket>> PacketCacheInfoCollection { get; internal set; }
        public IOCache(IChannel<TPacket> cachededChannel)
        {
            CachededChannel = cachededChannel;
            MonitorBeginTime = DateTime.Now;
            CachededChannel.DataReceived += (sender, arg) => { ChannelDataReceived?.Invoke(sender, arg); };
        }
        public DateTime MonitorBeginTime { get; protected set; }
        public int DataReceivedCount { get; protected set; }
        public event EventHandler<DataReceivedEventArg<TPacket>> ChannelDataReceived;
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class PacketCacheInfo<TPacket> where TPacket: IPacket
    {
        public TPacket packet { get; set; }
    }

    public class PacketTimeCacheInfo<TPacket> : PacketCacheInfo<TPacket> where TPacket : IPacket
    {
        public DateTime packetDateTimeReceived { get; set; }
    }


}
