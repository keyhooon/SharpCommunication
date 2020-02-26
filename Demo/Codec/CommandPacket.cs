using SharpCommunication.Base.Codec.Packets;
using System;

namespace Demo.Codec
{
    public class CommandPacket : IPacket, IDescendantPacket, IAncestorPacket
    {
        public static int ID = 2;
        public int Id => ID;

        public IAncestorPacket DescendantPacket { get ; set; }

    }
}
