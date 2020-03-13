using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Base.Codec
{
    public interface ICodec<T> where T : IPacket
    {

        Type DataType { get; }

        PacketEncoding Encoding { get; }

        void Encode(T data, BinaryWriter stream);

        byte[] Encode(T data);

        T Decode(BinaryReader stream);

        T Decode(byte[] bytes);
    }
}
