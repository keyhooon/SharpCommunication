using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpCommunication.Base.Codec.Packets.Records.CodeBook
{
    public class CodeBookRecord : Record
    {
        private const int CodeBookRecordId = 3;
        public override int TypeId => CodeBookRecordId;
        public IList<Field> Fields { get; set; }
        public CodeBookRecord(IEnumerable<Field> fields)
        {
            Fields = fields.ToList();
        }

        public override string ToString()
        {
            using var stringWriter = new StringWriter();
            stringWriter.Write("CodeBookRecord ");

            stringWriter.Write(", Fields: ");
            stringWriter.Write(Fields.Count());
            stringWriter.WriteLine("[");
            foreach (var field in Fields)
                stringWriter.WriteLine(field);
            stringWriter.Write("]");
            return stringWriter.ToString();
        }

        public new class Encoding : Encoding<CodeBookRecord>
        {
            private static Encoding<CodeBookRecord> _instance;

            public static Encoding<CodeBookRecord> Instance => _instance ??= new Encoding();

            public override int TypeId => CodeBookRecordId;

            protected override CodeBookRecord DecodeCore(BinaryReader reader)
            {
                return new CodeBookRecord(Enumerable.Range(0, reader.ReadByte()).Select(i => FieldEncoding.Instance.Decode(reader)));
            }

            protected override void EncodeCore(CodeBookRecord record, BinaryWriter writer)
            {
                foreach (var field in record.Fields)
                    FieldEncoding.Instance.Encode(field, writer);
            }
        }


    }
}
