using Microsoft.Extensions.Logging;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Transport.Stream
{
    public class StreamDataTransport<TPacket> : DataTransport<TPacket> where TPacket : IPacket
    {
        private ProducerConsumerStream InStream;
        private ProducerConsumerStream OutStream;
        public StreamDataTransport(IChannelFactory<TPacket> channelFactory, DataTransportOption option) : base(channelFactory, option)
        {

        }
        private StreamDataTransport(ProducerConsumerStream inStream, ProducerConsumerStream outStream, IChannelFactory<TPacket> channelFactory, DataTransportOption option) : base(channelFactory, option)
        {
            InStream = inStream;
            OutStream = outStream;
            _channels.Add(ChannelFactory.Create(InStream, OutStream));

        }


        public StreamDataTransport<TPacket> GetStreamDataTransportGateway()
        {
            if (IsOpen)
                return new StreamDataTransport<TPacket>(OutStream, InStream, ChannelFactory, Option);
            throw new System.Exception();
        }

        protected override bool IsOpenCore => InStream != null && OutStream != null ;

        protected override void CloseCore()
        {
            InStream.Close();
            OutStream.Close();
        }

        protected override void OpenCore()
        {
            InStream = new ProducerConsumerStream();
            OutStream = new ProducerConsumerStream();

            _channels.Add(ChannelFactory.Create(InStream, OutStream));
        }
    }
}
