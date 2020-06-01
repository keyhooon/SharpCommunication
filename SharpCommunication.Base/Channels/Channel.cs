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
        public virtual ICodec<TPacket> Codec { get; }
        protected readonly Stream _stream;
        protected readonly BufferedStream _bufferStream;
        private readonly CancellationTokenSource _cancellationTokenSource;
        public event EventHandler<DataReceivedEventArg<TPacket>> DataReceived;
        public Channel(Channel<TPacket> channel): this (channel.Codec, channel._stream)
        {

        }

        public Channel(ICodec<TPacket> codec, Stream stream)
        {
            Codec = codec;
            _stream = stream;
            _bufferStream = new BufferedStream(_stream);

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
                        OnDataReceived(new DataReceivedEventArg<TPacket>(packet));
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
            }, cancellationToken);
        }
        public BinaryWriter Writer { get; }
        public BinaryReader Reader { get; }



        public virtual void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _bufferStream?.Dispose();
            _cancellationTokenSource?.Dispose();
            Writer?.Dispose();
            Reader?.Dispose();
        }

        protected virtual void OnDataReceived(DataReceivedEventArg<TPacket> e)
        {
            DataReceived?.Invoke(this, e);
        }

        public void Transmit(TPacket packet)
        {
            Codec.Encode(packet, Writer);
            Writer.Flush();
        }
    }
}
