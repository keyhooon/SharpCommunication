using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{
    public class DescendantFlagPacketEncoding<T> : EncodingDecorator, IDescendantPacketEncoding where T : IListDescendantPacket, new()
    {
        public readonly bool ContainLength;
        public readonly IReadOnlyDictionary<Type, byte> IdDictionary;
        public readonly IReadOnlyList<EncodingDecorator> EncodingList;
        private readonly IDictionary<Type, byte> _idDictionary;
        private readonly IList<EncodingDecorator> _encodingList;

        public DescendantFlagPacketEncoding(EncodingDecorator encoding, IEnumerable<EncodingDecorator> encodingsList, bool containLength = false) : this(encoding, containLength)
        {
            foreach (var encodingItem in encodingsList)
            {
                Register(encodingItem);
            }
        }
        public DescendantFlagPacketEncoding(EncodingDecorator encoding, bool containLength = false) : base(encoding)
        {
            ContainLength = containLength;
            _idDictionary = new Dictionary<Type, byte>();
            IdDictionary = new ReadOnlyDictionary<Type, byte>(_idDictionary);

            _encodingList = new List<EncodingDecorator>(8);
            EncodingList = ((List < EncodingDecorator > )_encodingList).AsReadOnly();
            
        }

        public void Register(EncodingDecorator encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var enc = encoding.FindDecoratedEncoding<IAncestorPacketEncoding<IAncestorPacket>>();
            if (enc == null)
                throw new NotSupportedException();
            if (enc.Id> 8)
                throw new ArgumentOutOfRangeException();


            _idDictionary.Add(enc.PacketType, enc.Id);
            _encodingList[enc.Id] = encoding;
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            var descendantPacket = (IListDescendantPacket)packet;
            if (descendantPacket == null)
                return;
            byte flag = 0;
            var stream = new MemoryStream();
            var binarywriter = new BinaryWriter(stream);
            
            foreach (var content in descendantPacket.ContentsList)
            {
                _idDictionary.TryGetValue(content.GetType(), out var ancestorPacketId);
                flag |= ancestorPacketId;
                _encodingList[ancestorPacketId]?.Encode(content, binarywriter);
            }
            writer.Write(flag);
            if (ContainLength)
            {
                writer.Write(stream.Length);
            }
            writer.Write(stream.ToArray());

        }

        public override IPacket Decode(BinaryReader reader)
        {
            var packetEncodingId = reader.ReadByte();
            byte length = 0;
            if (ContainLength)
            {
                length = reader.ReadByte();
            }
            var obj = new T
            {
                ContentsList = new List<IAncestorPacket>() 
            };
            int index = 0;
            while(packetEncodingId != 0)
            {
                if ((packetEncodingId & 0x01) == 0x01)
                    obj.ContentsList.Add((IAncestorPacket)_encodingList[index++]?.Decode(reader));
                packetEncodingId >>= 1;
            }

            return obj;
        }
    }
}
