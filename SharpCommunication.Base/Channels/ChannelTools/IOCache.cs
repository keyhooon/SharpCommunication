using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SharpCommunication.Channels.ChannelTools
{
    public class IOCache<TPacket> where TPacket : IPacket 
    {

        public ObservableCollection<PacketCacheInfo<TPacket>> PacketCacheInfoCollection { get; internal set; }
        private int packetIndex;
        public IOCache(IChannel<TPacket> cachededChannel, IEnumerable<ICachingManager<TPacket>> cacheManagers)
        {
            PacketCacheInfoCollection = new ObservableCollection<PacketCacheInfo<TPacket>>();
            packetIndex = 0;
            foreach (var cacheManager in cacheManagers)
            {
                cacheManager.Bind(this);
            }
            cachededChannel.DataReceived += (sender, arg) => {
                PacketCacheInfoCollection.Add(new PacketCacheInfo<TPacket> { packet = arg.Data, packetDateTimeReceived = DateTime.Now, packetIndex = packetIndex++ });
            };
        }
    }
    public class PacketCacheInfo<TPacket> where TPacket: IPacket
    {
        public TPacket packet { get; set; }
        public DateTime packetDateTimeReceived { get; set; }
        public int packetIndex { get; set; }
    }



}
