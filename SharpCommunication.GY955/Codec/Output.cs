using SharpCommunication.Codec.Encoding;
using SharpCommunication.Codec.Packets;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SharpCommunication.GY955.Codec
{
    public class Output : IPacket
    {
        public VectorDataType Types { get; set; }
        public Vector3? Acc { get; set; }
        public Vector3? Mag { get; set; }
        public Vector3? Gyr { get; set; }
        public Vector3? Yrp { get; set; }
        public Vector4? Q4 { get; set; }
        public Level SystemAccuracy { get; set; }
        public Level MagAccuracy { get; set; }
        public Level AccAccuracy { get; set; }
        public Level GyrAccuracy { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Types.HasFlag(VectorDataType.Accelerator) && Acc != null)
                sb.Append($"Accelerator {{ X : {Acc.Value.X}, Y : {Acc.Value.Y}, Z : {Acc.Value.Z}, ");
            if (Types.HasFlag(VectorDataType.Gyroscope) && Gyr != null)
                sb.Append($"Gyroscope {{ X : {Gyr.Value.X}, Y : {Gyr.Value.Y}, Z : {Gyr.Value.Z}, ");
            if (Types.HasFlag(VectorDataType.Magnetometer) && Mag != null)
                sb.Append($"Magnetometer {{ X : {Mag.Value.X}, Y : {Mag.Value.Y}, Z : {Mag.Value.Z}, ");
            if (Types.HasFlag(VectorDataType.Euler) && Yrp != null)
                sb.Append($"Euler {{ X : {Yrp.Value.X}, Y : {Yrp.Value.Y}, Z : {Yrp.Value.Z}, ");
            if (Types.HasFlag(VectorDataType.Quaternion) && Q4 != null)
                sb.Append($"Quaternion {{ X : {Q4.Value.X}, Y : {Q4.Value.Y}, Z : {Q4.Value.Z}, ");
            return sb.ToString();

        }

        public class Encoding : HeaderPacketEncoding
        {
            public Encoding(EncodingDecorator encoding) : base(encoding, new byte[] { 0x5A, 0x5A })
            {

            }

            public override IPacket Decode(BinaryReader reader)
            {
                var result = new Output
                {
                    Types = (VectorDataType)reader.ReadByte()
                };
                var dataVolume = reader.ReadByte();
                var data = reader.ReadBytes(dataVolume);
                var accuracy = data[dataVolume - 1];
                var crc8 = data.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                if (crc8 != reader.ReadByte())
                    throw new AggregateException();
                var index = 0;
                if (result.Types.HasFlag(VectorDataType.Accelerator))
                {
                    result.Acc = new Vector3(
                        BitConverter.ToInt16(data, index),
                        BitConverter.ToInt16(data, index + 2),
                        BitConverter.ToInt16(data, index + 4));
                    result.AccAccuracy = (Level)(accuracy & 0x03); 
                    index += 6;
                    
                }
                if (result.Types.HasFlag(VectorDataType.Magnetometer))
                {
                    result.Mag = new Vector3(
                        BitConverter.ToInt16(data, index),
                        BitConverter.ToInt16(data, index + 2),
                        BitConverter.ToInt16(data, index + 4));
                    result.MagAccuracy = (Level)((accuracy & 0x0C)>>2);
                    index += 6;
                }
                if (result.Types.HasFlag(VectorDataType.Gyroscope))
                {
                    result.Gyr = new Vector3(
                        BitConverter.ToInt16(data, index),
                        BitConverter.ToInt16(data, index + 2),
                        BitConverter.ToInt16(data, index + 4));
                    result.GyrAccuracy = (Level)((accuracy & 0x30)>>4);
                    index += 6;
                }
                if (result.Types.HasFlag(VectorDataType.Euler))
                {
                    result.Yrp = new Vector3(
                        BitConverter.ToInt16(data, index),
                        BitConverter.ToInt16(data, index + 2),
                        BitConverter.ToInt16(data, index + 4));
                    index += 6;
                }
                if (result.Types.HasFlag(VectorDataType.Quaternion))
                {
                    result.Q4 = new Vector4(
                        BitConverter.ToInt16(data, index),
                        BitConverter.ToInt16(data, index + 2),
                        BitConverter.ToInt16(data, index + 4),
                        BitConverter.ToInt16(data, index + 6));
                    index += 8;
                }
                result.SystemAccuracy = (Level)((accuracy & 0xC0) >> 6);
                return result;
            }

            public override void Encode(IPacket packet, BinaryWriter writer)
            {
                var output = (Output)packet;
                byte dataVolume = 1;
                var index = 0;
                byte crc8 = 0;
                byte Accuracy = 0;
                writer.Write((byte)output.Types);
                if (output.Types.HasFlag(VectorDataType.Accelerator)) dataVolume += 6;
                if (output.Types.HasFlag(VectorDataType.Magnetometer)) dataVolume += 6;
                if (output.Types.HasFlag(VectorDataType.Gyroscope)) dataVolume += 6;
                if (output.Types.HasFlag(VectorDataType.Euler)) dataVolume += 6;
                if (output.Types.HasFlag(VectorDataType.Quaternion)) dataVolume += 8;
                writer.Write(dataVolume);

                var bytes = new byte[dataVolume] ;
                if (output.Types.HasFlag(VectorDataType.Accelerator))
                {
                    BitConverter.GetBytes((short)output.Acc.Value.X).CopyTo(bytes, index);
                    BitConverter.GetBytes((short)output.Acc.Value.Y).CopyTo(bytes, index + 2);
                    BitConverter.GetBytes((short)output.Acc.Value.Z).CopyTo(bytes, index + 4);
                    Accuracy |= (byte)output.AccAccuracy;
                    index += 6;
                    
                }
                if (output.Types.HasFlag(VectorDataType.Magnetometer))
                {
                    BitConverter.GetBytes((short)output.Mag.Value.X).CopyTo(bytes, index);
                    BitConverter.GetBytes((short)output.Mag.Value.Y).CopyTo(bytes, index + 2);
                    BitConverter.GetBytes((short)output.Mag.Value.Z).CopyTo(bytes, index + 4);
                    Accuracy |= (byte)(((byte)output.MagAccuracy)<<2);
                    index += 6;
                }
                if (output.Types.HasFlag(VectorDataType.Gyroscope))
                {
                    BitConverter.GetBytes((short)output.Gyr.Value.X).CopyTo(bytes, index);
                    BitConverter.GetBytes((short)output.Gyr.Value.Y).CopyTo(bytes, index + 2);
                    BitConverter.GetBytes((short)output.Gyr.Value.Z).CopyTo(bytes, index + 4);
                    Accuracy |= (byte)(((byte)output.GyrAccuracy) << 4);
                    index += 6;
                }
                if (output.Types.HasFlag(VectorDataType.Euler))
                {
                    BitConverter.GetBytes((short)output.Yrp.Value.X).CopyTo(bytes, index);
                    BitConverter.GetBytes((short)output.Yrp.Value.Y).CopyTo(bytes, index + 2);
                    BitConverter.GetBytes((short)output.Yrp.Value.Z).CopyTo(bytes, index + 4);
                    index += 6;
                }
                if (output.Types.HasFlag(VectorDataType.Quaternion))
                {
                    BitConverter.GetBytes((short)output.Q4.Value.X).CopyTo(bytes, index);
                    BitConverter.GetBytes((short)output.Q4.Value.Y).CopyTo(bytes, index + 2);
                    BitConverter.GetBytes((short)output.Q4.Value.Z).CopyTo(bytes, index + 4);
                    BitConverter.GetBytes((short)output.Q4.Value.W).CopyTo(bytes, index + 6);
                    index += 8;
                }
                if (index != dataVolume - 1)
                    throw new AggregateException();
                Accuracy |= (byte)(((byte)output.SystemAccuracy) << 6);
                bytes[index] = Accuracy;
                writer.Write(bytes);
                crc8 = bytes.Aggregate<byte, byte>(0, (current, t) => (byte)(current + t));
                writer.Write(crc8);
            }
        }

    }
    public enum Level
    {
        UnderLow = 0,
        Low = 1,
        Medium = 2,
        High = 3,      
    }
    [Flags]
    public enum VectorDataType : byte
    {
        Accelerator,
        Magnetometer,
        Gyroscope,
        Euler,
        Quaternion,
    }


}
