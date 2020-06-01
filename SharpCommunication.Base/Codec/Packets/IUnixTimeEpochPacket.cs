using System;

namespace SharpCommunication.Codec.Packets
{
    public interface IUnixTimeEpochPacket : IPacket
    {
         DateTime DateTime { get; set; }

    }
}

