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
    /// Global Positioning System Fix Data
    /// </summary>
    public class Gga : NmeaMessage
    {

        public override string MessageType => "Gga";
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
		/// Fix Quality
		/// </summary>
		public FixQuality Quality { get; private set; }

		/// <summary>
		/// Number of satellites being tracked
		/// </summary>
		public int NumberOfSatellites { get; private set; }

		/// <summary>
		/// Horizontal Dilution of Precision
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Hdop")]
		public double Hdop { get; private set; }

		/// <summary>
		/// Altitude
		/// </summary>
		public double Altitude { get; private set; }

		/// <summary>
		/// Altitude units ('M' for Meters)
		/// </summary>
		public string AltitudeUnits { get; private set; }
	
		/// <summary>
		/// Height of geoid (mean sea level) above WGS84
		/// </summary>
		public double HeightOfGeoid { get; private set; }

		/// <summary>
		/// Altitude units ('M' for Meters)
		/// </summary>
		public string HeightOfGeoidUnits { get; private set; }

		/// <summary>
		/// Time since last DGPS update
		/// </summary>
		public TimeSpan TimeSinceLastDgpsUpdate { get; private set; }

		/// <summary>
		/// DGPS Station ID Number
		/// </summary>
		public int DgpsStationId { get; private set; }

        [NmeaMessageType("GGA")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding)
            {
            }

            protected override IPacket DecodeCore(List<string> parts)
            {
                var ret = new Gga();

                if (parts == null || parts.Count < 14)
                    throw new ArgumentException("Invalid GGA", "parts");
                ret.FixTime = StringToTimeSpan(parts[0]);
                ret.Latitude = StringToLatitude(parts[1], parts[2]);
                ret.Longitude = StringToLongitude(parts[3], parts[4]);
                ret.Quality = (FixQuality)int.Parse(parts[5], CultureInfo.InvariantCulture);
                ret.NumberOfSatellites = int.Parse(parts[6], CultureInfo.InvariantCulture);
                ret.Hdop = StringToDouble(parts[7]);
                ret.Altitude = StringToDouble(parts[8]);
                ret.AltitudeUnits = parts[9];
                ret.HeightOfGeoid = StringToDouble(parts[10]);
                ret.HeightOfGeoidUnits = parts[11];
                var timeInSeconds = StringToDouble(parts[12]);
                ret.TimeSinceLastDgpsUpdate = !double.IsNaN(timeInSeconds) ? TimeSpan.FromSeconds(timeInSeconds) : TimeSpan.MaxValue;
                ret.DgpsStationId = parts[13].Length > 0 ? int.Parse(parts[13], CultureInfo.InvariantCulture) : -1;
                return ret;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().WithAncestorGeneric<string>("GGA",typeof(Gga)).AddDecorate(o => new Encoding(null));
        }
		/// <summary>
		/// Fix quality
		/// </summary>
		public enum FixQuality
        {
            /// <summary>Invalid</summary>
            Invalid = 0,
            /// <summary>GPS</summary>
            GpsFix = 1,
            /// <summary>Differential GPS</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Dgps")]
            DgpsFix = 2,
            /// <summary>Precise Positioning Service</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pps")]
            PpsFix = 3,
            /// <summary>Real Time Kinematic (Fixed)</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rtk")]
            Rtk = 4,
            /// <summary>Real Time Kinematic (doubleing)</summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rtk")]
            DoubleRtk = 5,
            /// <summary>Estimated</summary>
            Estimated = 6,
            /// <summary>Manual input</summary>
            ManualInput = 7,
            /// <summary>Simulation</summary>
            Simulation = 8
        }
    }
}
