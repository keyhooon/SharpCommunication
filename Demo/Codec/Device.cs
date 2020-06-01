﻿using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Codec
{
    public class Device : IPacket, IDescendantPacket
    {


        public IAncestorPacket DescendantPacket { get; set; }
        public override string ToString()
        {
            return $"DevicePacket : {DescendantPacket?.ToString()} ";
        }
        public class Encoding : DescendantPacketEncoding<Command>
        {
            private static readonly byte[] Header = { 0xAA, 0xAA };

            public Encoding(PacketEncoding encoding) : base(new HeaderPacketEncoding(encoding, Header))
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