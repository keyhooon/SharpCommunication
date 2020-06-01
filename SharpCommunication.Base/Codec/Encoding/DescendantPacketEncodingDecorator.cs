using SharpCommunication.Codec.Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public class DescendantPacketEncoding<T> : EncodingDecorator, IDescendantPacketEncoding<T> where T : IDescendantPacket, new()
        {
        public readonly IReadOnlyDictionary<byte, AncestorPacketEncoding<IAncestorPacket>> DecoderDictionary ;
        public readonly IReadOnlyDictionary<Type, AncestorPacketEncoding<IAncestorPacket>> EncoderDictionary;
        private readonly IDictionary<byte, AncestorPacketEncoding<IAncestorPacket>> _decoderDictionary;
        private readonly IDictionary<Type, AncestorPacketEncoding<IAncestorPacket>> _encoderDictionary;
        public DescendantPacketEncoding(EncodingDecorator encoding, IEnumerable<EncodingDecorator> encodingsList) : this(encoding)
        {
            foreach (var encodingItem in encodingsList)
            {
                Register(encodingItem);
            }
        }
        public DescendantPacketEncoding(EncodingDecorator encoding) : base(encoding)
        {
            _decoderDictionary = new Dictionary<byte, AncestorPacketEncoding<IAncestorPacket>>();
            DecoderDictionary = new ReadOnlyDictionary<byte, AncestorPacketEncoding<IAncestorPacket>>(_decoderDictionary);
            _encoderDictionary = new Dictionary<Type, AncestorPacketEncoding<IAncestorPacket>>();
           EncoderDictionary = new ReadOnlyDictionary<Type, AncestorPacketEncoding<IAncestorPacket>>(_encoderDictionary);
        }
        public void Register(EncodingDecorator encoding)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            
            var enc = encoding.FindDecoratedEncoding<AncestorPacketEncoding<IAncestorPacket>>();
            if (enc == null)
                throw new NotSupportedException();

            _decoderDictionary.Add(enc.Id, enc);
            _encoderDictionary.Add(enc.PacketType, enc);
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            Encode((T)packet, writer);
        }

        public override IPacket DecodeCore(BinaryReader reader)
        {
            return Decode(reader);
        }

        public void Encode(T packet, BinaryWriter writer)
        {
            var descendantPacket = (IDescendantPacket)packet;
            EncoderDictionary.TryGetValue(descendantPacket.DescendantPacket.GetType(), out var packetEncoding);
            writer.Write(packetEncoding.Id);
            packetEncoding.EncodeCore(descendantPacket.DescendantPacket, writer);
        }

        public T Decode(BinaryReader reader)
        {
            var packetEncodingId = reader.ReadByte();
            DecoderDictionary.TryGetValue(packetEncodingId, out var packetEncoding);
            T obj = new T
            {
                DescendantPacket = (IAncestorPacket)packetEncoding?.DecodeCore(reader)
            };
            return obj;
        }
    }
    public interface IDescendantPacketEncoding<T> : IEncoding<T> where T : IDescendantPacket
    {
        void Register(EncodingDecorator encoding);

    }

}
