using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System.IO;

namespace SharpCommunication.Base.Channels
{
    public interface IChannelFactory<TPacket> where TPacket : IPacket
    {
        ICodec<TPacket> Codec { get; }

        IChannel<TPacket> Create(Stream stream);
    }
}
