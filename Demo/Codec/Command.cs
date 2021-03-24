using System;
using System.Collections.Generic;
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace Demo.Codec
{
    public class Command : IDescendantPacket, IAncestorPacket
    {
        public IAncestorPacket Content { get ; set; }
        public override string ToString()
        {
            return $"Command {{ {Content?.ToString()} }}";
        }
        public class Encoding : DescendantPacketEncoding<Command>, IAncestorPacketEncoding<IAncestorPacket>
        {

            public byte Id => 1;

            public Type PacketType => typeof(Command);

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
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<PacketEncodingBuilder> encodingBuilders) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o, encodingBuilders.Select(builder => builder.Build()).ToList()));
            public static PacketEncodingBuilder CreateBuilder(IEnumerable<EncodingDecorator> encodings) =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o, encodings));
        }
    }
}
