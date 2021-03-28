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
    /// Global Positioning System DOP and active satellites
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gsa")]

    public class Gsa : NmeaMessage
    {

        public override string MessageType => "Gsa";
        /// <summary>
        /// Mode
        /// </summary>
        public ModeSelection GpsMode { get; private set; }

        /// <summary>
        /// Mode
        /// </summary>
        public Mode FixMode { get; private set; }

        /// <summary>
        /// IDs of SVs used in position fix
        /// </summary>
        public IReadOnlyList<int> SVs { get; private set; }

        /// <summary>
        /// Dilution of precision
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdop")]
        public double Pdop { get; private set; }

        /// <summary>
        /// Horizontal dilution of precision
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hdop")]
        public double Hdop { get; private set; }

        /// <summary>
        /// Vertical dilution of precision
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Vdop")]
        public double Vdop { get; private set; }
        [NmeaMessageType("GSA")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, "GSA", typeof(Gsa))
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Gsa();
                if (message == null || message.Count < 17)
                    throw new ArgumentException("Invalid GSA", "message");

                ret.GpsMode = message[0] == "A" ? ModeSelection.Auto : ModeSelection.Manual;
                ret.FixMode = (Mode)int.Parse(message[1], CultureInfo.InvariantCulture);

                var svs = new List<int>();
                for (var i = 2; i < 14; i++)
                {
                    if (message[i].Length > 0 && int.TryParse(message[i], out var id))
                        svs.Add(id);
                }
                ret.SVs = svs.ToArray();

                ret.Pdop = double.TryParse(message[14], NumberStyles.Float, CultureInfo.InvariantCulture, out var tmp) ? tmp : double.NaN;

                ret.Hdop = double.TryParse(message[15], NumberStyles.Float, CultureInfo.InvariantCulture, out tmp) ? tmp : double.NaN;

                ret.Vdop = double.TryParse(message[16], NumberStyles.Float, CultureInfo.InvariantCulture, out tmp) ? tmp : double.NaN;

                return ret;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }

        /// <summary>
        /// Mode selection
        /// </summary>
        public enum ModeSelection
        {
            /// <summary>
            /// Auto
            /// </summary>
            Auto,
            /// <summary>
            /// Manual mode
            /// </summary>
            Manual,
        }
        /// <summary>
        /// Fix Mode
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "Enum values matches NMEA spec")]
        public enum Mode
        {
            /// <summary>
            /// Not available
            /// </summary>
            NotAvailable = 1,
            /// <summary>
            /// 2D Fix
            /// </summary>
            Fix2D = 2,
            /// <summary>
            /// 3D Fix
            /// </summary>
            Fix3D = 3
        }
    }
}