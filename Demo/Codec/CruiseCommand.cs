using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;


namespace Demo.Codec
{
    class CruiseCommand : IFunctionPacket
    {
        public bool IsOn { get; set; }
        public byte[] Param { 
            get {
                return new byte[] { IsOn ? (byte)0x01 : (byte)0x00 };
            } 
            set {
                if (value != null && value.Length > 0 && value[0] == (byte)0x01)
                    IsOn = true;
            }
        }
        public Action Action => throw new NotImplementedException();
        public override string ToString()
        {

            return $"Cruise Command - Cruise : {IsOn}";
        }

        public class Encoding : FunctionPacketEncoding<CruiseCommand>
        {
            public static byte ParamByteCount = 1;
            public new const byte Id = 3;
            public Encoding(EncodingDecorator encoding) : base(encoding, ParamByteCount, Id)
            {

            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
        }
    }
}
