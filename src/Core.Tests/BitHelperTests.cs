using System;
using Xunit;

namespace RPLidar4Net.Core.Tests
{
    public class BitHelperTests
    {
        [Theory]
        [InlineData(1, 8)]
        [InlineData(2, 16)]
        [InlineData(4, 32)]
        public void GetBitLengthFromByteCountTheory(int byteCount, int expectedResult)
        {
            int result = BitHelper.GetBitLength(byteCount);
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(0x00, 0x00, 0x0, 24)]
        public void GetBitLengthFromByteArrayTheory(byte paramByte1, byte paramByte2, byte paramByte3, int expectedResult)
        {
            Int32 result = BitHelper.GetBitLength(new byte[]{paramByte1, paramByte2, paramByte3});
            Assert.Equal(expectedResult, result);
        }
    }
}
