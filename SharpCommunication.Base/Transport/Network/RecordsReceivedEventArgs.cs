using System;
using System.Collections.Generic;
using SharpCommunication.Base.Codec.Packets.Records;

namespace SharpCommunication.Base.Transport.Network
{
    public class RecordsReceivedEventArgs : EventArgs
    {
        public readonly IEnumerable<Record> Records;

        public RecordsReceivedEventArgs(IEnumerable<Record> records)
        {
            Records = records;
        }
    }
}