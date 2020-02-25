using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IDescendantPacket : IPacket
    {
        public IAncestorPacket DescendantPacket { get; set; }

    }

    public class DescendantPacketEncoding<T> : PacketEncoding<T> where T : IDescendantPacket, new()
    {
        public readonly IReadOnlyDictionary<int, AncestorPacketEncoding<IAncestorPacket>> EncodingList;
        private readonly IDictionary<int, AncestorPacketEncoding<IAncestorPacket>> _encodingsList;
        public DescendantPacketEncoding(PacketEncoding<IPacket> encoding, IEnumerable<AncestorPacketEncoding<IAncestorPacket>> encodingsList) : this(encoding)
        {
            foreach (var encodingItem in encodingsList)
            {
                Register(encodingItem);
            }
        }
        public DescendantPacketEncoding(PacketEncoding<IPacket> encoding) : base(encoding)
        {
            _encodingsList = new Dictionary<int, AncestorPacketEncoding<IAncestorPacket>>();
            EncodingList = new ReadOnlyDictionary<int, AncestorPacketEncoding<IAncestorPacket>>(_encodingsList);
        }
        public void Register(AncestorPacketEncoding<IAncestorPacket> encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            _encodingsList.Add(encoding.Id, encoding);
        }

        public override void EncodeCore(T packet, BinaryWriter writer)
        {
            EncodingList.TryGetValue(packet.DescendantPacket.Id, out var packetEncoding);
            writer.Write(packet.DescendantPacket.Id);
            packetEncoding.EncodeCore(packet.DescendantPacket, writer);
            Encoding.EncodeCore(packet, writer);
        }

        public override T DecodeCore( BinaryReader reader)
        {
            EncodingList.TryGetValue(reader.ReadByte(), out var packetEncoding);
            var descendPacket = packetEncoding.DecodeCore(reader);
            var obj = new T();
            obj.DescendantPacket = descendPacket;
            return obj;
        }

    }
    public static class HasDescendantPacketHelper
    {
        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList) where T : IDescendantPacket, new()
        {
            mapItemBuilder.SetupActions.Add(item => (PacketEncoding<IPacket>)new DescendantPacketEncoding<T>(item, encodingBuiledersList.Select(o => (AncestorPacketEncoding<IAncestorPacket>)o.Build()).ToList()));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder, IEnumerable<AncestorPacketEncoding<IAncestorPacket>> encodingsList) where T : IDescendantPacket, new()
        {
            mapItemBuilder.SetupActions.Add(item => new DescendantPacketEncoding<T>(item, encodingsList));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder) where T : IDescendantPacket, new()
        {
            mapItemBuilder.SetupActions.Add(item => (PacketEncoding<IPacket>)new DescendantPacketEncoding<T>(item));
            return mapItemBuilder;
        }
    }
}
