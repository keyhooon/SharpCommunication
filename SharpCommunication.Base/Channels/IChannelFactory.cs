using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System.IO;

namespace SharpCommunication.Channels
{
    public interface IChannelFactory<TPacket> where TPacket : IPacket
    {
        ICodec<TPacket> Codec { get; }

        IChannel<TPacket> Create(Stream stream);
        IChannel<TPacket> Create(Stream inputStream, Stream outputStream);

    }
}
