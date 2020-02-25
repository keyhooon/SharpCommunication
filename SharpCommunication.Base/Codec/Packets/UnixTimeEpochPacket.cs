using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IUnixTimeEpochPacket : IPacket
    {
         DateTime DateTime { get; set; }

    }
    public class HasUnixTimeEpochPacketEncoding<T> : PacketEncoding<T> where T : IUnixTimeEpochPacket
    {
        public HasUnixTimeEpochPacketEncoding(IEncoding<IPacket> encoding) : base(encoding)
        {
        }

        public override void EncodeCore(T packet, BinaryWriter writer)
        {
            writer.Write(packet.DateTime.ToUnixEpoch());
            Encoding.EncodeCore(packet, writer);
        }

        public override T DecodeCore(BinaryReader reader)
        {
            var datetime = reader.ReadUInt32().ToUnixTime();
            var obj = (IUnixTimeEpochPacket) Encoding.DecodeCore(reader);
            obj.DateTime = datetime;
            return (T) obj;
        }

    }
    public static class HasUnixTimeEpochPacketHelper
    {
        public static PacketEncodingBuilder WithUnixTimeEpoch(this PacketEncodingBuilder mapItemBuilder)
        {
            mapItemBuilder.SetupActions.Add(item => (IEncoding<IPacket>)new HasUnixTimeEpochPacketEncoding<IUnixTimeEpochPacket>(item));
            return mapItemBuilder;
        }

    }
}

