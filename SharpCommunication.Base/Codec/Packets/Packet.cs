using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public abstract class Packet
    {
        public abstract int TypeId { get; }



        public abstract override string ToString();

        public abstract class Encoding<T> : IPacketEncoding where T : Packet
        {
            public abstract int TypeId { get; }

            public void Encode(Packet packet, BinaryWriter writer)
            {
                if (packet == null)
                    throw new ArgumentNullException(nameof(packet));
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));
                if (!(packet is T command1))
                    throw new ArgumentException("The command type is not supported.");
                EncodeCore(command1, writer);
            }

            public Packet Decode(BinaryReader reader)
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
