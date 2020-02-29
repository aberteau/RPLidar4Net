using System;
using RPLidar4Net.Api.Data;
using RPLidar4Net.Api.Helpers;
using Xunit;

namespace RPLidar4Net.Api.Tests
{
    public class ResponseDescriptorHelperTests
    {
        [Theory]
        // GET_INFO (p.33)
        [InlineData(0x14, 0x00, 0x00, 0x00, 20, SendMode.SingleRequestSingleResponse)]

        // GET_HEALTH (p.35)
        [InlineData(0x03, 0x00, 0x00, 0x00, 3, SendMode.SingleRequestSingleResponse)]

        // SCAN (p. 14)
        [InlineData(0x05, 0x00, 0x00, 0x40, 5, SendMode.SingleRequestMultipleResponse)]
        public void Should_GetDataResponseLengthAndSendModeTuple(byte paramByte1, byte paramByte2, byte paramByte3, byte paramByte4, UInt32 dataResponseLength, SendMode sendMode)
        {
            UInt32 dataResponseLengthAndSendMode = BitConverter.ToUInt32(new[] { paramByte1, paramByte2, paramByte3, paramByte4 },0);
            Tuple<uint, SendMode> result = ResponseDescriptorHelper.GetDataResponseLengthAndSendMode(dataResponseLengthAndSendMode);
            Assert.Equal(dataResponseLength, result.Item1);
            Assert.Equal(sendMode, result.Item2);
        }

        [Theory]
        // GET_INFO (p.33)
        [InlineData(0x14, 0x00, 0x00, 0x00, 0x04, 20, SendMode.SingleRequestSingleResponse, DataType.GetInfo)]

        // GET_HEALTH (p.35)
        [InlineData(0x03, 0x00, 0x00, 0x00, 0x06, 3, SendMode.SingleRequestSingleResponse, DataType.GetHealth)]

        // SCAN (p. 14)
        [InlineData(0x05, 0x00, 0x00, 0x40, 0x81, 5, SendMode.SingleRequestMultipleResponse, DataType.Scan)]
        public void Should_To_ResponseDescriptor(byte paramByte1, byte paramByte2, byte paramByte3, byte paramByte4, byte paramByte5, UInt32 dataResponseLength, SendMode sendMode, DataType dataType)
        {
            ResponseDescriptor result = ResponseDescriptorHelper.ToResponseDescriptor(new [] { paramByte1, paramByte2, paramByte3, paramByte4, paramByte5 });
            Assert.Equal(dataResponseLength, result.DataResponseLength);
            Assert.Equal(sendMode, result.SendMode);
            Assert.Equal(dataType, result.DataType);
        }
    }
}
