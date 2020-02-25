using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Codec.Packets;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCommunication.Base.Channels
{
    public class Channel<T> : IDisposable, IChannel<T> where T : IPacket, new()
    {

        public virtual ICodec<T> Codec { get; }

        private readonly BufferedStream _bufferStream;
        private readonly CancellationTokenSource _cancellationTokenSource;
        public event EventHandler<DataReceivedEventArg<T>> DataReceived;

        public Channel(ICodec<T> codec, Stream stream)
        {
            Codec = codec;
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
                        OnDataReceived(new DataReceivedEventArg<T>(packet));
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

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _bufferStream?.Dispose();
            _cancellationTokenSource?.Dispose();
            Writer?.Dispose();
            Reader?.Dispose();
        }

        protected virtual void OnDataReceived(DataReceivedEventArg<T> e)
        {
            DataReceived?.Invoke(this, e);
        }
    }
}
