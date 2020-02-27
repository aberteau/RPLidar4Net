using System;
using System.Linq;

namespace RPLidar4Net.Core
{
    public class ByteHelper
    {
        public static string ToHexString(byte[] bytes)
        {
            String byteArrayStr = String.Join(" ", bytes.Select(b => $"{b:X2}"));
            return byteArrayStr;
        }

    }
}
