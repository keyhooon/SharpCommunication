using System.Collections.Generic;

namespace SharpCommunication.Codec.Packets
{
    public interface IListDescendantPacket : IPacket
    {
        List<IAncestorPacket> ContentsList { get; set; }
    }
}
