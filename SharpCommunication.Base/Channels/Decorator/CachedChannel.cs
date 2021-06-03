﻿using System;
using SharpCommunication.Channels.ChannelTools;
using SharpCommunication.Codec.Packets;
using System.Collections.ObjectModel;

namespace SharpCommunication.Channels.Decorator
{
    public class CachedChannel<TPacket> : ChannelDecorator<TPacket> where TPacket : IPacket
    {
        public ObservableCollection<PacketCacheInfo<TPacket>> Packet;
        private IoCache<TPacket> ioCache;
        public CachedChannel(Channel<TPacket> innerChannel) : base(innerChannel)
        {
            Packet = new ObservableCollection<PacketCacheInfo<TPacket>>();
            ioCache = new IoCache<TPacket>(this, new ICachePolicy<TPacket>[] { new LimitCountPacketCachePolicy<TPacket>(), new ExpireTimePacketCachePolicy<TPacket>()});
        }


    }
    public class PacketCacheInfo<TPacket> where TPacket : IPacket
    {
        public TPacket Packet { get; set; }
        public DateTime PacketDateTimeReceived { get; set; }
        public int PacketIndex { get; set; }
    }
    public static class CachedChannelExtension
    {
        public static CachedChannel<TPacket> ToCachedChannel<TPacket>(this IChannel<TPacket> channel) where TPacket : IPacket
        {
            while (channel is ChannelDecorator<TPacket> item)
            {
                if (item is CachedChannel<TPacket> cachedChannel)
                    return cachedChannel;
                channel = item.InnerChannel;
            }
            return null;
        }
    }
}
