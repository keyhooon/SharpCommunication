using SharpCommunication.Base.Codec.Encoding;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SharpCommunication.Base.Codec.Encoding
{

    public class DescendantPacketEncoding<T> : PacketEncoding where T : IDescendantPacket, new()
    {
        public readonly IReadOnlyDictionary<byte, AncestorPacketEncoding> EncodingList;
        private readonly IDictionary<byte, AncestorPacketEncoding> _encodingsList;
        public DescendantPacketEncoding(PacketEncoding encoding, IEnumerable<PacketEncoding> encodingsList) : this(encoding)
        {
            foreach (var encodingItem in encodingsList)
            {
                Register(encodingItem);
            }
        }
        public DescendantPacketEncoding(PacketEncoding encoding) : base(encoding)
        {
            _encodingsList = new Dictionary<byte, AncestorPacketEncoding>();
            EncodingList = new ReadOnlyDictionary<byte, AncestorPacketEncoding>(_encodingsList);
        }
        public void Register(PacketEncoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            
            var enc = encoding.FindDecoratedEncoding<AncestorPacketEncoding>();
            if (enc == null)
                throw new NotSupportedException();

            _encodingsList.Add(enc.Id, enc);
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            var descendantPacket = (IDescendantPacket)packet;
            EncodingList.TryGetValue(descendantPacket.DescendantPacket.Id, out var packetEncoding);
            writer.Write(descendantPacket.DescendantPacket.Id);
            packetEncoding.EncodeCore(descendantPacket.DescendantPacket, writer);
        }

        public override IPacket DecodeCore( BinaryReader reader)
        {
            var packetEncodingId = reader.ReadByte();
            EncodingList.TryGetValue(packetEncodingId, out var packetEncoding);
            T obj = new T
            {
                DescendantPacket = (IAncestorPacket)packetEncoding?.DecodeCore(reader)
            };
            return obj;
        }

    }

}
