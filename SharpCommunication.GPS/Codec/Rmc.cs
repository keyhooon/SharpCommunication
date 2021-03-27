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
    /// Recommended Minimum
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gprmc")]

    public class Rmc : NmeaMessage
	{

		/// <summary>
		/// Fix Time
		/// </summary>
		public DateTime FixTime { get; private set; }

		/// <summary>
		/// Gets a value whether the device is active
		/// </summary>
		public bool Active { get; private set; }

		/// <summary>
		/// Latitude
		/// </summary>
		public double Latitude { get; private set; }

		/// <summary>
		/// Longitude
		/// </summary>
		public double Longitude { get; private set; }

		/// <summary>
		/// Speed over the ground in knots
		/// </summary>
		public double Speed { get; private set; }

		/// <summary>
		/// Track angle in degrees True
		/// </summary>
		public double Course { get; private set; }

		/// <summary>
		/// Magnetic Variation
		/// </summary>
		public double MagneticVariation { get; private set; }
        [NmeaMessageType("RMC")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, "RMC", typeof(Bod))
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Rmc();
                if (message == null || message.Count < 11)
                    throw new ArgumentException("Invalid RMC", "message");

                if (message[8].Length == 6 && message[0].Length >= 6)
                {
                    ret.FixTime = new DateTime(int.Parse(message[8].Substring(4, 2), CultureInfo.InvariantCulture) + 2000,
                        int.Parse(message[8].Substring(2, 2), CultureInfo.InvariantCulture),
                        int.Parse(message[8].Substring(0, 2), CultureInfo.InvariantCulture),
                        int.Parse(message[0].Substring(0, 2), CultureInfo.InvariantCulture),
                        int.Parse(message[0].Substring(2, 2), CultureInfo.InvariantCulture),
                        0, DateTimeKind.Utc).AddSeconds(double.Parse(message[0].Substring(4), CultureInfo.InvariantCulture));
                }
                ret.Active = (message[1] == "A");
                ret.Latitude = StringToLatitude(message[2], message[3]);
                ret.Longitude = StringToLongitude(message[4], message[5]);
                ret.Speed = StringToDouble(message[6]);
                ret.Course = StringToDouble(message[7]);
                ret.MagneticVariation = StringToDouble(message[9]);
                if (!double.IsNaN(ret.MagneticVariation) && message[10] == "W")
                    ret.MagneticVariation *= -1;
				return ret;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
	}
}
