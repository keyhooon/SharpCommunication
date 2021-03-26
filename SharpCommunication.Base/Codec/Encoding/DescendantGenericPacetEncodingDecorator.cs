using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec.Encoding
{
    public class DescendantGenericPacketEncodingDecorator<T, TG> : EncodingDecorator, IDescendantPacketEncoding where T : IDescendantPacket, new() 
    {
        public readonly IReadOnlyDictionary<Type, string> IdDictionary;
        public readonly IReadOnlyDictionary<string, EncodingDecorator> EncodingDictionary;
        private readonly IDictionary<Type, string> _idDictionary;
        private readonly IDictionary<string, EncodingDecorator> _encodingDictionary;

        public DescendantGenericPacketEncodingDecorator(EncodingDecorator encoding, IEnumerable<EncodingDecorator> encodingsList) : this(encoding)
        {
            foreach (var encodingItem in encodingsList)
            {
                Register(encodingItem);
            }
        }
        public DescendantGenericPacketEncodingDecorator(EncodingDecorator encoding) : base(encoding)
        {
            _idDictionary = new Dictionary<Type, string>();
            IdDictionary = new ReadOnlyDictionary<Type, string>(_idDictionary);

            _encodingDictionary = new Dictionary<string, EncodingDecorator>();
            EncodingDictionary = new ReadOnlyDictionary<string, EncodingDecorator>(_encodingDictionary);
        }
        public void Register(EncodingDecorator encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var enc = (IAncestorPacketEncoding<IAncestorPacket, TG>)encoding.FindDecoratedEncoding<IAncestorPacketEncoding<IAncestorPacket>>();
            if (enc == null)
                throw new NotSupportedException();

            _idDictionary.Add(enc.PacketType, enc.Id.ToString());
            _encodingDictionary.Add(enc.Id.ToString(), encoding);
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
            _encodingDictionary.TryGetValue(packetEncodingId.ToString(), out var encodingDecorator);
            var obj = new T
            {
                Content = (IAncestorPacket)encodingDecorator?.Decode(reader)
            };
            return obj;
        }

    }
}
