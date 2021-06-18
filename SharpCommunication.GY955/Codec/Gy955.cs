using System;
using System.IO;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.GY955.Codec
{
    public class Gy955 : IPacket
    {
        public Output Output { get; set; }

        public Command Command { get; set; }

        public class Encoding : SeparateInputOutputEncodingDecorator<Output.Encoding, Command.Encoding, Output, Command>
        {
            public Encoding() : base(new Output.Encoding(null), new Command.Encoding(null))
            {

            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                if (packet.GetType() != typeof(Gy955))
                    throw new ArgumentException();
                var gy = (Gy955) packet;
                base.Encode(gy.Command, writer);
            }

            public override IPacket Decode(BinaryReader reader)
            {
                return new Gy955()
                {
                    Output = (Output) base.Decode(reader)
                };

            }
        }
    }
}
