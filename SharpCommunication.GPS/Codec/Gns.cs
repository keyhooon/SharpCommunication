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
using System.Linq;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec
{
    /// <summary>
    /// Fixes data for single or combined (GPS, GLONASS, possible future satellite systems, and systems combining these) satellite navigation systems
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gns")]

    public class Gns : NmeaMessage
    {
        public override string MessageType => "Gns";
        /*
         * Example of GNS messages:
         * $GNGNS,014035.00,4332.69262,S,17235.48549,E,RR,13,0.9,25.63,11.24,,*70   //GLONASS
         * $GPGNS,014035.00,,,,,,8,,,,1.0,23*76                                     //GPS
         * $GLGNS,014035.00,,,,,,5,,,,1.0,23*67                                     //GALILEO
         */

        /// <summary>
        /// GNS Mode Indicator
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// No fix. Satellite system not used in position fix, or fix not valid
            /// </summary>
            NoFix,
            /// <summary>
            /// Autonomous. Satellite system used in non-differential mode in position fix
            /// </summary>
            Autonomous,
            /// <summary>
            /// Differential (including all OmniSTAR services). Satellite system used in differential mode in position fix
            /// </summary>
            Differential,
            /// <summary>
            /// Precise. Satellite system used in precision mode. Precision mode is defined as no deliberate degradation (such as Selective Availability) and higher resolution code (P-code) is used to compute position fix.
            /// </summary>
            Precise,
            /// <summary>
            ///  Real Time Kinematic. Satellite system used in RTK mode with fixed integers
            /// </summary>
            RealTimeKinematic,
            /// <summary>
            /// double RTK. Satellite system used in real time kinematic mode with doubleing integers
            /// </summary>
            DoubleRtk,
            /// <summary>
            /// Estimated (dead reckoning) mode
            /// </summary>
            Estimated,
            /// <summary>
            /// Manual input mode
            /// </summary>
            Manual,
            /// <summary>
            /// Simulator mode
            /// </summary>
            Simulator
        }

        /// <summary>
        /// Navigational status
        /// </summary>
        public enum NavigationalStatus
        {
            /// <summary>
            /// Not valid for navigation
            /// </summary>
            NotValid,
            /// <summary>
            /// Safe
            /// </summary>
            Safe,
            /// <summary>
            /// Caution
            /// </summary>
            Caution,
            /// <summary>
            /// Unsafe
            /// </summary>
            Unsafe
        }

        private static Mode ParseModeIndicator(char c)
        {
            return c switch
            {
                'A' => Mode.Autonomous,
                'D' => Mode.Differential,
                'P' => Mode.Precise,
                'R' => Mode.RealTimeKinematic,
                'F' => Mode.DoubleRtk,
                'E' => Mode.Estimated,
                'M' => Mode.Manual,
                'S' => Mode.Simulator,
                _ => Mode.NoFix
            };
        }

		/// <summary>
		/// Time of day fix was taken
		/// </summary>
		public TimeSpan FixTime { get; private set; }
		
		/// <summary>
		/// Latitude
		/// </summary>
		public double Latitude { get; private set; }

		/// <summary>
		/// Longitude
		/// </summary>
		public double Longitude { get; private set; }

        /// <summary>
        /// Mode indicator for GPS
        /// </summary>
        /// <seealso cref="GlonassModeIndicator"/>
        /// <see cref="FutureModeIndicator"/>
        public Mode GpsModeIndicator { get; private set; }

        /// <summary>
        /// Mode indicator for GLONASS
        /// </summary>
        /// <seealso cref="GpsModeIndicator"/>
        /// <see cref="FutureModeIndicator"/>
        public Mode GlonassModeIndicator { get; private set; }

        /// <summary>
        /// Mode indicator for future constallations
        /// </summary>
        /// <seealso cref="GlonassModeIndicator"/>
        /// <seealso cref="GpsModeIndicator"/>
        public Mode[] FutureModeIndicator { get; private set; }

        /// <summary>
        /// Number of satellites (SVs) in use
        /// </summary>
        public int NumberOfSatellites { get; private set; }

        /// <summary>
        /// Horizontal Dilution of Precision (HDOP), calculated using all the satellites (GPS, GLONASS, and any future satellites) used in computing the solution reported in each GNS sentence.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hdop")]
		public double Hdop { get; private set; }

        /// <summary>
        /// Orthometric height in meters (MSL reference)
        /// </summary>
        public double OrhometricHeight { get; private set; }

        /// <summary>
        /// Geoidal separation in meters - the difference between the earth ellipsoid surface and mean-sea-level (geoid) surface defined by the reference datum used in the position solution<br/>
        /// '-' = mean-sea-level surface below ellipsoid.
        /// </summary>
        public double GeoidalSeparation { get; private set; }

        /// <summary>
        ///  Age of differential data - <see cref="TimeSpan.MaxValue"/> if talker ID is GN, additional GNS messages follow with GP and/or GL Age of differential data
		/// </summary>
		public TimeSpan TimeSinceLastDgpsUpdate { get; private set; }

		/// <summary>
		/// eference station ID1, range 0000-4095 - Null if talker ID is GN, additional GNS messages follow with GP and/or GL Reference station ID
        /// </summary>
        public int DgpsStationId { get; private set; }

        /// <summary>
        /// Navigational status
        /// </summary>
        public NavigationalStatus Status { get; private set; }
        [NmeaMessageType("GNS")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding)
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Gns();

                if (message == null || message.Count < 12)
                    throw new ArgumentException("Invalid GNS", "message");
                ret.FixTime = StringToTimeSpan(message[0]);
                ret.Latitude = StringToLatitude(message[1], message[2]);
                ret.Longitude = StringToLongitude(message[3], message[4]);
                if (message[5].Length > 0)
                    ret.GpsModeIndicator = ParseModeIndicator(message[5][0]);
                if (message[5].Length > 1)
                    ret.GlonassModeIndicator = ParseModeIndicator(message[5][1]);
                ret.FutureModeIndicator = message[5].Length > 2 ? message[5].Skip(2).Select(ParseModeIndicator).ToArray() : new Mode[] { };
                ret.NumberOfSatellites = int.Parse(message[6], CultureInfo.InvariantCulture);
                ret.Hdop = StringToDouble(message[7]);
                ret.OrhometricHeight = StringToDouble(message[8]);
                ret.GeoidalSeparation = StringToDouble(message[9]);
                var timeInSeconds = StringToDouble(message[10]);
                ret.TimeSinceLastDgpsUpdate = !double.IsNaN(timeInSeconds) ? TimeSpan.FromSeconds(timeInSeconds) : TimeSpan.MaxValue;
                ret.DgpsStationId = message[11].Length > 0 ? int.Parse(message[1], CultureInfo.InvariantCulture) : -1;

                if (message.Count <= 12) return ret;
                ret.Status = message[12] switch
                {
                    "S" => NavigationalStatus.Safe,
                    "C" => NavigationalStatus.Caution,
                    "U" => NavigationalStatus.Unsafe,
                    "V" => NavigationalStatus.NotValid,
                    _ => NavigationalStatus.NotValid
                };
                return ret;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().WithAncestorGeneric<string>("GNS", typeof(Gns)).AddDecorate(o => new Encoding(null));

        }
    }
}
