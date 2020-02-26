using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Text;

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

        public static byte ParamByteCount = 1;
        private static int ID = 3;
        public int Id => ID;
    }
}
