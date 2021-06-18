using SharpCommunication.Codec.Packets;
using System;

namespace SharpCommunication.Codec.Encoding
{

    public class AncestorPacketEncoding : AncestorGenericPacketEncodingDecorator<byte>, IAncestorPacketEncoding<IAncestorPacket> 
    {


        public AncestorPacketEncoding(EncodingDecorator encoding, byte id, Type packetType) : base(encoding, id, packetType)
        {

        }

    }
  

}
