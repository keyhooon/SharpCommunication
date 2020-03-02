using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Codec
{
    class LightCommand : IFunctionPacket
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
                return $"Light Command - Light : {LightId}, State : On";
            return $"Light : {LightId}, State : Off";
        }
        public Action Action => throw new NotImplementedException();

        public readonly static byte ParamByteCount = 2;
        public readonly static byte ID = 2;
        public int Id => ID;

    }
    public static class LightCommandEncodingHelper
    {
        public static PacketEncodingBuilder CreateLightCommand(this PacketEncodingBuilder packetEncodingBuilder)
        {
            return packetEncodingBuilder.WithFunction<LightCommand>(ReadCommand.ParamByteCount, LightCommand.ID);
        }
    }
}
