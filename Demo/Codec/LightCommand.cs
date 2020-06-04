using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;

namespace Demo.Codec
{
    public class LightCommand : IFunctionPacket
    {
        public byte LightId { get; set; }
        public bool IsOn { get; set; }
        public byte[] Param
        {
            get
            {
                return new byte[] { LightId,  IsOn ? (byte)0x01 : (byte)0x00 };
            }
            set
            {
                if (value != null && value.Length > 1)
                {
                    LightId = value[0];
                    IsOn = value[1] == (byte)0x01;
                }
            }
        }
        public override string ToString()
        {
            if (IsOn)
                return $"Light Command {{ Light : {LightId}, State : On }}";
            return $"Light Command {{ Light : {LightId}, State : Off }}";
        }
        public class Encoding : FunctionPacketEncoding<LightCommand>
        {
            public override byte ParameterByteCount => 1;
            public override byte Id => 2;

            public override Action<byte[]> ActionToDo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public Encoding(EncodingDecorator encoding) : base(encoding)
            {

            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
        }

    }
}
