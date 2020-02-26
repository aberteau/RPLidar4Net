using System;
using Xunit;

namespace RPLidar4Net.Core.Tests
{
    public class BoolConverterTests
    {
        [Theory]
        [InlineData(new[] { false, false, false, false, false, false, false, false }, 0x00)]
        [InlineData(new[] { false, false, false, false, false, false, false, true }, 0x01)]
        [InlineData(new[] { false, false, true, true, false, false, false, false }, 0x30)]
        public void BoolArrayToByteTheory(bool[] boolArray, byte expected)
        {
            Byte result = BoolConverter.ToByte(boolArray);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(new[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, true, false, false, false, false }, new byte[] { 0x00, 0x01, 0x30 })]
        public void BoolArrayToByteArrayTheory(bool[] boolArray, byte[] expected)
        {
            Byte[] result = BoolConverter.ToBytes(boolArray);
            Assert.Equal(expected[0], result[0]);
            Assert.Equal(expected[1], result[1]);
        }

        [Theory]
        [InlineData(new[] { false, false, false, false, false, false, false, false }, 0)]
        [InlineData(new[] { false, false, false, false, false, false, false, true }, 1)]
        [InlineData(new[] { false, false, true, true, false, false, false, false }, 48)]
        public void BoolToUInt32Theory(bool[] boolArray, UInt32 expected)
        {
            UInt32 result = BoolConverter.ToUInt32(boolArray);
            Assert.Equal(expected, result);
        }
    }
}
