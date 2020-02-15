using System;
using System.Collections.Generic;

namespace SharpCommunication.Base.Codec.Packets.Records.Data
{
    public class DataEncodingFactory
    {
        private static DataEncodingFactory _instance;

        public static DataEncodingFactory Instance => _instance ??= new DataEncodingFactory();

        private readonly IDictionary<int, IDataEncoding> _encodings = new Dictionary<int, IDataEncoding>();

        private DataEncodingFactory()
        {

        }

        public IDataEncoding Create(int packetId)
        {
            if (!_encodings.TryGetValue(packetId & 0xf, out var dataEncoding) || dataEncoding == null)
                throw new NotSupportedException($"The data encoding for data 0x{packetId:X} is not supported.");
            return dataEncoding;
        }

        private void Register(IDataEncoding encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            _encodings.Add(encoding.Id, encoding);
        }
    }
}
