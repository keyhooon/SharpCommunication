using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpCommunication.Base.Codec.Packets.Records.CodeBook
{
    public sealed class MultiCodeBookRecord : Packet
    {


        private const int MultiCodeBookRecordId = 4;

        public override int TypeId => MultiCodeBookRecordId;

        public IEnumerable<CodeBookRecord> Records { get; set; }

        public MultiCodeBookRecord(IEnumerable<CodeBookRecord> records)
        {

            Records = records;
        }


        public override string ToString()
        {
            using (var stringWriter = new StringWriter())
            {
                stringWriter.Write("RECORD ");

                stringWriter.Write(", Records: ");
                stringWriter.Write(Records.Count());
                stringWriter.WriteLine("[");
                foreach (var record in Records)
                    stringWriter.WriteLine(record);
                stringWriter.Write("]");
                return stringWriter.ToString();
            }
        }

        public sealed class Encoding : Encoding<MultiCodeBookRecord>
        {
            private static Encoding<MultiCodeBookRecord> _instance;

            public static Encoding<MultiCodeBookRecord> Instance => _instance ??= new Encoding();
            public override int TypeId => MultiCodeBookRecordId;

            protected override void EncodeCore(MultiCodeBookRecord record, BinaryWriter writer)
            {
                var records = record.Records.ToArray();
                writer.Write((byte)records.Count());
                foreach (var rec in records) 
                    CodeBookRecord.Encoding.Instance.Encode(rec, writer);
            }

            protected override MultiCodeBookRecord DecodeCore(BinaryReader reader)
            {
                var recordCount = reader.ReadByte();
                return new MultiCodeBookRecord(Enumerable.Range(0, recordCount).Select(i => (CodeBookRecord) CodeBookRecord.Encoding.Instance.Decode(reader)));
            }
        }
    }
}
