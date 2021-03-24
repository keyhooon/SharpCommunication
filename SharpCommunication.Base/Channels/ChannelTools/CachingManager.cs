using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCommunication.Channels.ChannelTools
{
    public interface ICachingManager<TPacket> where TPacket : IPacket
    {
        void Bind(IoCache<TPacket> ioCache);
    }

    public class LimitCountPacketCachingManager<TPacket> : ICachingManager<TPacket> where TPacket : IPacket
    {
        public int CacheCount { get; set; } = 10;
        public LimitCountPacketCachingManager()
        {

        }
        public void Bind(IoCache<TPacket> ioCache) 
        {

            ioCache.PacketCacheInfoCollection.CollectionChanged += (sender, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    if (ioCache.PacketCacheInfoCollection.Count > CacheCount)
                       Task.Run(()=> ioCache.PacketCacheInfoCollection.RemoveAt(0));
            };
        }
        public class PacketCountCacheInfo : PacketCacheInfo<TPacket> 
        {

        }
    }
    public class ExpireTimePacketCachingManager<TPacket> : ICachingManager<TPacket> where TPacket : IPacket
    {
        private Dictionary<PacketCacheInfo<TPacket>, Timer> _timer;
        public TimeSpan ExpireTime { get; set; } = TimeSpan.FromSeconds(10);
        public ExpireTimePacketCachingManager()
        {
            _timer = new Dictionary<PacketCacheInfo<TPacket>, Timer>();
        }
        public void Bind(IoCache<TPacket> ioCache) 
        {
            ioCache.PacketCacheInfoCollection.CollectionChanged += (sender, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    foreach (var item in e.NewItems)
                    {
                        _timer.Add((PacketCacheInfo<TPacket>)item,new Timer((state) => ioCache.PacketCacheInfoCollection.Remove((PacketCacheInfo<TPacket>)state), (PacketCacheInfo<TPacket>)item, ExpireTime, Timeout.InfiniteTimeSpan)) ;
                    }
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                    foreach(var item in e.OldItems)
                    {
                        _timer.Remove((PacketCacheInfo<TPacket>)item);
                    }

            };

        }


    }
}