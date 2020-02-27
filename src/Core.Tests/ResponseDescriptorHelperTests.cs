using System;
using System.Collections.Generic;
using System.Text;
using RPLidar4Net.Core.Api;
using Xunit;

namespace RPLidar4Net.Core.Tests
{
    public class ResponseDescriptorHelperTests
    {
        [Theory]
        [InlineData(0x14, 0x00, 0x00, 0x00, 0x04, 83886080, SendMode.SingleRequestSingleResponse, DataType.RPLIDAR_ANS_TYPE_DEVINFO)]
        [InlineData(0x03, 0x00, 0x00, 0x00, 0x06, 12582912, SendMode.SingleRequestSingleResponse, DataType.RPLIDAR_ANS_TYPE_DEVHEALTH)]
        public void Should_To_ResponseDescriptor(byte paramByte1, byte paramByte2, byte paramByte3, byte paramByte4, byte paramByte5, UInt32 dataResponseLength, SendMode sendMode, DataType dataType)
        {
            //TODO: Revoir calxuls dataResponseLength et sendMode
            ResponseDescriptor result = ResponseDescriptorHelper.ToResponseDescriptor(new byte[] { Constants.StartFlag1, Constants.StartFlag2, paramByte1, paramByte2, paramByte3, paramByte4, paramByte5 });
            Assert.Equal(Constants.StartFlag1, result.StartFlag1);
            Assert.Equal(Constants.StartFlag2, result.StartFlag2);
            Assert.Equal(dataResponseLength, result.DataResponseLength);
            Assert.Equal(sendMode, result.SendMode);
            Assert.Equal(dataType, result.DataType);
        }
    }
}
