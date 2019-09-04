using System;
using System.IO;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;

namespace SharpCommunication.Base.Channels
{
    public class Channel<T> : Channel where T : IPacket
    {
        public new event EventHandler<DataReceivedEventArg<T>> DataReceived;


        protected override void OnDataReceived(DataReceivedEventArg e)
        {
            DataReceived?.Invoke(this, new DataReceivedEventArg<T>((T)e.Data));
        }

        public Channel(ICodec codec, Stream stream, IDisposable streamingObject) : base(codec, stream, streamingObject) { }
    }
}
