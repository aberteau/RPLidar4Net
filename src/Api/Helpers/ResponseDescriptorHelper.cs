using System;
using RPLidar4Net.Api.Data;

namespace RPLidar4Net.Api.Helpers
{
    public class ResponseDescriptorHelper
    {
        private const int DataResponseLengthMask = 0x3FFFFFFF;
        private const int SendModeShift = 30;

        public static ResponseDescriptor ToResponseDescriptor(byte[] data)
        {
            if (data.Length < Constants.ResponseDescriptorLength)
                throw new Exception("RESULT_INVALID_ANS_TYPE");

            //Check Validity
            if (!IsValid(data[0], data[1]))
            {
                throw new Exception("RESULT_INVALID_ANS_TYPE");
            }

            Tuple<uint, SendMode> tuple = GetDataResponseLengthAndSendMode(data);

            ResponseDescriptor responseDescriptor = new ResponseDescriptor();
            responseDescriptor.DataResponseLength = tuple.Item1;
            responseDescriptor.SendMode = tuple.Item2;
            responseDescriptor.DataType = (DataType)data[6];

            return responseDescriptor;
        }

        private static Tuple<uint, SendMode> GetDataResponseLengthAndSendMode(byte[] data)
        {
            UInt32 dataResponseLengthAndSendModeBytes = BitConverter.ToUInt32(data, 2);
            Tuple<uint, SendMode> tuple = GetDataResponseLengthAndSendMode(dataResponseLengthAndSendModeBytes);
            return tuple;
        }

        public static Tuple<uint, SendMode> GetDataResponseLengthAndSendMode(uint dataResponseLengthAndSendMode)
        {
            uint dataResponseLength = dataResponseLengthAndSendMode & DataResponseLengthMask;
            SendMode sendMode = (SendMode)(dataResponseLengthAndSendMode >> SendModeShift);
            Tuple<uint, SendMode> tuple = new Tuple<uint, SendMode>(dataResponseLength, sendMode);
            return tuple;
        }

        /// <summary>
        /// Check Command Packet Validity
        /// </summary>
        /// <param name="startFlag1"></param>
        /// <param name="startFlag2"></param>
        /// <returns>True if packet is valid, False if invalid</returns>
        public static bool IsValid(byte startFlag1, byte startFlag2)
        {
            //Check Validity
            if (startFlag1 != Constants.StartFlag1 ||
                startFlag2 != Constants.StartFlag2)
            {
                return false;
            }
            return true;
        }
    }
}
