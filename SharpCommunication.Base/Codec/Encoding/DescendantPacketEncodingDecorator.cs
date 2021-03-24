using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public class DescendantPacketEncoding<T> : EncodingDecorator, IDescendantPacketEncoding where T: IDescendantPacket, new()
        {
        public readonly IReadOnlyDictionary<Type, byte> IdDictionary;
        public readonly IReadOnlyDictionary<byte, EncodingDecorator> EncodingDictionary ;
        private readonly IDictionary<Type, byte> _idDictionary;
        private readonly IDictionary<byte, EncodingDecorator> _encodingDictionary;

        public DescendantPacketEncoding(EncodingDecorator encoding, IEnumerable<EncodingDecorator> encodingsList) : this(encoding)
        {
            foreach (var encodingItem in encodingsList)
            {
                Register(encodingItem);
            }
        }
        public DescendantPacketEncoding(EncodingDecorator encoding) : base(encoding)
        {
            _idDictionary = new Dictionary<Type, byte>();
            IdDictionary = new ReadOnlyDictionary<Type, byte>(_idDictionary);

            _encodingDictionary = new Dictionary<byte, EncodingDecorator>();
            EncodingDictionary = new ReadOnlyDictionary<byte, EncodingDecorator>(_encodingDictionary);
        }
        public void Register(EncodingDecorator encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var enc = (IAncestorPacketEncoding<IAncestorPacket>) encoding.FindDecoratedEncoding<IAncestorPacketEncoding<IAncestorPacket>>();
            if (enc == null)
                throw new NotSupportedException();

            _idDictionary.Add(enc.PacketType, enc.Id);
            _encodingDictionary.Add(enc.Id, encoding);
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            var descendantPacket = (IDescendantPacket)packet;
            _idDictionary.TryGetValue(descendantPacket.Content.GetType(), out var ancestorPacketId);
            writer.Write(ancestorPacketId);
            _encodingDictionary.TryGetValue(ancestorPacketId, out var encodingDecorator);
            encodingDecorator?.Encode(descendantPacket.Content, writer);
        }

        public override IPacket Decode(BinaryReader reader)
        {
            var packetEncodingId = reader.ReadByte();
            _encodingDictionary.TryGetValue(packetEncodingId, out var encodingDecorator);
            var obj = new T
            {
                Content = (IAncestorPacket)encodingDecorator?.Decode(reader)
            };
            return obj;
        }

    }
    public interface IDescendantPacketEncoding : IEncoding<IPacket>
    {
        void Register(EncodingDecorator encoding);

    }

}
