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
	/// Course over ground and ground speed
	/// </summary>
    /// <remarks>
    /// The actual course and speed relative to the ground.
    /// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "GPVTG")]

    public class Vtg : NmeaMessage
    {

        public override string MessageType => "Vtg";
        /// <summary>
        ///  Course over ground relative to true north
        /// </summary>
        public double TrueCourseOverGround { get; private set; }

        /// <summary>
        ///  Course over ground relative to magnetic north
        /// </summary>
        public double MagneticCourseOverGround { get; private set; }

        /// <summary>
        /// Speed over ground in knots
        /// </summary>
        public double SpeedInKnots { get; private set; }

        /// <summary>
        /// Speed over ground in kilometers/hour
        /// </summary>
        public double SpeedInKph { get; private set; }
        [NmeaMessageType("VTG")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding)
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Vtg();;
                if (message == null || message.Count < 7)
                    throw new ArgumentException("Invalid VTG", "message");
                ret.TrueCourseOverGround = StringToDouble(message[0]);
                ret.MagneticCourseOverGround = StringToDouble(message[2]);
                ret.SpeedInKnots = StringToDouble(message[4]);
                ret.SpeedInKph = StringToDouble(message[6]);
                return ret;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().WithAncestorGeneric<string>("VTG", typeof(Vtg)).AddDecorate(o => new Encoding(null));

        }
    }
}