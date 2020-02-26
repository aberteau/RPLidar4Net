using System;
using System.Collections.Generic;
using System.Linq;

namespace RPLidar4Net.Core
{
    public class ByteConverter
    {
        public static bool[] ToBoolArray(byte b)
        {
            // prepare the return result
            bool[] result = new bool[8];

            // check each bit in the byte. if 1 set to true, if 0 set to false
            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) != 0;

            // reverse the array
            Array.Reverse(result);

            return result;
        }

        public static bool[] ToBoolArray(byte[] bytes)
        {
            bool[] boolArrayResult = new bool[0];
            foreach (byte b in bytes)
            {
                bool[] boolArray = ToBoolArray(b);
                boolArrayResult = boolArrayResult.Concat(boolArray).ToArray();
            }

            return boolArrayResult;
        }

        public static UInt32 ToUInt32(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            byte[] rBytes = ToUInt32ByteArray(bytes);

            UInt32 result = BitConverter.ToUInt32(rBytes, 0);
            return result;
        }

        private static byte[] ToUInt32ByteArray(byte[] bytes)
        {
            if (bytes.Length >= BitHelper.UInt32Length)
            {
                return bytes;
            }
            else
            {
                byte[] rBytes = new byte[BitHelper.UInt32Length];
                if (!BitConverter.IsLittleEndian)
                {
                    bytes.CopyTo(rBytes, BitHelper.UInt32Length - bytes.Length);
                }
                else
                {
                    bytes.CopyTo(rBytes, 0);
                }

                return rBytes;
            }
        }
    }
}
