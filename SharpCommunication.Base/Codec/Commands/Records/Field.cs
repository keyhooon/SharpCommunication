using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpCommunication.Base.Codec.Commands.Records
{
    public enum FieldFormat : byte
    {
        Number,
        Buffer,
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Field : IEquatable<Field>
    {
        [FieldOffset(0)] private readonly byte _id;
        [FieldOffset(1)] private readonly byte _size;
        [FieldOffset(2)] private readonly FieldFormat _format;
        [FieldOffset(3)] private unsafe fixed byte _buffer[8];
        [FieldOffset(3)] private readonly long _value;

        public static readonly Field Default;

        public bool IsDefault
        {
            get
            {
                if (_id == 0 && _size == 0 && _format == FieldFormat.Number)
                    return _value == 0L;
                return false;
            }
        }

        public int Id => _id;

        public int Size => _size;

        public FieldFormat Format => _format;


        private Field(byte id, byte size, long value)
        {
            _id = id;
            _size = size;
            _format = FieldFormat.Number;
            _value = value;
        }

        private unsafe Field(byte id, byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (8 < buffer.Length)
                throw new ArgumentException("The buffer size exceedes maximum buffer size of 8.");
            _id = id;
            _value = 0L;
            _format = FieldFormat.Buffer;
            _size = (byte)buffer.Length;

            // ISSUE: reference to a compiler-generated field
            fixed (byte* numPtr = _buffer)
                Marshal.Copy(buffer, 0, new IntPtr(numPtr), buffer.Length);
        }

        public static explicit operator bool(Field data)
        {
            return data._value != 0L;
        }

        public static explicit operator sbyte(Field data)
        {
            return (sbyte)data._value;
        }

        public static explicit operator byte(Field data)
        {
            return (byte)data._value;
        }

        public static explicit operator short(Field data)
        {
            return (short)data._value;
        }

        public static explicit operator ushort(Field data)
        {
            return (ushort)data._value;
        }

        public static explicit operator int(Field data)
        {
            return (int)data._value;
        }

        public static explicit operator uint(Field data)
        {
            return (uint)data._value;
        }

        public static explicit operator long(Field data)
        {
            return data._value;
        }

        public static explicit operator ulong(Field data)
        {
            return (ulong)data._value;
        }

        public static unsafe explicit operator byte[](Field data)
        {
            var destination = new byte[data._size];

            // ISSUE: reference to a compiler-generated field
            Marshal.Copy(new IntPtr(data._buffer), destination, 0, destination.Length);
            return destination;
        }

        public static bool operator ==(Field left, Field right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Field left, Field right)
        {
            return !left.Equals(right);
        }

        public static Field Create(byte id, sbyte value)
        {
            return new Field(id, 1, value);
        }

        public static Field Create(byte id, byte value)
        {
            return new Field(id, 1, value);
        }

        public static Field Create(byte id, short value)
        {
            return new Field(id, 2, value);
        }

        public static Field Create(byte id, ushort value)
        {
            return new Field(id, 2, value);
        }

        public static Field Create(byte id, int value)
        {
            return new Field(id, 4, value);
        }

        public static Field Create(byte id, uint value)
        {
            return new Field(id, 4, value);
        }

        public static Field Create(byte id, long value)
        {
            return new Field(id, 8, value);
        }

        public static Field Create(byte id, ulong value)
        {
            return new Field(id, 8, (long)value);
        }


        public static Field Create(byte id, byte[] value)
        {
            return new Field(id, value);
        }

        public unsafe string ToHexString()
        {
            // ISSUE: reference to a compiler-generated field
            fixed (byte* numPtr = _buffer)
            {
                if ((IntPtr)numPtr == IntPtr.Zero)
                    return string.Empty;
                var size = _size;
                var capacity = size << 1;
                var stringBuilder = new StringBuilder(capacity)
                {
                    Length = capacity
                };
                var index1 = 0;
                var index2 = capacity - 2;
                while (index1 < size)
                {
                    var num = numPtr[index1];
                    var ch1 = EncodingHelper.Alphabet[num >> 4];
                    var ch2 = EncodingHelper.Alphabet[num & 15];
                    stringBuilder[index2] = ch1;
                    stringBuilder[index2 + 1] = ch2;
                    ++index1;
                    index2 -= 2;
                }

                return stringBuilder.ToString();
            }
        }

        public bool ToBoolean()
        {
            return _value != 0L;
        }

        public sbyte ToSByte()
        {
            return (sbyte)_value;
        }

        public byte ToByte()
        {
            return (byte)_value;
        }

        public short ToInt16()
        {
            return (short)_value;
        }

        public ushort ToUInt16()
        {
            return (ushort)_value;
        }

        public int ToInt32()
        {
            return (int)_value;
        }

        public uint ToUInt32()
        {
            return (uint)_value;
        }

        public long ToInt64()
        {
            return _value;
        }

        public ulong ToUInt64()
        {
            return (ulong)_value;
        }

        public unsafe byte[] ToBuffer()
        {
            var destination = new byte[_size];

            fixed (byte* numPtr = _buffer)
                Marshal.Copy(new IntPtr(numPtr), destination, 0, destination.Length);
            return destination;
        }

        public override bool Equals(object obj)
        {
            return obj is Field property && Equals(property);
        }

        public bool Equals(Field other)
        {
            if (_id == other._id && _size == other._size && _format == other._format)
                return _value == other._value;
            return false;
        }

        public override int GetHashCode()
        {
            return (int)((FieldFormat)((_id * 397 ^ _size.GetHashCode()) * 397) ^ _format) * 397 ^ _value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id}={ToHexString()}";
        }





    }
}
