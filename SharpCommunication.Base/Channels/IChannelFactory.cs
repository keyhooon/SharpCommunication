using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System.IO;

namespace SharpCommunication.Base.Channels
{
    interface IChannelFactory<T> where T: IPacket, new()
    {
        ICodec<T> Codec { get; }

        Channel<T> Create(Stream stream);
    }
}
