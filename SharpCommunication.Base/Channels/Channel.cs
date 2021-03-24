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
        public event EventHandler<DataReceivedEventArg<TPacket>> DataReceived;
        public event EventHandler<Exception> ErrorReceived;
        protected Channel() 
        {

        }
        public Channel(ICodec<TPacket> codec, Stream stream) : this(codec, stream, stream) { }

        public Channel(ICodec<TPacket> codec, Stream inputstream, Stream outputstream)
        {
            Codec = codec;
            BufferStream = new BufferedStream(outputstream);

            Writer = new BinaryWriter(BufferStream);
            Reader = new BinaryReader(inputstream);
            _cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _cancellationTokenSource.Token;
            Task.Factory.StartNew(async() =>
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
            }, cancellationToken);
        }


        public virtual BinaryWriter Writer { get; }
        public virtual BinaryReader Reader { get; }
        public virtual ICodec<TPacket> Codec { get; }

        public virtual void Dispose()
        {
            _cancellationTokenSource.Cancel();
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
