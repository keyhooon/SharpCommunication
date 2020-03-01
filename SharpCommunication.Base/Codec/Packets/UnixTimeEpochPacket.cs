using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IUnixTimeEpochPacket : IPacket
    {
         DateTime DateTime { get; set; }

    }
    public class HasUnixTimeEpochPacketEncoding : PacketEncoding
    {
        public HasUnixTimeEpochPacketEncoding(PacketEncoding encoding) : base(encoding)
        {
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            var unixTimeEpochPacket = (IUnixTimeEpochPacket)packet;
            writer.Write(unixTimeEpochPacket.DateTime.ToUnixEpoch());
            Encoding.EncodeCore(unixTimeEpochPacket, writer);
        }

        public override IPacket DecodeCore(BinaryReader reader)
        {
            var datetime = reader.ReadUInt32().ToUnixTime();
            var unixTimeEpochPacket = (IUnixTimeEpochPacket) Encoding.DecodeCore(reader);
            unixTimeEpochPacket.DateTime = datetime;
            return unixTimeEpochPacket;
        }

    }
    public static class HasUnixTimeEpochPacketHelper
    {
        public static PacketEncodingBuilder WithUnixTimeEpoch(this PacketEncodingBuilder mapItemBuilder)
        {
            mapItemBuilder.SetupActions.Add(item => new HasUnixTimeEpochPacketEncoding(item));
            return mapItemBuilder;
        }

    }
}

