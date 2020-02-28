using System;
using RPLidar4Net.Api.Data;
using RPLidar4Net.Api.Helpers;
using RPLidar4Net.Core;
using Xunit;

namespace RPLidar4Net.Api.Tests
{
    public class ScanDataResponseHelperTests
    {
        [Theory]
        [InlineData(0x36, 0x37, 0x51, 0x02, 0x06, 162.421875, 384.5, 13, false)]
        [InlineData(0x3E, 0xE7, 0x51, 0xD3, 0x05, 163.796875, 372.75, 15, false)]
        [InlineData(0x3E, 0x7F, 0x52, 0xC9, 0x05, 164.984375, 370.25, 15, false)]
        [InlineData(0x3D, 0x07, 0xB0, 0xC1, 0x0D, 352.046875, 880.25, 15, true)]
        public void Should_To_Point(byte paramByte1, byte paramByte2, byte paramByte3, byte paramByte4, byte paramByte5, float angle, float distance, Int32 quality, bool startFlag)
        {
            ScanDataResponse scanDataResponse = ScanDataResponseHelper.ToScanDataResponse(new byte[] { paramByte1, paramByte2, paramByte3, paramByte4, paramByte5 });
            Point result = ScanDataResponseHelper.ToPoint(scanDataResponse);
            Assert.Equal(angle, result.Angle);
            Assert.Equal(distance, result.Distance);
            Assert.Equal(quality, result.Quality);
            Assert.Equal(startFlag, result.StartFlag);
        }
    }
}
