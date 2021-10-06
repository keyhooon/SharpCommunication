using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec.Encoding
{
    public class DescendantFlagPacketEncodingDecorator<T> : EncodingDecorator, IDescendantPacketEncoding where T : IDescendantPacket, new()
    {
        public readonly IReadOnlyDictionary<Type, byte> IdDictionary;
        public readonly IReadOnlyList<EncodingDecorator> EncodingDictionary;
        private readonly IDictionary<Type, byte> _idDictionary;
        private readonly EncodingDecorator[] _encodingDictionary;

        public override IPacket Decode(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            var descendantPacket = (IListDescendantPacket)packet;
            byte flag = 0;
            var idList = new List<byte>();
            foreach (var ancestorPacket in descendantPacket.ContentsList)
            {
                _idDictionary.TryGetValue(ancestorPacket.GetType(), out var ancestorPacketId);
                flag |= ancestorPacketId;
            }
            writer.Write(flag);
            for (int i = 0; i < 8; i++)
            {
                if ((flag & 0x01) == 0x01)
                {

                }

                flag = (byte) (flag >> 1);
            }
            foreach (var ancestorPacket in descendantPacket.ContentsList)
            {
                _idDictionary.TryGetValue(ancestorPacket.GetType(), out var ancestorPacketId);

                writer.Write(ancestorPacketId);
                _encodingDictionary.TryGetValue(ancestorPacketId, out var encodingDecorator);
                encodingDecorator?.Encode(ancestorPacket, writer);
            }

        }

        public void Register(EncodingDecorator encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var enc = encoding.FindDecoratedEncoding<IAncestorPacketEncoding<IAncestorPacket>>();
            if (enc == null)
                throw new NotSupportedException();

            _idDictionary.Add(enc.PacketType, enc.Id);
            _encodingDictionary[enc.Id] = encoding;
        }

        public DescendantFlagPacketEncodingDecorator(EncodingDecorator encoding, IEnumerable<EncodingDecorator> encodingsList) : this(encoding)
        {
            foreach (var encodingItem in encodingsList)
            {
                Register(encodingItem);
            }
        }
        public DescendantFlagPacketEncodingDecorator(EncodingDecorator encoding) : base(encoding)
        {
            _idDictionary = new Dictionary<Type, byte>();
            IdDictionary = new ReadOnlyDictionary<Type, byte>(_idDictionary);

            _encodingDictionary = new EncodingDecorator[8];
            EncodingDictionary = new ReadOnlyCollection<EncodingDecorator>(_encodingDictionary);
        }
    }
}
