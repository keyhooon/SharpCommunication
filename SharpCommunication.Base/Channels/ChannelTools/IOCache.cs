using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SharpCommunication.Channels.Decorator;

namespace SharpCommunication.Channels.ChannelTools
{
    public class IoCache<TPacket> where TPacket : IPacket 
    {

        
        public ObservableCollection<PacketCacheInfo<TPacket>> PacketCacheInfoCollection { get; internal set; }
        public IChannel<TPacket> Channel { get; }
        public IoCache(IChannel<TPacket> cachedChannel, IEnumerable<ICachePolicy<TPacket>> cacheManagers)
        {
            PacketCacheInfoCollection = new ObservableCollection<PacketCacheInfo<TPacket>>();
            Channel = cachedChannel;
            var packetIndex = 0;
            foreach (var cacheManager in cacheManagers)
            {
                cacheManager.Bind(this);
            }


            Channel.DataReceived += (sender, arg) => {
                PacketCacheInfoCollection.Add(new PacketCacheInfo<TPacket> { Packet = arg.Data, PacketDateTimeReceived = DateTime.Now, PacketIndex = packetIndex++ });
            };
        }
    }




}
