using System;
using SharpCommunication.Codec.Packets;
using System.Collections.Generic;
using System.Linq;

namespace SharpCommunication.Codec.Encoding
{
    public static class PacketEncodingExtension
    {

        public static T FindDecoratedEncoding<T>(this EncodingDecorator packetEncoding) where T : class, IEncoding<IPacket>
        {
            while (packetEncoding is { } item)
            {
                if (item is T encoding)
                    return encoding;
                packetEncoding = packetEncoding.Encoding;
            }
            return null;
        }

        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList) where T : IDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantPacketEncoding<T>(item, encodingBuiledersList.Select(o => o.Build()).ToList()));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder, IEnumerable<EncodingDecorator> encodingsList) where T : IDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantPacketEncoding<T>(item, encodingsList));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithDescendant<T>(this PacketEncodingBuilder mapItemBuilder) where T : IDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantPacketEncoding<T>(item));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithHeader(this PacketEncodingBuilder mapItemBuilder, byte[] header)
        {
            mapItemBuilder.AddDecorate(item => new HeaderPacketEncoding(item, header));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithUnixTimeEpoch(this PacketEncodingBuilder mapItemBuilder) 
        {
            mapItemBuilder.AddDecorate(item => new UnixTimeEpochPacketEncoding(item));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithProperty(this PacketEncodingBuilder mapItemBuilder, byte propertySize) 
        {
            mapItemBuilder.AddDecorate(item => new PropertyPacketEncoding(item, propertySize));
            return mapItemBuilder;
        }
        public static EncodingDecorator GetChildEncoding<TP, TC>(this EncodingDecorator encodingDecorator) where TP : class, IDescendantPacket, new() where TC : IAncestorPacket
        {
            var descendantPacketEncoding = encodingDecorator.FindDecoratedEncoding<DescendantPacketEncoding<TP>>();
            if (descendantPacketEncoding == null)
                throw new ArgumentException();
            return descendantPacketEncoding.EncodingDictionary[descendantPacketEncoding.IdDictionary[typeof(TC)]];

        }
    }
    public static class HasDescendantPacketHelper
    {

    }
}
