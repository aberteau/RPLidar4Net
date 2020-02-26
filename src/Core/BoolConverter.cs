using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPLidar4Net.Core
{
    public class BoolConverter
    {
        public static Byte ToByte(bool[] bArray)
        {
            if (bArray == null)
                throw new ArgumentNullException(nameof(bArray));
            if (bArray.Length != 8)
                throw new ArgumentException("must be 8 bits long");

            byte val = 0;
            foreach (bool b in bArray)
            {
                val <<= 1;
                if (b) val |= 1;
            }
            return val;
        }

        public static Byte[] ToBytes(bool[] bArray)
        {
            if (bArray == null)
                throw new ArgumentNullException(nameof(bArray));
            if (bArray.Length % BitHelper.ByteLength != 0)
                throw new ArgumentException($"must be a multiple of {BitHelper.ByteLength}");

            IList<Byte> bytes = new List<byte>();

            for (int skipCount = 0; skipCount < bArray.Length; skipCount += BitHelper.ByteLength)
            {
                bool[] currentBitArray = bArray.Skip(skipCount).Take(BitHelper.ByteLength).ToArray();
                byte b = ToByte(currentBitArray);
                bytes.Add(b);
            }

            return bytes.ToArray();
        }

        public static UInt32 ToUInt32(bool[] bArray)
        {
            if (bArray == null)
                throw new ArgumentNullException(nameof(bArray));

            int bitLength = BitHelper.UInt32Length;

            if (bArray.Length > BitHelper.UInt32Length)
                throw new ArgumentException("must be at most 32 bits long");

            Int32 startIndex = bitLength - bArray.Length;

            bool[] resultArray = new Boolean[bitLength];

            Int32 t = 0;
            for (int i = startIndex; i < bitLength; i++)
            {
                resultArray[i] = bArray[t];
                t++;
            }

            byte[] bytes = ToBytes(resultArray);

            UInt32 value = ByteConverter.ToUInt32(bytes);
            return value;
        }
    }
}
