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
    /// GPS Satellites in view
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gsv")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]

    public class Gsv : NmeaMessage, IMultiPartMessage<SatelliteVehicle>
    {


        /// <summary>
        /// Total number of messages of this type in this cycle
        /// </summary>
        public int TotalMessages { get; private set; }

        /// <summary>
        /// Message number
        /// </summary>
        public int MessageNumber { get; private set; }

        /// <summary>
        /// Total number of SVs in view
        /// </summary>
        public int SVsInView { get; private set; }

        /// <summary>
        /// Satellite vehicles in this message part.
        /// </summary>
        public IReadOnlyList<SatelliteVehicle> SVs { get; private set; }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A System.Collections.Generic.IEnumerator{SatelliteVehicle} that can be used to iterate through the collection.</returns>
        public IEnumerator<SatelliteVehicle> GetEnumerator()
        {
            foreach (var sv in SVs)
                yield return sv;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An System.Collections.IEnumerator object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        [NmeaMessageType("GSV")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, "GSV", typeof(Bod))
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Gsv();
                if (message == null || message.Count < 3)
                    throw new ArgumentException("Invalid GSV", "message");

                ret.TotalMessages = int.Parse(message[0], CultureInfo.InvariantCulture);
                ret.MessageNumber = int.Parse(message[1], CultureInfo.InvariantCulture);
                ret.SVsInView = int.Parse(message[2], CultureInfo.InvariantCulture);

                var svs = new List<SatelliteVehicle>();
                for (var i = 3; i < message.Count - 3; i += 4)
                {
                    if (message[i].Length != 0)

                        svs.Add(new SatelliteVehicle(message.ToArray(), i));
                }
                ret.SVs = svs.ToArray();
                return ret;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
    /// <summary>
    /// Satellite vehicle
    /// </summary>
    public sealed class SatelliteVehicle
    {
        internal SatelliteVehicle(string[] message, int startIndex)
        {
            PrnNumber = int.Parse(message[startIndex], CultureInfo.InvariantCulture);
            if (double.TryParse(message[startIndex + 1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var e))
                Elevation = e;
            if (double.TryParse(message[startIndex + 2], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var a))
                Azimuth = a;
            int snr;
            if (int.TryParse(message[startIndex + 3], out snr))
                SignalToNoiseRatio = snr;
        }
        /// <summary>
        /// SV PRN number
        /// </summary>
        public int PrnNumber { get; }
        /// <summary>
        /// Elevation in degrees, 90 maximum
        /// </summary>
        public double Elevation { get; } = double.NaN;
        /// <summary>
        /// Azimuth, degrees from true north, 000 to 359
        /// </summary>
        public double Azimuth { get; } = double.NaN;
        /// <summary>
        /// Signal-to-Noise ratio, 0-99 dB (-1 when not tracking) 
        /// </summary>
        public int SignalToNoiseRatio { get; }

        /// <summary>
        /// Satellite system
        /// </summary>
        public SatelliteSystem System
        {
            get
            {
                if (PrnNumber >= 1 && PrnNumber <= 32)
                    return SatelliteSystem.Gps;
                if (PrnNumber >= 33 && PrnNumber <= 64)
                    return SatelliteSystem.Waas;
                if (PrnNumber >= 65 && PrnNumber <= 96)
                    return SatelliteSystem.Glonass;
                return SatelliteSystem.Unknown;
            }
        }
    }

    /// <summary>
    /// Satellite system
    /// </summary>
    public enum SatelliteSystem
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown,
        /// <summary>
        /// GPS - Global Positioning System (NAVSTAR)
        /// </summary>
        Gps,
        /// <summary>
        /// WAAS - Wide Area Augmentation System
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Waas")]
        Waas,
        /// <summary>
        /// GLONASS - Globalnaya navigatsionnaya sputnikovaya sistema
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Glonass")]
        Glonass,
        /// <summary>
        /// Galileo
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Galileo")]
        Galileo
    }
}