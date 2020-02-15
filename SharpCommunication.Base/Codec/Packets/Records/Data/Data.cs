using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets.Records.Data
{
    public abstract class Data
    {
        public abstract int Id { get; }
               
        public abstract override string ToString();

        public abstract class Encoding<T> : IDataEncoding where T : Data
        {
            public abstract int Id { get; }

            public void Encode(Data data, BinaryWriter writer)
            {
                if (data == null)
                    throw new ArgumentNullException(nameof(data));
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));
                if (!(data is T da))
                    throw new ArgumentException("The command type is not supported.");
                EncodeCore(da, writer);
            }

            public Data Decode(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));
                var obj = DecodeCore(reader);
                if (obj == null)
                    throw new NotSupportedException("The encoding was unable to decode the command.");
                return obj;
            }

            protected abstract void EncodeCore(T command, BinaryWriter writer);

            protected abstract T DecodeCore(BinaryReader reader);
        }
    }
}
