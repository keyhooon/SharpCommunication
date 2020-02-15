using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets.Records.Data
{
    public class DataRecord : Record
    {
        const int DataRecordId = 1;
        public override int TypeId => DataRecordId;
        public Data Data { get; set; }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public new class Encoding : Encoding<DataRecord>
        {
            private static Encoding<DataRecord> _instance;

            public static Encoding<DataRecord> Instance
            {
                get
                {
                    var encoding = _instance ??= new Encoding();
                    return encoding;
                }
            }

            public override int TypeId => DataRecordId;


            protected override DataRecord DecodeCore(BinaryReader reader)
            {
                return new DataRecord
                {

                    Data = DataEncodingFactory.Instance.Create(reader.ReadByte()).Decode(reader)
                };
            }


            protected override void EncodeCore(DataRecord record, BinaryWriter writer)
            {
                DataEncodingFactory.Instance.Create(record.Data.Id).Encode(record.Data, writer);
            }
        }

    }
}
