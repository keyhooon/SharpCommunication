using System.Collections.Generic;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class Device : IDescendantPacket
    {


        public IAncestorPacket Content { get; set; }
        public override string ToString()
        {
            return $"DevicePacket {{ {Content?.ToString()} }}";
        }
        public class Encoding : DescendantPacketEncoding<Device>
        {
            private static readonly byte[] _header = { 0xAA, 0xAA };

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public Encoding(EncodingDecorator encoding, IEnumerable<EncodingDecorator> encodingsList) : this(encoding)
            {
                foreach (var encodingItem in encodingsList)
                {
                    Register(encodingItem);
                }
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new HeaderPacketEncoding(o, _header)).AddDecorate(o => new Encoding(o));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<PacketEncodingBuilder> encodingBuilders) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o =>new HeaderPacketEncoding(o, _header)).AddDecorate(o => new Encoding(o, encodingBuilders.Select(o => o.Build()).ToList()));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<EncodingDecorator> encodings) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new HeaderPacketEncoding(o, _header)).AddDecorate(o => new Encoding(o, encodings));
        }

    }
}
