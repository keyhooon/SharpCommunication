using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;

namespace Demo.Codec
{
    public class DataPacket : IPacket, IDescendantPacket, IAncestorPacket
    {
        public static byte ID = 1;
        public int Id => ID;

        public IAncestorPacket DescendantPacket { get; set; }
        public override string ToString()
        {
            return $"Data \r\n\t\t {DescendantPacket?.ToString()} ";
        }
    }
    public static class DataPacketEncodingHelper
    {
        public static PacketEncodingBuilder CreateDataPacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList)
        {
            return packetEncodingBuilder.WithAncestor(DataPacket.ID).WithDescendant<DataPacket>(encodingBuiledersList);
        }
        public static PacketEncodingBuilder CreateDataPacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncoding> encodingsList)
        {
            return packetEncodingBuilder.WithAncestor(DataPacket.ID).WithDescendant<DataPacket>(encodingsList);
        }
        public static PacketEncodingBuilder CreateDataPacket(this PacketEncodingBuilder packetEncodingBuilder)
        {
            return packetEncodingBuilder.WithAncestor(DataPacket.ID).WithDescendant<DataPacket>();
        }
    }
}
