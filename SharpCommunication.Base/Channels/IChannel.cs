using System;
using System.IO;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Channels
{
    public interface IChannel
    {
        ICodec Codec { get; }
        BinaryReader Reader { get; }
        BinaryWriter Writer { get; }
        void Dispose();
        event EventHandler<DataReceivedEventArg> DataReceived;
    }
}