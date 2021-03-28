using SharpCommunication.Codec.Packets;
using System;
using System.IO;

namespace SharpCommunication.Codec.Encoding
{

    public class AncestorPacketEncoding : AncestorGenericPacketEncodingDecorator<byte>, IAncestorPacketEncoding<IAncestorPacket> 
    {


        public AncestorPacketEncoding(EncodingDecorator encoding, byte id, Type packetType) : base(encoding, id, packetType)
        {

        }

    }
  

}
