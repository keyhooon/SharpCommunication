using System;
using System.IO;
using SharpCommunication.Base.Channels.ChannelTools;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;

namespace SharpCommunication.Base.Channels.Decorator
{
    public class MonitoredChannel<T> : ChannelDecorator<T> where T: IPacket, new()
    {
        private IoMonitor<T> ioMonitor { get; set; }
        public MonitoredChannel(ICodec<T> codec, Stream stream) : this(new Channel<T>(codec, stream))
        {
        }

        public MonitoredChannel(Channel<T> innerChannel) : base(innerChannel)
        {
            ioMonitor = new IoMonitor<T>(innerChannel);
        }

        public int GetDataReceivedCount => ioMonitor.DataReceivedCount;
        public DateTime MonitorBeginTime => ioMonitor.MonitorBeginTime;

    }
}
