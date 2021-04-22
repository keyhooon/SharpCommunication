using Microsoft.Extensions.Options;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Transport.Stream
{
    public class StreamDataTransport<TPacket> : DataTransport<TPacket> where TPacket : IPacket
    {
        private ProducerConsumerStream _inStream;
        private ProducerConsumerStream _outStream;
        public StreamDataTransport(IChannelFactory<TPacket> channelFactory, StreamDataTransportSettings settings) : base(channelFactory, settings)
        {

        }
        private StreamDataTransport(ProducerConsumerStream inStream, ProducerConsumerStream outStream, IChannelFactory<TPacket> channelFactory, StreamDataTransportSettings settings) : base(channelFactory, settings)
        {
            _inStream = inStream;
            _outStream = outStream;
            InnerChannels.Add(ChannelFactory.Create(_inStream, _outStream));

        }


        public StreamDataTransport<TPacket> GetStreamDataTransportGateway()
        {
            if (IsOpen)
                return new StreamDataTransport<TPacket>(_outStream, _inStream, ChannelFactory, (StreamDataTransportSettings)Settings);
            throw new System.Exception();
        }

        protected override bool IsOpenCore => _inStream != null && _outStream != null ;

        protected override void CloseCore()
        {
            _inStream.Close();
            _outStream.Close();
        }

        protected override void OpenCore()
        {
            _inStream = new ProducerConsumerStream();
            _outStream = new ProducerConsumerStream();

            InnerChannels.Add(ChannelFactory.Create(_inStream, _outStream));
        }
    }
}
