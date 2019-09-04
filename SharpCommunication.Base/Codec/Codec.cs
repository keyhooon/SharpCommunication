using System;
using System.IO;

namespace SharpCommunication.Base.Codec
{
    public class Codec<TData, TEncoding> : ICodec where TData : class where TEncoding : IEncoding<TData>, new()
    {
        public Codec()
        {

        }

        public Type DataType => typeof(TData);

        public virtual void Encode(object data, BinaryWriter binaryWriter)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (binaryWriter == null)
                throw new ArgumentNullException(nameof(binaryWriter));
            if (!(data is TData tData))
                throw new CodecException("Expected data type " + DataType);
            new TEncoding().Encode(tData, binaryWriter);

        }

        public virtual byte[] Encode(object data)
        {
            using (var memoryStream = new MemoryStream())
            {
                Encode(data, new BinaryWriter(memoryStream));
                return memoryStream.ToArray();
            }
        }

        public virtual object Decode(BinaryReader binaryReader)
        {
            if (binaryReader == null)
                throw new ArgumentNullException(nameof(binaryReader));
            return new TEncoding().Decode(binaryReader);

        }

        public virtual object Decode(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0)
                throw new InvalidOperationException("bytes.Length = 0");
            using (var memoryStream = new MemoryStream(bytes))
                return Decode(new BinaryReader(memoryStream));
        }
    }
}
