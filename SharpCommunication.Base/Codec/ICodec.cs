﻿using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec
{
    public interface ICodec<T> where T : IPacket
    {

        Type DataType { get; }

        EncodingDecorator Encoding { get; }

        void Encode(T data, BinaryWriter stream);

        byte[] Encode(T data);

        T Decode(BinaryReader stream);

        T Decode(byte[] bytes);
    }
}
