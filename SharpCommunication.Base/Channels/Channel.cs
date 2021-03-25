using SharpCommunication.Codec;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCommunication.Channels
{
    public class Channel<TPacket> : IDisposable, IChannel<TPacket> where TPacket : IPacket
    {
        protected readonly BufferedStream BufferStream;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Task<Task> _receptionTask;
        public event EventHandler<DataReceivedEventArg<TPacket>> DataReceived;
        public event EventHandler<Exception> ErrorReceived;
        protected Channel() 
        {

        }
        public Channel(ICodec<TPacket> codec, Stream stream) : this(codec, stream, stream) { }

        public Channel(ICodec<TPacket> codec, Stream inputStream, Stream outputStream)
        {
            Codec = codec;
            BufferStream = new BufferedStream(outputStream);

            Writer = new BinaryWriter(BufferStream);
            Reader = new BinaryReader(inputStream);
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            
            _receptionTask = Task.Factory.StartNew(async() =>
            {
                while (true)
                {
                    try
                    {
                        if (cancellationToken.IsCancellationRequested)
                            break;
                        var packet = codec.Decode(Reader);
                        OnDataReceived(new DataReceivedEventArg<TPacket>(packet));
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        OnErrorReceived(ex);
                        await Task.Delay(100);
                    }
                }
            });
        }


        public virtual BinaryWriter Writer { get; }
        public virtual BinaryReader Reader { get; }
        public virtual ICodec<TPacket> Codec { get; }

        public virtual void Dispose()
        {
            _cancellationTokenSource.Cancel();
            Task.WaitAll(new Task[] {_receptionTask}, 1000);
            _cancellationTokenSource.Dispose();
            BufferStream?.Dispose();
            Writer?.Dispose();
            Reader?.Dispose();
        }

        protected virtual void OnDataReceived(DataReceivedEventArg<TPacket> e)
        {
            DataReceived?.Invoke(this, e);
        }
        protected virtual void OnErrorReceived(Exception ex)
        {
            ErrorReceived?.Invoke(this, ex);
        }


        public virtual void Transmit(TPacket packet)
        {
            Codec.Encode(packet, Writer);
            Writer.Flush();
        }
    }
}
