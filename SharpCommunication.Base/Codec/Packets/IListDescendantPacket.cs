using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCommunication.Codec.Packets
{
    public interface IListDescendantPacket : IPacket
    {
        List<IAncestorPacket> ContentsList { get; set; }
    }
}
