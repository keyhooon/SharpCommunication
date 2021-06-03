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


        public ObservableCollection<PacketCacheInfo<TPacket>> PacketCacheInfoCollection => Channel.Packet;
        public CachedChannel<TPacket> Channel { get; }
        private int packetIndex;
        public IoCache(CachedChannel<TPacket> cachedChannel, IEnumerable<ICachePolicy<TPacket>> cachePolicies)
        {
            packetIndex = 0;

            Channel = cachedChannel;
            foreach (var cachePolicy in cachePolicies)
            {
                cachePolicy.Bind(this);
            }

            Channel.DataReceived += OnDataReceived;
        }
        private void OnDataReceivedFirstTime(object sender, DataReceivedEventArg<TPacket> arg)
        {
        }

        private void OnDataReceived(object sender, DataReceivedEventArg<TPacket> arg)
        {
            lock (PacketCacheInfoCollection)
            {
                PacketCacheInfoCollection.Add(new PacketCacheInfo<TPacket> { Packet = arg.Data, PacketDateTimeReceived = DateTime.Now, PacketIndex = packetIndex++ });
            }
        }
    }




}
