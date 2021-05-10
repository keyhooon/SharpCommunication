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
        public Vector3 Euler { get; set; }
        public Vector4 Quaternion { get; set; }
        public Level SystemAccuracy { get; set; }

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
