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
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;

namespace SharpCommunication.Codec
{
    /// <summary>
    /// NMEA Message base class.
    /// </summary>
    public abstract class NmeaMessage: IPropertyPacket<string>, IAncestorPacket
    {
        public string Property { get; set; }

        /// <summary>
        /// Gets the NMEA message parts.
        /// </summary>
        protected ReadOnlyCollection<string> MessageParts { get; private set; }

        /// <summary>
        /// Gets the NMEA type id for the message.
        /// </summary>
        public abstract string MessageType { get; }

        /// <summary>
        /// Gets the talker ID for this message (
        /// </summary>
        public Talker TalkerId => TalkerHelper.GetTalker(MessageType);

        /// <summary>
        /// Gets a value indicating whether this message type is proprietary
        /// </summary>
        public bool IsProprietary => MessageType[0] == 'P'; //Appendix B

        /// <summary>
        /// Gets the checksum value of the message.
        /// </summary>
        public byte Checksum
        {
            get
            {
                var checksumTest = 0;
                for (var j = -1; j < MessageParts.Count; j++)
                {
                    var message = j < 0 ? MessageType : MessageParts[j];
                    if (j >= 0)
                        checksumTest ^= 0x2C; //Comma separator
                    checksumTest = message.Aggregate(checksumTest, (current, t) => current ^ Convert.ToByte(t));
                }
                return Convert.ToByte(checksumTest);
            }
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{MessageType} {{ {string.Join(",", MessageParts)} }}";
        }


        public abstract class Encoding : EncodingDecorator
        {

            protected Encoding(EncodingDecorator encoding) : base(encoding)
            {
            }
            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                throw new NotSupportedException();
            }

            public override IPacket Decode(BinaryReader reader)
            {
                var parts = new List<string>();
                var ch = reader.ReadChar();
                var sb = new StringBuilder();
                if (ch != ',')
                    throw new FormatException();
                ch = reader.ReadChar();
                while (ch != '\r')
                {

                    if (ch == ',')
                    {
                        parts.Add(sb.ToString());
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(ch);
                    }
                    ch = reader.ReadChar();
                }
                parts.Add(sb.ToString());
                NmeaMessage decodeCore = (NmeaMessage) DecodeCore(parts);
                decodeCore.MessageParts = new ReadOnlyCollection<string>(parts);
                return decodeCore;
            }

            protected abstract IPacket DecodeCore(List<string> parts);


        }


        
		

		internal static double StringToLatitude(string value, string ns)
		{
			if (value == null || value.Length < 3)
				return double.NaN;
			var latitude = int.Parse(value.Substring(0, 2), CultureInfo.InvariantCulture) + double.Parse(value.Substring(2), CultureInfo.InvariantCulture) / 60;
			if (ns == "S")
				latitude *= -1;
			return latitude;
		}

		internal static double StringToLongitude(string value, string ew)
		{
			if (value == null || value.Length < 4)
				return double.NaN;
			var longitude = int.Parse(value.Substring(0, 3), CultureInfo.InvariantCulture) + double.Parse(value.Substring(3), CultureInfo.InvariantCulture) / 60;
			if (ew == "W")
				longitude *= -1;
			return longitude;
		}

		internal static double StringToDouble(string value)
		{
			if(value != null && double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
			{
				return result;
			}
			return double.NaN;
		}
		internal static TimeSpan StringToTimeSpan(string value)
		{
			if (value != null && value.Length >= 6)
			{
				return new TimeSpan(int.Parse(value.Substring(0, 2), CultureInfo.InvariantCulture),
								   int.Parse(value.Substring(2, 2), CultureInfo.InvariantCulture), 0)
								   .Add(TimeSpan.FromSeconds(double.Parse(value.Substring(4), CultureInfo.InvariantCulture)));
			}
			return TimeSpan.Zero;
		}



    }
}
