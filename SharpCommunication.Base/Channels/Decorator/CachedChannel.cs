using SharpCommunication.Channels.ChannelTools;
using SharpCommunication.Codec.Packets;
using System.Collections.ObjectModel;

namespace SharpCommunication.Channels.Decorator
{
    public class CachedChannel<TPacket> : ChannelDecorator<TPacket> where TPacket : IPacket
    {
        private IOCache<TPacket> ioCache { get; set; }
        public CachedChannel(Channel<TPacket> innerChannel) : base(innerChannel)
        {
            ioCache = new IOCache<TPacket>(innerChannel);
        }

        public CachingManager<TPacket> CachingManager => ioCache.CachingManager;
        public ObservableCollection<PacketCacheInfo<TPacket>> packet { get { return ioCache.PacketCacheInfoCollection; } }

    }
}
