using System;
using Xunit;

namespace RPLidar4Net.Core.Tests
{
    public class ByteConverterTests
    {
        [Theory]
        [InlineData(0x00, 0x00, 0)]
        [InlineData(0x00, 0x01, 1)]
        public void Should_Convert_2Bytes_To_UInt32(byte paramByte1, byte paramByte2, UInt32 expectedResult)
        {
            UInt32 int32 = ByteConverter.ToUInt32(new byte[] { paramByte1, paramByte2 });
            Assert.Equal(expectedResult, int32);
        }

        [Theory]
        [InlineData(0x00, 0x00, 0x0, 0)]
        [InlineData(0x00, 0x01, 0x29, 297)]
        [InlineData(0x04, 0x56, 0x74, 284276)]
        public void Should_Convert_3Bytes_To_UInt32(byte paramByte1, byte paramByte2, byte paramByte3, UInt32 expectedResult)
        {
            UInt32 int32 = ByteConverter.ToUInt32(new byte[]{paramByte1, paramByte2, paramByte3});
            Assert.Equal(expectedResult, int32);
        }

        [Theory]
        [InlineData(0x00, new[] { false, false, false, false, false, false, false, false })]
        [InlineData(0x01, new[] { false, false, false, false, false, false, false, true })]
        [InlineData(0x30, new[] { false, false, true, true, false, false, false, false })]
        public void Should_Convert_Byte_To_BoolArray(byte b, bool[] expectedBoolArray)
        {
            bool[] boolArray = ByteConverter.ToBoolArray(b);

            Assert.Equal(expectedBoolArray, boolArray);
        }
    }
}
