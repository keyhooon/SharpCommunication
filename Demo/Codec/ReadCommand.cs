using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace Demo.Codec
{
    class ReadCommand : IFunctionPacket
    {
        public byte DataId { get; set; }

        public byte[] Param
        {
            get
            {
                return new byte[] { DataId };
            }
            set
            {
                if (value != null && value.Length > 0)
                    DataId = value[0];

            }
        }
        public Action Action { get; }
        public override string ToString()
        {

            return $"Read Command - Request Data: {DataId}";
        }

        public class Encoding : FunctionPacketEncoding<ReadCommand>
        {
            private static byte ParamByteCount = 1;
            public new const byte Id = 1;
            public Encoding(EncodingDecorator encoding) : base(encoding, ParamByteCount, Id)
            {

            }
            public static PacketEncodingBuilder CreateBuilder()=>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
        }

    }
}
