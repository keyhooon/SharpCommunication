//  *******************************************************************************
//  *  Licensed under the Apache License, Version 2.0 (the "License");
//  *  you may not use this file except in compliance with the License.
//  *  You may obtain a copy of the License at
//  *
//  *  http://www.apache.org/licenses/LICENSE-2.0
//  *
//  *   Unless required by applicable law or agreed to in writing, software
//  *   distributed under the License is distributed on an "AS IS" BASIS,
//  *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  *   See the License for the specific language governing permissions and
//  *   limitations under the License.
//  ******************************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec
{
    /// <summary>
    /// Date and time of fix
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Zda")]

    public class Zda : NmeaMessage
    {

        public override string MessageType => "Zda";
        /// <summary>
        /// Gets the time of fix
        /// </summary>
        public DateTime FixDateTime { get; private set; }
        [NmeaMessageType("ZDA")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding)
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Zda();
                if (message?.Count != 6)
                {
                    throw new ArgumentException("Invalid ZDA", nameof(message));
                }

                var time = StringToTimeSpan(message[0]);
                var day = int.Parse(message[1], CultureInfo.InvariantCulture);
                var month = int.Parse(message[2], CultureInfo.InvariantCulture);
                var year = int.Parse(message[3], CultureInfo.InvariantCulture);

                ret.FixDateTime = new DateTime(year, month, day, time.Hours, time.Minutes,
                    time.Seconds, DateTimeKind.Utc);

                // Index 4 and 5 is used to specify a local time zone.
                // However I haven't come across any receiver that actually
                // specify this, so we're just ignoring it.
                return ret;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
    PacketEncodingBuilder.CreateDefaultBuilder().WithAncestorGeneric<string>("ZDA", typeof(Zda)).AddDecorate(o => new Encoding(null));

        }
    }
}
