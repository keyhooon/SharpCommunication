using SharpCommunication.Base.Codec.Packets;
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

        public override string ToString()
        {

            return $"Cruise : {IsOn}";
        }
        public Action Action => throw new NotImplementedException();

        public readonly static byte ParamByteCount = 1;
        public static readonly byte ID = 3;
        public int Id => ID;
    }
    public static class CruiseCommandEncodingHelper
    {
        public static PacketEncodingBuilder CreateCruiseCommand(this PacketEncodingBuilder packetEncodingBuilder)
        {
            return packetEncodingBuilder.WithFunction<CruiseCommand>(ReadCommand.ParamByteCount, CruiseCommand.ID);
        }
    }
}
