using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;

namespace Demo.Codec
{
    public class DevicePacket : IPacket, IDescendantPacket
    {

        public static readonly byte[] Header = { 0x55, 0x55 };

        public IAncestorPacket DescendantPacket { get; set; }
        public override string ToString()
        {
            return $"DevicePacket \r\n\t {DescendantPacket?.ToString()} ";
        }


    }
    public static class DevicePacketEncodingHelper
    {
        public static PacketEncodingBuilder CreateDevicePacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncodingBuilder> encodingBuiledersList)
        {
            return packetEncodingBuilder.WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(encodingBuiledersList);
        }
        public static PacketEncodingBuilder CreateDevicePacket(this PacketEncodingBuilder packetEncodingBuilder, IEnumerable<PacketEncoding> encodingsList) 
        {
            return packetEncodingBuilder.WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>(encodingsList);
        }
        public static PacketEncodingBuilder CreateDevicePacket(this PacketEncodingBuilder packetEncodingBuilder) 
        {
            return packetEncodingBuilder.WithHeader(DevicePacket.Header).WithDescendant<DevicePacket>();
        }
    }
}
