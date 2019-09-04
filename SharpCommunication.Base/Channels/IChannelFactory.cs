using SharpCommunication.Base.Codec;
using System;
using System.IO;

namespace SharpCommunication.Base.Channels
{
    interface IChannelFactory
    {
        ICodec Codec { get; }

        Channel Create(Stream stream, IDisposable streamingObject);
    }
}
