using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets.Records.Decorator
{
    class RecordWithSize : Record
    {
        private readonly Record _record;
        public override int TypeId => _record.TypeId;
        public override string ToString()
        {
            throw new NotImplementedException();
        }
        public RecordWithSize(Record record)
        {
            _record = record;
        }
        public int ByteSize { get; set; }
        public new class Encoding<T> : Record.Encoding<T> where T : Record
        {
            readonly Encoding<T> _encoding;
            public Encoding(Encoding<T> encoding)
            {
                this._encoding = encoding;
            }

            public override int TypeId => _encoding.TypeId;

            protected override void EncodeCore(T record, BinaryWriter writer)
            {
                if (!(record is RecordWithSize record1))
                    throw new ArgumentException("The record type is not supported.");
                writer.Write(record1.ByteSize);
                _encoding.Encode(record, writer);
            }

            protected override T DecodeCore(BinaryReader reader)
            {
                int byteSize = reader.ReadByte();
                var rec = _encoding.Decode(reader);
                ((RecordWithSize)rec).ByteSize = byteSize;
                return (T)rec;
            }
        }


    }
}
