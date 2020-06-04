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

        public override string ToString()
        {

            return $"Read Command {{ Request Data: {DataId} }}";
        }

        public class Encoding : FunctionPacketEncoding<ReadCommand>
        {
            public override byte ParameterByteCount => 1;
            public override byte Id => 1;
            public override Action<byte[]> ActionToDo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }

            public static PacketEncodingBuilder CreateBuilder()=>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
        }

    }
}
