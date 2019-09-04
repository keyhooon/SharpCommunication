using System;
using System.IO;

namespace SharpCommunication.Base.Codec
{
    public interface ICodec
    {

        Type DataType { get; }

        void Encode(object data, BinaryWriter stream);

        byte[] Encode(object data);

        object Decode(BinaryReader stream);

        object Decode(byte[] bytes);
    }
}
