using SharpCommunication.Base.Codec.Packets;
using System;

namespace Demo.Codec
{
    public class DataPacket : IPacket, IDescendantPacket, IAncestorPacket
    {
        public static int ID = 1;
        public int Id => ID;

        public IAncestorPacket DescendantPacket { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    }
}
