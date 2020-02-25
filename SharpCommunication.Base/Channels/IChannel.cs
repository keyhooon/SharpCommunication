using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Base.Channels
{
    public interface IChannel<T> where T : IPacket, new() 
    {
        ICodec<T> Codec { get; }
        BinaryReader Reader { get; }
        BinaryWriter Writer { get; }
        void Dispose();
        event EventHandler<DataReceivedEventArg<T>> DataReceived;
    }
}