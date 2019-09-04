using System;
using System.IO;
using SharpCommunication.Base.Channels.ChannelTools;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Channels.Decorator
{
    public class MonitoredChannel : ChannelDecorator
    {
        private IoMonitor ioMonitor { get; set; }
        public MonitoredChannel(ICodec codec, Stream stream, IDisposable streamingObject) : this(new Channel(codec, stream, streamingObject))
        {
        }

        public MonitoredChannel(Channel innerChannel) : base(innerChannel)
        {
            ioMonitor = new IoMonitor(innerChannel);
        }

        public int GetDataReceivedCount => ioMonitor.DataReceivedCount;
        public DateTime MonitorBeginTime => ioMonitor.MonitorBeginTime;

    }
}
