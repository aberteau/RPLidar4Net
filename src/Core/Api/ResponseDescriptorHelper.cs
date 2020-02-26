using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPLidar4Net.Core.Api
{
    public class ResponseDescriptorHelper
    {
        private const int DataResponseLengthBitLength = 30;

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

            byte[] dataResponseLengthAndSendModeBytes = data.Skip(2).Take(ByteHelper.UInt32Length).ToArray();
            bool[] dataResponseLengthAndSendModeBoolArray = ByteConverter.ToBoolArray(dataResponseLengthAndSendModeBytes);

            responseDescriptor.DataResponseLength = GetDataResponseLength(dataResponseLengthAndSendModeBoolArray);
            responseDescriptor.SendMode = GetSendMode(dataResponseLengthAndSendModeBoolArray);
            responseDescriptor.DataType = GetDataType(data[6]);

            return responseDescriptor;
        }

        private static SendMode GetSendMode(bool[] dataResponseLengthAndSendModeBoolArray)
        {
            bool[] sendModeBoolArray = dataResponseLengthAndSendModeBoolArray.Skip(DataResponseLengthBitLength).Take(2).ToArray();
            bool[] bArray = {false, false, false, false, false, false, sendModeBoolArray[0], sendModeBoolArray[1]};

            byte sendModeByte = BoolConverter.ToByte(bArray);
            SendMode sendMode = GetSendMode(sendModeByte);
            return sendMode;
        }

        private static uint GetDataResponseLength(bool[] bArray)
        {
            bool[] dataResponseLengthBoolArray = bArray.Take(DataResponseLengthBitLength).ToArray();
            uint dataResponseLength = BoolConverter.ToUInt32(dataResponseLengthBoolArray);
            return dataResponseLength;
        }

        private static SendMode GetSendMode(byte b)
        {
            switch (b)
            {
                case (byte) SendMode.SingleRequestSingleResponse:
                    return SendMode.SingleRequestSingleResponse;
                case (byte) SendMode.SingleRequestMultipleResponse:
                    return SendMode.SingleRequestMultipleResponse;
            }

            throw new Exception($"Unknown SendMode {b:X2}");
        }

        private static DataType GetDataType(byte b)
        {
            switch (b)
            {
                case (byte)DataType.RPLIDAR_ANS_TYPE_DEVHEALTH:
                    return DataType.RPLIDAR_ANS_TYPE_DEVHEALTH;
                case (byte)DataType.RPLIDAR_ANS_TYPE_DEVINFO:
                    return DataType.RPLIDAR_ANS_TYPE_DEVINFO;
                case (byte)DataType.RPLIDAR_ANS_TYPE_MEASUREMENT:
                    return DataType.RPLIDAR_ANS_TYPE_MEASUREMENT;
            }

            throw new Exception($"Unknown DataType {b:X2}");
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
            if (startFlag1 != Constants.SYNC_BYTE1 ||
                startFlag2 != Constants.SYNC_BYTE2)
            {
                return false;
            }
            return true;
        }
    }
}
