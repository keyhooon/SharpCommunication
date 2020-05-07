using System;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IFunctionPacket : IPacket, IAncestorPacket
    {
        byte[] Param { get; set; }
        Action Action { get; }
    }

}
