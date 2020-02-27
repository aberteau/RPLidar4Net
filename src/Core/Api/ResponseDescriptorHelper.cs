using System;
using System.Linq;

namespace RPLidar4Net.Core.Api
{
    public class ResponseDescriptorHelper
    {
        private const int DataResponseLengthMask = 0x3FFFFFFF;
        private const int SendModeShift = 30;

        public static ResponseDescriptor ToResponseDescriptor(byte[] data)
        {
            if (data.Length < Constants.ResponseDescriptorLength)
                throw new Exception("RESULT_INVALID_ANS_TYPE");

            ResponseDescriptor responseDescriptor = new ResponseDescriptor();
            responseDescriptor.StartFlag1 = data[0];
            responseDescriptor.StartFlag2 = data[1];

            //Check Validity
            if (!IsValid(responseDescriptor.StartFlag1, responseDescriptor.StartFlag2))
            {
                throw new Exception("RESULT_INVALID_ANS_TYPE");
            }

            UInt32 dataResponseLengthAndSendModeBytes = BitConverter.ToUInt32(data,2);
            Tuple<uint, SendMode> tuple = GetDataResponseLengthAndSendMode(dataResponseLengthAndSendModeBytes);
            responseDescriptor.DataResponseLength = tuple.Item1;
            responseDescriptor.SendMode = tuple.Item2;
            responseDescriptor.DataType = (DataType)data[6];

            return responseDescriptor;
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
