using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpCommunication.Channels.Decorator;

namespace SharpCommunication.Channels.ChannelTools
{
    public interface ICachePolicy<TPacket> where TPacket : IPacket
    {
        void Bind(IoCache<TPacket> ioCache);
    }

    public class LimitCountPacketCachePolicy<TPacket> : ICachePolicy<TPacket> where TPacket : IPacket
    {
        public int CacheCount { get; set; } = 10;
        public void Bind(IoCache<TPacket> ioCache) 
        {
            ioCache.Channel.DataReceived += (sender, arg) =>
            {
                if (ioCache.PacketCacheInfoCollection.Count > CacheCount)
                    ioCache.PacketCacheInfoCollection.RemoveAt(0);
            };
        }
    }
    public class ExpireTimePacketCachePolicy<TPacket> : ICachePolicy<TPacket> where TPacket : IPacket
    {
        private readonly Dictionary<PacketCacheInfo<TPacket>, Timer> _timer;
        public TimeSpan ExpireTime { get; set; } = TimeSpan.FromSeconds(10);
        public ExpireTimePacketCachePolicy()
        {
            _timer = new Dictionary<PacketCacheInfo<TPacket>, Timer>();
        }
        public void Bind(IoCache<TPacket> ioCache) 
        {
            ioCache.PacketCacheInfoCollection.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    {
                        foreach (var item in e.NewItems)
                        {
                            _timer.Add((PacketCacheInfo<TPacket>)item,new Timer((state) => ioCache.PacketCacheInfoCollection.Remove((PacketCacheInfo<TPacket>)state), (PacketCacheInfo<TPacket>)item, ExpireTime, Timeout.InfiniteTimeSpan)) ;
                        }

                        break;
                    }
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    {
                        foreach(var item in e.OldItems)
                        {
                            _timer.Remove((PacketCacheInfo<TPacket>)item);
                        }

                        break;
                    }
                }
            };

        }


    }
}