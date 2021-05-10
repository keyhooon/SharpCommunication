using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace SharpCommunication.GY955.Codec
{
    public class Output
    {
        public VectorDataType Types { get; set; }
        public AccuratedVector Accelerator { get; set; }
        public AccuratedVector Magnetometer { get; set; }
        public AccuratedVector Gyroscope { get; set; }
        public Vector3? Euler { get; set; }
        public Vector4? Quaternion { get; set; }
        public Level SystemAccuracy { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Types.HasFlag(VectorDataType.Accelerator) && Accelerator != null)
                sb.Append($"Accelerator {{ X : {Accelerator.Value.X}, Y : {Accelerator.Value.Y}, Z : {Accelerator.Value.Z}, ");
            if (Types.HasFlag(VectorDataType.Gyroscope) && Gyroscope != null)
                sb.Append($"Gyroscope {{ X : {Gyroscope.Value.X}, Y : {Gyroscope.Value.Y}, Z : {Gyroscope.Value.Z}, ");
            if (Types.HasFlag(VectorDataType.Magnetometer) && Magnetometer != null)
                sb.Append($"Magnetometer {{ X : {Magnetometer.Value.X}, Y : {Magnetometer.Value.Y}, Z : {Magnetometer.Value.Z}, ");
            if (Types.HasFlag(VectorDataType.Euler) && Euler != null)
                sb.Append($"Euler {{ X : {Euler.Value.X}, Y : {Euler.Value.Y}, Z : {Euler.Value.Z}, ");
            if (Types.HasFlag(VectorDataType.Quaternion) && Quaternion != null)
                sb.Append($"Quaternion {{ X : {Quaternion.Value.X}, Y : {Quaternion.Value.Y}, Z : {Quaternion.Value.Z}, ");
            return sb.ToString();

        }

    }

    

    public class AccuratedVector
    {
        public Level Accuracy { get; set; }

        public Vector3 Value { get; set; }


    }
    public enum Level
    {
        High,
        Medium,
        Low,
        UnderLow,
    }
    [Flags]
    public enum VectorDataType
    {
        Accelerator,
        Magnetometer,
        Gyroscope,
        Euler,
        Quaternion,
    }


}
