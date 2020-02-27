using System;
using System.Linq;

namespace RPLidar4Net.Core
{
    public class ByteHelper
    {
        public const int UInt32Length = 4;

        public static string ToHexString(byte[] bytes)
        {
            String byteArrayStr = String.Join(" ", bytes.Select(b => $"{b:X2}"));
            return byteArrayStr;
        }

    }
}
