using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Codec
{
    public class Command : IPacket, IDescendantPacket, IAncestorPacket
    {
        public IAncestorPacket DescendantPacket { get ; set; }
        public override string ToString()
        {
            return $"Command : {DescendantPacket?.ToString()} ";
        }
        public class Encoding : DescendantPacketEncoding<Command>
        {

            public static byte Id => 1;
            public Encoding(PacketEncoding encoding) : base(new AncestorPacketEncoding<Command>(encoding, Id))
            {
                
            }
            public Encoding(PacketEncoding encoding, IEnumerable<PacketEncoding> encodingsList) : this(encoding)
            {
                foreach (var encodingItem in encodingsList)
                {
                    Register(encodingItem);
                }
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<PacketEncodingBuilder> encodingBuileders) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o, encodingBuileders.Select(o => o.Build()).ToList()));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<PacketEncoding> encodings) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o, encodings));
        }
    }
}
