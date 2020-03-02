using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;
using System.Linq;

namespace Demo.Codec
{
    class CoreConfigurationPacket : IPacket, IAncestorPacket
    {
        public static byte id = 4;
        public static byte byteCount = 16;
        public byte Id => id;
        public string UniqueId { get; set; }
        public string FirmwareVersion{ get; set; }
        public string ModelVersion { get; set; }


        public override string ToString()
        {

            return $"Core Configuration - UniqueId : {UniqueId}, FirmwareVersion : {FirmwareVersion}, " +
                $"ModelVersion : {ModelVersion}";
        }
        public CoreConfigurationPacket()
        {

        }
        public class Encoding : AncestorPacketEncoding
        {

            public Encoding(PacketEncoding encoding) : base(encoding, id)
            {

            }
            public Encoding() : base(null, id)
            {

            }
            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (CoreConfigurationPacket)packet;
                byte crc8 = 0;
                byte[] value;
                value = o.UniqueId.ToByteArray();
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = o.FirmwareVersion.ToByteArray();
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                value = o.ModelVersion.ToByteArray();
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(16);
                byte crc8 = 0;
                for (int i = 0; i < value.Length; i++)
                    crc8 += value[i];
                if (crc8 == reader.ReadByte())
                    return new CoreConfigurationPacket()
                    {
                        UniqueId = value.Take(12).ToHexString(),
                        FirmwareVersion = value.Skip(12).Take(2).ToHexString(),
                        ModelVersion = value.Skip(14).Take(2).ToHexString(),
                    };
                return null;
            }
        }

    }

    public static class CoreConfigurationPacketHelper
    {
        public static PacketEncodingBuilder CreateCoreConfigurationEncodingBuilder(this PacketEncodingBuilder mapItemBuilder)
        {
            mapItemBuilder.SetupActions.Add(item => new CoreConfigurationPacket.Encoding(item));
            return mapItemBuilder;
        }

    }
}