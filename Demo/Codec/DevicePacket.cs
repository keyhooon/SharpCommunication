using SharpCommunication.Base.Codec.Packets;
using System;
using System.Collections.Generic;

namespace Demo.Codec
{
    public class DevicePacket : IPacket, IHeaderPacket, IDescendantPacket
    {

        public static readonly byte[] Header = { 0x55, 0x55 };

        public IAncestorPacket DescendantPacket { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



    }
}
