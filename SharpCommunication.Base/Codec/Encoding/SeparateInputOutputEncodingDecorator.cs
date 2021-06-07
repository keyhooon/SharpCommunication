using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec.Encoding
{
    public class SeparateInputOutputEncodingDecorator<TEncodingInput, TEncodingOutput, TInputPacket, TOutputPacket> : EncodingDecorator 
        where TInputPacket : IPacket 
        where TOutputPacket : IPacket 
        where TEncodingInput : EncodingDecorator
        where TEncodingOutput : EncodingDecorator
    {
        private readonly TEncodingInput _encodingInput;
        private readonly TEncodingOutput _encodingOutput;

        public SeparateInputOutputEncodingDecorator(TEncodingInput encodingInput, TEncodingOutput encodingOutput) : base(null)
        {
            _encodingInput = encodingInput;
            _encodingOutput = encodingOutput;
        }

        public override void Encode(IPacket packet, BinaryWriter writer)
        {
            _encodingOutput.Encode((TOutputPacket) packet, writer);
        }

        public override IPacket Decode(BinaryReader reader)
        {
            return _encodingInput.Decode(reader);
        }
    }
}
