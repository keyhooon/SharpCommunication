using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Codec
{
    class ReadCommand : IFunctionPacket
    {
        public byte[] Param { get; set; }

        public Action Action => throw new NotImplementedException();

        public static byte ParamByteCount = 1;
        public static byte ID = 1;
        public int Id => ID;
    }
}
