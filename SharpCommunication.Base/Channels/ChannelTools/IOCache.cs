using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SharpCommunication.Channels.ChannelTools
{
    public class IoCache<TPacket> where TPacket : IPacket 
    {

        public ObservableCollection<PacketCacheInfo<TPacket>> PacketCacheInfoCollection { get; internal set; }
        private int _packetIndex;
        public IoCache(IChannel<TPacket> cachededChannel, IEnumerable<ICachingManager<TPacket>> cacheManagers)
        {
            PacketCacheInfoCollection = new ObservableCollection<PacketCacheInfo<TPacket>>();
            _packetIndex = 0;
            foreach (var cacheManager in cacheManagers)
            {
                cacheManager.Bind(this);
            }
            cachededChannel.DataReceived += (sender, arg) => {
                PacketCacheInfoCollection.Add(new PacketCacheInfo<TPacket> { Packet = arg.Data, PacketDateTimeReceived = DateTime.Now, PacketIndex = _packetIndex++ });
            };
        }
    }
    public class PacketCacheInfo<TPacket> where TPacket: IPacket
    {
        public TPacket Packet { get; set; }
        public DateTime PacketDateTimeReceived { get; set; }
        public int PacketIndex { get; set; }
    }



}
