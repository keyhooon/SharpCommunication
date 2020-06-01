﻿using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public class AncestorPacketEncoding<T> : PacketEncoding where T : IAncestorPacket
    {
        public byte Id { get; }
        public Type PacketType => typeof(T);
        public AncestorPacketEncoding(PacketEncoding encoding, byte id) : base(encoding)
        {
            Id = id;
        }

        public override void EncodeCore(IPacket packet, BinaryWriter writer)
        {
            Encoding.EncodeCore(packet, writer);
        }

        public override IPacket DecodeCore(BinaryReader reader)
        {
            return Encoding.DecodeCore(reader);
        }
        public void Encode(T packet, BinaryWriter writer)
        {
            EncodeCore(packet, writer);
        }

        public T Decode(BinaryReader reader)
        {
            return (T)DecodeCore(reader);
        }
    }

}