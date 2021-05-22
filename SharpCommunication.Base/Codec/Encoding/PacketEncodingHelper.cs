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

        public static PacketEncodingBuilder WithDescendantFlag<T>(this PacketEncodingBuilder mapItemBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList, bool containLength = false) where T : IListDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantFlagPacketEncoding<T>(item, encodingBuiledersList.Select(o => o.Build()).ToList(),containLength));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithDescendantFlag<T>(this PacketEncodingBuilder mapItemBuilder, IEnumerable<EncodingDecorator> encodingsList, bool containLength = false) where T : IListDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantFlagPacketEncoding<T>(item, encodingsList, containLength));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithDescendantFlag<T>(this PacketEncodingBuilder mapItemBuilder, bool containLength = false) where T : IListDescendantPacket, new()
        {
            mapItemBuilder.AddDecorate(item => new DescendantFlagPacketEncoding<T>(item, containLength));
            return mapItemBuilder;
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
        public static PacketEncodingBuilder WithProperty<T>(this PacketEncodingBuilder mapItemBuilder, byte propertySize, Func<IPropertyPacket<T>, byte[]> Encode, Action<byte[], IPropertyPacket<T>> Decode)
        {
            mapItemBuilder.AddDecorate(item => new PropertyPacketEncoding<T>(item, propertySize, Encode, Decode));
            return mapItemBuilder;
        }
        public static PacketEncodingBuilder WithStringProperty(this PacketEncodingBuilder mapItemBuilder, byte propertySize)
        {
            mapItemBuilder.AddDecorate(item => new PropertyPacketEncoding<string>(item, propertySize,
                (packet) => System.Text.Encoding.UTF8.GetBytes(packet.Property),
                (bytes, packet) => packet.Property = System.Text.Encoding.UTF8.GetString(bytes)));

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
