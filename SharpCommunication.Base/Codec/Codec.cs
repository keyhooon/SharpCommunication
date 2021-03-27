using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec
{
    public class Codec<TData> : ICodec<TData> where TData : IPacket, new()
    {
        public Type DataType => typeof(TData);
        public EncodingDecorator Encoding { get; }

        public Codec(EncodingDecorator encoding)
        {
            Encoding = encoding;
        }
        public TData Decode(BinaryReader stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            return (TData) Encoding.Decode(stream);

        }

        public TData Decode(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0)
                throw new InvalidOperationException("bytes.Length = 0");
            using var memoryStream = new MemoryStream(bytes);
            return (TData) Encoding.Decode(new BinaryReader(memoryStream));
        }

        public void Encode(TData data, BinaryWriter stream)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (!(data is TData tData))
                throw new CodecException("Expected data type " + DataType);
            Encoding.Encode(tData, stream);

        }

        public byte[] Encode(TData data)
        {
            using var memoryStream = new MemoryStream();
            Encode(data, new BinaryWriter(memoryStream));
            return memoryStream.ToArray();
        }
    }
}
