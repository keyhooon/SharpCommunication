using System;
using System.Collections.Generic;
using SharpCommunication.Base.Codec.Packets.Commands;
using SharpCommunication.Base.Codec.Packets.Records;
using SharpCommunication.Base.Codec.Packets.Records.CodeBook;
using SharpCommunication.Base.Codec.Packets.Records.Data;

namespace SharpCommunication.Base.Codec.Packets
{
    public sealed class PacketEncodingFactory
    {
        private static PacketEncodingFactory _instance;

        public static PacketEncodingFactory Instance => _instance ??= new PacketEncodingFactory();

        private readonly IDictionary<int, IPacketEncoding> _encodings = new Dictionary<int, IPacketEncoding>();

        private PacketEncodingFactory()
        {
            Register(new DataRecord.Encoding());
            Register(new CommandPacket.Encoding());
        }



        public IPacketEncoding Create(int packetId)
        {
            if (!_encodings.TryGetValue(packetId & 0xf, out var packetEncoding) || packetEncoding == null)
                throw new NotSupportedException($"The packet encoding for packet 0x{packetId:X} is not supported.");
            return packetEncoding;
        }

        private void Register(IPacketEncoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            _encodings.Add(encoding.TypeId, encoding);
        }
    }
}
