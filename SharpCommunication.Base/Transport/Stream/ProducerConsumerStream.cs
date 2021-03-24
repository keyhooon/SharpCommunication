using System;
using System.IO;

namespace SharpCommunication.Transport.Stream
{
    public class ProducerConsumerStream : System.IO.Stream
    {
        private readonly MemoryStream _innerStream;
        private long _readPosition;
        private long _writePosition;

        public ProducerConsumerStream()
        {
            _innerStream = new MemoryStream();
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override void Flush()
        {
            lock (_innerStream)
            {
                _innerStream.Flush();
            }
        }

        public override long Length
        {
            get
            {
                lock (_innerStream)
                {
                    return _innerStream.Length;
                }
            }
        }

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {

            var red = 0;
            while (red < count)
                lock (_innerStream)
                {
                    _innerStream.Position = _readPosition;
                    red += _innerStream.Read(buffer, offset + red, count - red);
                    _readPosition = _innerStream.Position;

                }

            return red;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (_innerStream)
            {
                _innerStream.Position = _writePosition;
                _innerStream.Write(buffer, offset, count);
                _writePosition = _innerStream.Position;
            }
        }
    }
}
