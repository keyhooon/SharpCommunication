using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Text;

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

            return $"Read Command - Request Data: {DataId}";
        }
        public Action Action { get; }

        public readonly static byte ParamByteCount = 1;
        public readonly static byte ID = 1;
        public byte Id => ID;
    }
    public static class ReadCommandEncodingHelper
    {
        public static PacketEncodingBuilder CreateReadCommand(this PacketEncodingBuilder packetEncodingBuilder)
        {
            return packetEncodingBuilder.WithFunction<ReadCommand>(ReadCommand.ParamByteCount, ReadCommand.ID);
        }
    }
}
