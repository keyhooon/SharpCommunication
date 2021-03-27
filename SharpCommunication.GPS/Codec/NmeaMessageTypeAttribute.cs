using System;

namespace SharpCommunication.Codec
{
    /// <summary>
    /// Nmea message attribute type used on concrete <see cref="NmeaMessage"/> implementations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class NmeaMessageTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NmeaMessageTypeAttribute"/> class.
        /// </summary>
        /// <param name="nmeaType">The type.</param>
        public NmeaMessageTypeAttribute(string nmeaType)
        {
            NmeaType = nmeaType;
        }
        /// <summary>
        /// Gets or sets the NMEA message type.
        /// </summary>
        public string NmeaType { get; }
    }
}