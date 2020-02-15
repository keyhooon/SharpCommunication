using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets.Records.Decorator
{
    class RecordWithTime : Record
    {
        private readonly Record _record;
        public override int TypeId => _record.TypeId;
        public override string ToString()
        {
            throw new NotImplementedException();
        }
        public RecordWithTime(Record record)
        {
            _record = record;
        }
        public DateTime Time { get; set; }
        public new class Encoding<T> : Record.Encoding<T> where T : Record
        {
            readonly Encoding<T> _encoding;
            public Encoding(Encoding<T>  encoding)
            {
                _encoding = encoding;
            }

            public override int TypeId => _encoding.TypeId;

            protected override void EncodeCore(T record, BinaryWriter writer)
            {
                if (!(record is RecordWithTime record1))
                    throw new ArgumentException("The record type is not supported.");
                writer.Write(record1.Time.ToUnixEpoch());
                _encoding.Encode(record, writer);
            }

            protected override T DecodeCore(BinaryReader reader)
            {
                var time = reader.ReadUInt32().ToUnixTime();
                var rec = _encoding.Decode(reader);
                ((RecordWithTime)rec).Time = time;
                return (T) rec;
            }
        }


    }
}
