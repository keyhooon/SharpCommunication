using System;
using System.IO;

namespace SharpCommunication.Base.Codec.Packets.Records
{

    public abstract class Record : Packet
    {

        public abstract class Encoding : Encoding<Record>
        {

        }
    }
}
