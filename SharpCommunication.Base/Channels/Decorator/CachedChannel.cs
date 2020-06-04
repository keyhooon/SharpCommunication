using SharpCommunication.Channels.ChannelTools;
using SharpCommunication.Codec.Packets;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace SharpCommunication.Channels.Decorator
{
    public class CachedChannel<TPacket> : ChannelDecorator<TPacket> where TPacket : IPacket
    {
        public ReadOnlyObservableCollection<PacketCacheInfo<TPacket>> Packet { get; }

        public CachedChannel(Channel<TPacket> innerChannel) : base(innerChannel)
        {
            var ioCache = new IOCache<TPacket>(innerChannel, new ICachingManager<TPacket>[] { new LimitCountPacketCachingManager<TPacket> (), new ExpireTimePacketCachingManager<TPacket>()});
            Packet = new ReadOnlyObservableCollection<PacketCacheInfo<TPacket>>( ioCache.PacketCacheInfoCollection );
        }


    }
    public static class CachedChannelExtension
    {
        public static CachedChannel<TPacket> ToCachedChannel<TPacket>(this IChannel<TPacket> channel) where TPacket : IPacket
        {
            while (channel is ChannelDecorator<TPacket> item)
            {
                if (item is CachedChannel<TPacket>)
                    return (CachedChannel<TPacket>)item;
                channel = item.innerChannel;
            }
            return null;
        }
    }
}
