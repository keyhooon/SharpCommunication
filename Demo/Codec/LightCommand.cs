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

        private static byte ID = 2;
        public int Id => LightCommand.ID;
    }
}
