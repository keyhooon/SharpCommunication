using SharpCommunication.Base.Codec.Commands;
using System;
using System.IO;

namespace SharpCommunication.Base.Codec
{
    public class DevicePacket : IPacket
    {
        const int UniqueIdLength = 12;
        public static readonly byte[] Header = { 0x55, 0x55 };

        public byte[] UniqueId { get; set; }

        public Commands.Command Command { get; set; }


        public sealed class Encoding : IEncoding<DevicePacket>
        {
            private static IEncoding<DevicePacket> _instance;

            public static IEncoding<DevicePacket> Instance
            {
                get
                {
                    IEncoding<DevicePacket> encoding = _instance ??= new Encoding();
                    return encoding;
                }
            }

            public void Encode(DevicePacket packet, BinaryWriter writer)
            {
                if (packet == null)
                    throw new ArgumentNullException(nameof(packet));
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));
                if (packet.Command == null)
                    throw new ArgumentException("The packet does not contain a valid command.", nameof(packet));
                writer.Write(Header, 0, Header.Length);
                writer.Write(packet.UniqueId, 0, UniqueIdLength);
                CommandEncodingFactory.Instance.Create(packet.Command.Id).Encode(packet.Command, writer);
            }

            public DevicePacket Decode(BinaryReader reader)
            {
                if (reader == null)
                    throw new ArgumentNullException(nameof(reader));
                var found = 0;
                while (found < Header.Length)
                {
                    var header = reader.ReadByte();
                    if (header == Header[found])
                        found++;
                    else if (header == Header[0])
                        found = 1;
                    else
                        found = 0;
                }


                return new DevicePacket
                {
                    UniqueId = reader.ReadBytes(UniqueIdLength),
                    Command = CommandEncodingFactory.Instance.Create(reader.ReadByte()).Decode(reader)
                };
            }
        }
    }
}
