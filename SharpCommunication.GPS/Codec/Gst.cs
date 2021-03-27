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
    /// Position error statistics
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Gpgst")]

    public class Gst : NmeaMessage
    {
        /// <summary>
        /// UTC of position fix
        /// </summary>
        public TimeSpan FixTime { get; private set; }

        /// <summary>
        /// RMS value of the pseudo range residuals; includes carrier phase residuals during periods of RTK (double) and RTK (fixed) processing
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rms")]
        public double Rms { get; private set; }

        /// <summary>
        /// Error ellipse semi-major axis 1 sigma error, in meters
        /// </summary>
        public double SemiMajorError { get; private set; }

        /// <summary>
        /// Error ellipse semi-minor axis 1 sigma error, in meters
        /// </summary>
        public double SemiMinorError { get; private set; }

        /// <summary>
        /// Error ellipse orientation, degrees from true north
        /// </summary>
        public double ErrorOrientation { get; private set; }

        /// <summary>
        /// Latitude 1 sigma error, in meters
        /// </summary>
        /// <remarks>
        /// The error expressed as one standard deviation.
        /// </remarks>
        public double SigmaLatitudeError { get; private set; }

        /// <summary >
        /// Longitude 1 sigma error, in meters
        /// </summary>
        /// <remarks>
        /// The error expressed as one standard deviation.
        /// </remarks>
        public double SigmaLongitudeError { get; private set; }

        /// <summary >
        /// Height 1 sigma error, in meters
        /// </summary>
        /// <remarks>
        /// The error expressed as one standard deviation.
        /// </remarks>
        public double SigmaHeightError { get; private set; }
        [NmeaMessageType("GST")]
        public new class Encoding : NmeaMessage.Encoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, "GST", typeof(Bod))
            {
            }

            protected override IPacket DecodeCore(List<string> message)
            {
                var ret = new Gst();
                if (message == null || message.Count < 8)
                    throw new ArgumentException("Invalid GST", "message");
                ret.FixTime = StringToTimeSpan(message[0]);
                ret.Rms = StringToDouble(message[1]);
                ret.SemiMajorError = StringToDouble(message[2]);
                ret.SemiMinorError = StringToDouble(message[3]);
                ret.ErrorOrientation = StringToDouble(message[4]);
                ret.SigmaLatitudeError = StringToDouble(message[5]);
                ret.SigmaLongitudeError = StringToDouble(message[6]);
                ret.SigmaHeightError = StringToDouble(message[7]);
                return ret;
            }

            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(item => new Encoding(item));
        }
    }
}
