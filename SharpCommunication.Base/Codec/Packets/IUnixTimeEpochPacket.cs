using System;

namespace SharpCommunication.Base.Codec.Packets
{
    public interface IUnixTimeEpochPacket : IPacket
    {
         DateTime DateTime { get; set; }

    }
}

