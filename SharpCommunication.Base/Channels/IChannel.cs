using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Channels
{
    public interface IChannel<TPacket> where TPacket : IPacket
    {
        ICodec<TPacket> Codec { get; }
        BinaryReader Reader { get; }
        BinaryWriter Writer { get; }
        void Dispose();
        void Transmit(TPacket packet);

        event EventHandler<DataReceivedEventArg<TPacket>> DataReceived;
        event EventHandler<Exception> ErrorReceived;
    }
    public interface IChannel : IChannel<IPacket>
    {
        
    }
}