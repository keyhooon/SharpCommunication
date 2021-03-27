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
    /// Bearing Origin to Destination
    /// </summary>
    /// <remarks>
    /// Bearing angle of the line, calculated at the origin waypoint, extending to the destination waypoint from 
    /// the origin waypoint for the active navigation leg of the journey
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly",
        MessageId = "Gpbod")]

    public class Bod : NmeaMessage, IAncestorPacket
    {


        /// <summary>
        /// True Bearing from start to destination
        /// </summary>
        public double TrueBearing { get; private set; }

        /// <summary>
        /// Magnetic Bearing from start to destination
        /// </summary>
        public double MagneticBearing { get; private set; }

        /// <summary>
        /// Name of origin
        /// </summary>
        public string OriginId { get; private set; }

        /// <summary>
        /// Name of destination
        /// </summary>
        public string DestinationId { get; private set; }
        [NmeaMessageType("BOD")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, "BOD", typeof(Bod))
            {
            }

            protected override IPacket DecodeCore(List<string> parts)
            {
                var ret = new Bod();
                if (parts == null || parts.Count < 3)
                    throw new ArgumentException("Invalid BOD", "parts");
                ret.TrueBearing = parts[0].Length > 0
                    ? double.Parse(parts[0], CultureInfo.InvariantCulture)
                    : double.NaN;
                ret.MagneticBearing = parts[2].Length > 0
                    ? double.Parse(parts[2], CultureInfo.InvariantCulture)
                    : double.NaN;
                if (parts.Count > 4 && !string.IsNullOrEmpty(parts[4]))
                    ret.DestinationId = parts[4];
                if (parts.Count > 5 && !string.IsNullOrEmpty(parts[5]))
                    ret.OriginId = parts[5];
                return ret;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}