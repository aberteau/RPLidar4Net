using System;
using System.Collections.Generic;
using System.Text;

namespace RPLidar4Net.Core
{
    public class BitHelper
    {
        public const int ByteLength = 8;
        public const int UInt32Length = ByteLength * ByteHelper.UInt32Length;

        public static int GetBitLength(int byteCount)
        {
            return byteCount * ByteLength;
        }

        public static int GetBitLength(byte[] bytes)
        {
            int length = GetBitLength(bytes.Length);
            return length;
        }
    }
}
