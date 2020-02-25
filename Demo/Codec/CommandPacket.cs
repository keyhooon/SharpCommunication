using SharpCommunication.Base.Codec.Packets;
using System;

namespace Demo.Codec
{
    public class CommandPacket : IPacket, IDescendantPacket, IAncestorPacket
    {
        public int Id => 2;

        public IAncestorPacket DescendantPacket { get ; set; }

    }
}
