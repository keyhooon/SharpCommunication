using SharpCommunication.Base.Codec.Encoding;
using SharpCommunication.Base.Codec.Packets;
using System.Collections.Generic;
using System.Linq;

namespace SharpCommunication.Base.Codec.Encoding
{
    public static class PacketEncodingHelper
    {

        public static T FindDecoratedEncoding<T>(this PacketEncoding packetEncoding) where T : PacketEncoding 
        {
            while (packetEncoding is PacketEncoding item)
            {
                if (item is T packetEncodingDesire)
                    return packetEncodingDesire;
                packetEncoding = packetEncoding.Encoding;
            }
            return null;
        }
        public static PacketEncodingBuilder WithAncestor(this PacketEncodingBuilder packetEncodingBuilder, byte id)
        {
            packetEncodingBuilder.AddDecorate(item => new AncestorPacketEncoding(item, id));
            return packetEncodingBuilder;
        }
        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList) where T : IDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantPacketEncoding<T>(item, encodingBuiledersList.Select(o => o.Build()).ToList()));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder, IEnumerable<PacketEncoding> encodingsList) where T : IDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantPacketEncoding<T>(item, encodingsList));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder) where T : IDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantPacketEncoding<T>(item));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithFunction<T>(this PacketEncodingBuilder mapItemBuilder, byte inputByteCount, byte id) where T : IFunctionPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new FunctionPacketEncoding<T>(item, inputByteCount, id));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithHeader(this PacketEncodingBuilder mapItemBuilder, byte[] header)
        {
            mapItemBuilder.AddDecorate(item => new HeaderPacketEncoding(item, header));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithUnixTimeEpoch(this PacketEncodingBuilder mapItemBuilder)
        {
            mapItemBuilder.AddDecorate(item => new HasUnixTimeEpochPacketEncoding(item));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithProperty(this PacketEncodingBuilder mapItemBuilder, byte propertySize)
        {
            mapItemBuilder.AddDecorate(item => new HasPropertyPacketEncoding(item, propertySize));
            return mapItemBuilder;
        }

    }
    public static class HasDescendantPacketHelper
    {

    }
}
