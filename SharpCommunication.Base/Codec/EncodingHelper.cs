using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;


namespace SharpCommunication.Codec
{
    public static class EncodingHelper
    {
        public static readonly char[] Alphabet =
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            'a',
            'b',
            'c',
            'd',
            'e',
            'f'
        };



        public static string ToHexString(this byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException($"bytes");
            var length = bytes.Length;
            var stringBuilder = new StringBuilder(length << 1);
            for (var index = 0; index < length; ++index)
            {
                var num = bytes[index];
                var ch1 = Alphabet[num >> 4];
                var ch2 = Alphabet[num & 15];
                stringBuilder.Append(ch1).Append(ch2);
            }
            return stringBuilder.ToString();
        }
        public static string ToHexString(this byte byt)
        {
            if (byt == null)
                throw new ArgumentNullException($"bytes");
            return (new byte[] { byt }).ToHexString();
        }


        public static string ToHexString(this ArraySegment<byte> bytes)
        {
            var offset = bytes.Offset;
            var num1 = offset + bytes.Count;
            var capacity = bytes.Count << 1;
            var stringBuilder = new StringBuilder(capacity)
            {
                Length = capacity
            };
            var index1 = offset;
            var index2 = 0;
            while (index1 < num1)
            {
                if (bytes.Array != null)
                {
                    var num2 = bytes.Array[index1];
                    var ch1 = Alphabet[num2 >> 4];
                    var ch2 = Alphabet[num2 & 15];
                    stringBuilder[index2] = ch1;
                    stringBuilder[index2 + 1] = ch2;
                }
                ++index1;
                index2 += 2;
            }
            return stringBuilder.ToString();
        }

        public static string ToHexString(this byte[] bytes, int offset, int count)
        {
            return new ArraySegment<byte>(bytes, offset, count).ToHexString();
        }

        public static string ToHexString(this IEnumerable<byte> bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            var stringBuilder = new StringBuilder();
            foreach (var num in bytes)
            {
                var ch1 = Alphabet[num >> 4];
                var ch2 = Alphabet[num & 15];
                stringBuilder.Append(ch1).Append(ch2);
            }
            return stringBuilder.ToString();
        }

        public static byte[] ToByteArray(this string hex)
        {
            if (hex == null)
                throw new ArgumentNullException(nameof(hex));
            if (hex.Length % 2 != 0)
                throw new InvalidOperationException("hex length");
            if (hex.Length == 0)
                return new byte[0];
            var numArray = new byte[hex.Length / 2];
            var num1 = 0;
            for (var index1 = 0; index1 < numArray.Length; ++index1)
            {
                var str1 = hex;
                var index2 = num1;
                var num3 = index2 + 1;
                var str2 = str1[index2].ToString(CultureInfo.InvariantCulture);
                var str3 = hex;
                var index3 = num3;
                num1 = index3 + 1;
                var local = str3[index3];
                var str4 = str2 + local;
                numArray[index1] = Convert.ToByte(str4, 16);
            }
            return numArray;
        }
        public static uint ToUnixEpoch(this DateTime dateTime)
        {
            return (uint)Math.Ceiling((dateTime.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
        }
        public static DateTime ToUnixTime(this uint second)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds(second);
        }

    }
}
