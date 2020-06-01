﻿using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;
using System.Linq;

namespace Demo.Codec
{
    public class Fault : IPacket, IAncestorPacket
    {

        public bool OverCurrent { get; set; }
        public bool OverTemprature { get; set; }
        public bool PedalSensor { get; set; }
        public bool Throttle { get; set; }
        public bool OverVoltage { get; set; }
        public bool UnderVoltage { get; set; }
        public bool Motor { get; set; }
        public bool Drive { get; set; }

        public override string ToString()
        {

            return $"OverCurrent : {OverCurrent}, " +
                $"OverTemprature : {OverTemprature}, " +
                $"PedalSensor : {PedalSensor}, " +
                $"Throttle : {Throttle}, " +
                $"OverVoltage : {OverVoltage}, " +
                $"UnderVoltage : {UnderVoltage}, " +
                $"Motor : {Motor}, " +
                $"Drive : {Drive}, ";
        }
        public class Encoding : AncestorPacketEncoding<Fault>
        {

            private readonly static byte byteCount = 2;
            public new const byte Id = 100;
            public Encoding(PacketEncoding encoding) : base(encoding, Id)
            {

            }
            public Encoding() : base(null, Id)
            {

            }

            public override void EncodeCore(IPacket packet, BinaryWriter writer)
            {
                var o = (Fault)packet;
                var value = BitConverter.GetBytes(
                    (o.OverTemprature ? 0x02 : 0x00) | (o.OverCurrent ? 0x01 : 0x00) |
                    (o.Throttle ? 0x08 : 0x00) | (o.PedalSensor ? 0x04 : 0x00) |
                    (o.UnderVoltage ? 0x20 : 0x00) | (o.OverVoltage ? 0x10 : 0x00) |
                    (o.Drive ? 0x80 : 0x00) | (o.Motor ? 0x40 : 0x00));
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                writer.Write(value);
                writer.Write(crc8);
            }

            public override IPacket DecodeCore(BinaryReader reader)
            {
                var value = reader.ReadBytes(byteCount);
                var crc8 = value.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                if (crc8 == reader.ReadByte())
                    return new Fault
                    {
                        OverCurrent = (value[0] & 0x01) == 0x01,
                        OverTemprature = (value[0] & 0x02) == 0x02,
                        PedalSensor = (value[0] & 0x04) == 0x04,
                        Throttle = (value[0] & 0x08) == 0x08,
                        OverVoltage = (value[0] & 0x10) == 0x10,
                        UnderVoltage = (value[0] & 0x20) == 0x20,
                        Motor = (value[0] & 0x40) == 0x40,
                        Drive = (value[0] & 0x80) == 0x80
                    };
                return null;
            }
            public static PacketEncodingBuilder CreateBuilder() =>
                PacketEncodingBuilder.CreateDefaultBuilder().AddDecorate(o => new Encoding(o));
        }

    }
}