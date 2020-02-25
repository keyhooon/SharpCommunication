using SharpCommunication.Base.Codec.Packets;
using System;

namespace Demo.Codec
{
    class DataPacket : IPacket, IDescendantPacket, IAncestorPacket
    {
        public int Id => 1;

        public IAncestorPacket DescendantPacket { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    }
}
