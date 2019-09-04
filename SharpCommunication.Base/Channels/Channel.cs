using SharpCommunication.Base.Codec;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCommunication.Base.Channels
{
    public class Channel : IDisposable, IChannel
    {

        public virtual ICodec Codec { get; }

        private readonly BufferedStream _bufferStream;
        private readonly CancellationTokenSource _cancellationTokenSource;
        public event EventHandler<DataReceivedEventArg> DataReceived;

        public Channel(ICodec codec, Stream stream, IDisposable streamingObject)
        {
            Codec = codec;
            StreamingObject = streamingObject;

            _bufferStream = new BufferedStream(stream);

            Writer = new BinaryWriter(_bufferStream);
            Reader = new BinaryReader(stream);
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        var packet = codec.Decode(Reader);
                        OnDataReceived(new DataReceivedEventArg(packet));
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }, cancellationToken);
        }

        public BinaryWriter Writer { get; }
        public BinaryReader Reader { get; }
        public IDisposable StreamingObject { get; }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            StreamingObject?.Dispose();
            _bufferStream?.Dispose();
            _cancellationTokenSource?.Dispose();
            Writer?.Dispose();
            Reader?.Dispose();
        }



        protected virtual void OnDataReceived(DataReceivedEventArg e)
        {
            DataReceived?.Invoke(this, e);
        }
    }
}
