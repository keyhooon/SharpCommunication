using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Codec
{
    public class Data : IPacket, IDescendantPacket, IAncestorPacket
    {

        public IAncestorPacket DescendantPacket { get; set; }
        public override string ToString()
        {
            return $"Data : {DescendantPacket?.ToString()} ";
        }

        public class Encoding : DescendantPacketEncoding<Command>
        {

            public static byte Id => 0;
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
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o=>new AncestorPacketEncoding<Command>(o,Id)).AddDecorate(o => new Encoding(o));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<PacketEncodingBuilder> encodingBuileders) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding<Command>(o, Id)).AddDecorate(o => new Encoding(o, encodingBuileders.Select(o => o.Build()).ToList()));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<EncodingDecorator> encodings) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new AncestorPacketEncoding<Command>(o, Id)).AddDecorate(o => new Encoding(o, encodings));
        }
    }
}
