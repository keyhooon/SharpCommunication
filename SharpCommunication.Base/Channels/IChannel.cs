using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Base.Channels
{
    public interface IChannel<TPacket> where TPacket : IPacket
    {
        ICodec<TPacket> Codec { get; }
        BinaryReader Reader { get; }
        BinaryWriter Writer { get; }
        void Dispose();
        void Transmit(TPacket packet);
        event EventHandler<DataReceivedEventArg<TPacket>> DataReceived;
    }
}