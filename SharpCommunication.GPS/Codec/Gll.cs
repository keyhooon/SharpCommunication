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
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec
{
    /// <summary>
    /// Geographic position, latitude / longitude
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gll")]
    public class Gll : NmeaMessage
	{


		/// <summary>
		/// Latitude
		/// </summary>
		public double Latitude { get; private set; }

		/// <summary>
		/// Longitude
		/// </summary>
		public double Longitude { get; private set; }

		/// <summary>
		/// Time since last DGPS update
		/// </summary>
		public TimeSpan FixTime { get; private set; }

		/// <summary>
		/// Gets a value indicating whether data is active.
		/// </summary>
		/// <value>
		///   <c>true</c> if data is active; otherwise, <c>false</c>.
		/// </value>
		public bool DataActive { get; private set; }

        /// <summary>
        /// Positioning system Mode Indicator
        /// </summary>
        public Mode ModeIndicator { get; private set; }

        [NmeaMessageType("GLL")]

        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, "GLL", typeof(Bod))
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Gll();
                if (message == null || message.Count < 4)
                    throw new ArgumentException("Invalid GLL", "message");
                ret.Latitude = StringToLatitude(message[0], message[1]);
                ret.Longitude = StringToLongitude(message[2], message[3]);
                if (message.Count >= 5) //Some older GPS doesn't broadcast fix time
                {
                    ret.FixTime = StringToTimeSpan(message[4]);
                }
                ret.DataActive = (message.Count < 6 || message[5] == "A");
                ret.ModeIndicator = ret.DataActive ? Mode.Autonomous : Mode.DataNotValid;
                if (message.Count > 6)
                {
                    ret.ModeIndicator = message[6] switch
                    {
                        "A" => Mode.Autonomous,
                        "D" => Mode.DataNotValid,
                        "E" => Mode.EstimatedDeadReckoning,
                        "M" => Mode.Manual,
                        "S" => Mode.Simulator,
                        "N" => Mode.DataNotValid,
                        _ => ret.ModeIndicator
                    };
                }

                return ret;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
        /// <summary>
        /// Positioning system Mode Indicator
        /// </summary>
        /// <seealso cref="Gll.ModeIndicator"/>
        public enum Mode
        {
            /// <summary>
            /// Autonomous mode
            /// </summary>
            Autonomous,
            /// <summary>
            ///  Differential mode
            /// </summary>
            Differential,
            /// <summary>
            ///  Estimated (dead reckoning) mode
            /// </summary>
            EstimatedDeadReckoning,
            /// <summary>
            /// Manual input mode
            /// </summary>
            Manual,
            /// <summary>
            /// Simulator mode
            /// </summary>
            Simulator,
            /// <summary>
            /// Data not valid
            /// </summary>
            DataNotValid
        }
	}
}
