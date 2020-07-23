using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpCommunication.Transport.Stream
{
    public class ProducerConsumerStream : System.IO.Stream
    {
        private readonly MemoryStream innerStream;
        private long readPosition;
        private long writePosition;

        public ProducerConsumerStream()
        {
            innerStream = new MemoryStream();
        }

        public override bool CanRead { get { return true; } }

        public override bool CanSeek { get { return false; } }

        public override bool CanWrite { get { return true; } }

        public override void Flush()
        {
            lock (innerStream)
            {
                innerStream.Flush();
            }
        }

        public override long Length
        {
            get
            {
                lock (innerStream)
                {
                    return innerStream.Length;
                }
            }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {

            int red = 0;
            while (red < count)
                lock (innerStream)
                {
                    innerStream.Position = readPosition;
                    red += innerStream.Read(buffer, offset + red, count - red);
                    readPosition = innerStream.Position;

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
            lock (innerStream)
            {
                innerStream.Position = writePosition;
                innerStream.Write(buffer, offset, count);
                writePosition = innerStream.Position;
            }
        }
    }
}
