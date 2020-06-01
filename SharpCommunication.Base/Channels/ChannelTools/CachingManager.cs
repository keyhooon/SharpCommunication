using SharpCommunication.Codec.Packets;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpCommunication.Channels.ChannelTools
{
    public class CachingManager<TPacket> where TPacket : IPacket
    {
        public IOCache<TPacket> IoCache { get; }
        public CachingManager(IOCache<TPacket> ioCache)
        {
            IoCache = ioCache;
        }

    }

    public class LimitCountPacketCachingManager<TPacket> : CachingManager<TPacket> where TPacket : IPacket
    {
        public int CacheCount { get; set; } = 10;
        public LimitCountPacketCachingManager(IOCache<TPacket> ioCache) : base(ioCache)
        {
            
            IoCache.ChannelDataReceived += (sender, e) =>
            {
                IoCache.PacketCacheInfoCollection = new ObservableCollection<PacketCacheInfo<TPacket>>();
                ioCache.PacketCacheInfoCollection.Add(new PacketCountCacheInfo { packet = e.Data });
                if (IoCache.PacketCacheInfoCollection.Count > CacheCount)
                    IoCache.PacketCacheInfoCollection.RemoveAt(0);
            };
        }
        public class PacketCountCacheInfo : PacketCacheInfo<TPacket> 
        {

        }
    }
    public class ExpireTimePacketCachingManager<TPacket> : CachingManager<TPacket> where TPacket : IPacket
    {
        public TimeSpan ExpirationTime { get; set; } = TimeSpan.FromSeconds(10);
        public ExpireTimePacketCachingManager(IOCache<TPacket> ioCache) : base(ioCache)
        {
            IoCache.PacketCacheInfoCollection = new ObservableCollection<PacketCacheInfo<TPacket>>();
            IoCache.ChannelDataReceived += (sender, e) =>
            {
                IoCache.PacketCacheInfoCollection.Add(new PacketTimeExpirationCacheInfo() { packet = e.Data, ExpirationTime = DateTime.Now + ExpirationTime });
                while (IoCache.PacketCacheInfoCollection.Any() && ((PacketTimeExpirationCacheInfo)IoCache.PacketCacheInfoCollection.First()).ExpirationTime < DateTime.Now)
                    IoCache.PacketCacheInfoCollection.RemoveAt(0);
            };
        }
        public class PacketTimeExpirationCacheInfo : PacketCacheInfo<TPacket>
        {
            public DateTime ExpirationTime;
        }
    }
}