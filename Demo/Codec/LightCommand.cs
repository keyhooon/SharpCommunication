using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Codec
{
    class LightCommand : IFunctionPacket
    {
        public byte[] Param { get; set; }

        public Action Action => throw new NotImplementedException();

        public static byte ParamByteCount = 2;
        public static byte ID = 2;
        public int Id => ID;

    }
}
