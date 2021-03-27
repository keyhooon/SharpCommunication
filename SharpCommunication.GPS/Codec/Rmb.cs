﻿//  *******************************************************************************
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
    /// Recommended minimum navigation information
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gprmb")]

    public class Rmb : NmeaMessage
    {
        /// <summary>
        /// Data status
        /// </summary>
        public enum DataStatus
        {
            /// <summary>
            /// Ok
            /// </summary>
            Ok,
            /// <summary>
            /// Warning
            /// </summary>
            Warning
        }

        /// <summary>
        /// Data Status
        /// </summary>
        public DataStatus Status { get; private set; }

        /// <summary>
        /// Cross-track error (steer left when negative, right when positive)
        /// </summary>
        public double CrossTrackError { get; private set; }

        /// <summary>
        /// Origin waypoint ID
        /// </summary>
        public double OriginWaypointId { get; private set; }

        /// <summary>
        /// Destination waypoint ID
        /// </summary>
        public double DestinationWaypointId { get; private set; }

        /// <summary>
        /// Destination Latitude
        /// </summary>
        public double DestinationLatitude { get; private set; }

        /// <summary>
        /// Destination Longitude
        /// </summary>
        public double DestinationLongitude { get; private set; }

        /// <summary>
        /// Range to destination in nautical miles
        /// </summary>
        public double RangeToDestination { get; private set; }

        /// <summary>
        /// True bearing to destination
        /// </summary>
        public double TrueBearing { get; private set; }

        /// <summary>
        /// Velocity towards destination in knots
        /// </summary>
        public double Velocity { get; private set; }

        /// <summary>
        /// Arrived (<c>true</c> if arrived)
        /// </summary>
        public bool Arrived { get; private set; }
        [NmeaMessageType("RMB")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, "RMB", typeof(Bod))
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Rmb();
                if (message == null || message.Count < 13)
                    throw new ArgumentException("Invalid RMB", "message");

                ret.Status = message[0] == "A" ? DataStatus.Ok : DataStatus.Warning;
                if (double.TryParse(message[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var tmp))
                {
                    ret.CrossTrackError = tmp;

                    if (message[2] == "L") //Steer left
                        ret.CrossTrackError *= -1;
                }
                else
                    ret.CrossTrackError = double.NaN;

                if (message[3].Length > 0)
                    ret.OriginWaypointId = int.Parse(message[3], CultureInfo.InvariantCulture);
                if (message[4].Length > 0)
                    ret.DestinationWaypointId = int.Parse(message[4], CultureInfo.InvariantCulture);
                ret.DestinationLatitude = StringToLatitude(message[5], message[6]);
                ret.DestinationLongitude = StringToLongitude(message[7], message[8]);
                ret.RangeToDestination = double.TryParse(message[9], NumberStyles.Float, CultureInfo.InvariantCulture, out tmp) ? tmp : double.NaN;
                ret.TrueBearing = double.TryParse(message[10], NumberStyles.Float, CultureInfo.InvariantCulture, out tmp) ? tmp : double.NaN;
                ret.Velocity = double.TryParse(message[11], NumberStyles.Float, CultureInfo.InvariantCulture, out tmp) ? tmp : double.NaN;
                ret.Arrived = message[12] == "A";
                return ret;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
