using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPLidar4Net.Core.Api;

namespace RPLidarSerial.RPLidar
{
    /// <summary>
    /// Response for CMD
    /// </summary>
    public class Response_Command : IResponse
    {
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        private int _Length = 7;
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        public int Length { get { return _Length; } }
        /// <summary>
        /// Raw Byte information
        /// </summary>
        public byte[] Raw { get; set; }

        public byte SyncByte1 { get; set; }
        public byte SyncByte2 { get; set; }
        /// <summary>
        /// Incoming Reponse Data Size
        /// </summary>
        public int ResponseSize { get; set; }
        /// <summary>
        /// Command Reponse Mode
        /// Incoming response type, Single means there will be only 1 response packet after this
        /// Not all CMD's have a response. 
        /// 0 = Single, 1 = Multi
        /// </summary>
        public SendMode ResponseMode { get; set; }
        public ReponseType ResponseType { get; set; }
        /// <summary>
        /// Parse Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void parseData(byte[] data)
        {
            this.Raw = data;
            if (data.Length < _Length) throw new Exception("RESULT_INVALID_ANS_TYPE");

            this.SyncByte1 = data[0];
            this.SyncByte2 = data[1];

            //Check Validity
            if (!IsValid())
            {
                throw new Exception("RESULT_INVALID_ANS_TYPE");
            }

            byte[] _Num = new byte[] { data[2], data[3], data[4], new byte() };
            this.ResponseSize = BitConverter.ToInt32(_Num, 0);

            switch (data[5])
            {
                case (byte)SendMode.SingleRequestSingleResponse:
                    this.ResponseMode = SendMode.SingleRequestSingleResponse;
                    break;
                case (byte)SendMode.SingleRequestMultipleResponse:
                    this.ResponseMode = SendMode.SingleRequestMultipleResponse;
                    break;
            }
            switch(data[6])
            {
                case (byte)ReponseType.RPLIDAR_ANS_TYPE_DEVHEALTH:
                    this.ResponseType = ReponseType.RPLIDAR_ANS_TYPE_DEVHEALTH;
                    break;
                case (byte)ReponseType.RPLIDAR_ANS_TYPE_DEVINFO:
                    this.ResponseType = ReponseType.RPLIDAR_ANS_TYPE_DEVINFO;
                    break;
                case (byte)ReponseType.RPLIDAR_ANS_TYPE_MEASUREMENT:
                    this.ResponseType = ReponseType.RPLIDAR_ANS_TYPE_MEASUREMENT;
                    break;
            }
        }
        /// <summary>
        /// Check Command Packet Validity
        /// </summary>
        /// <returns>True if packet is valid, False if invalid</returns>
        public bool IsValid()
        {
            //Check Validity
            if (this.SyncByte1 != (byte)Protocol.RPLIDAR_ANS_SYNC_BYTE1 ||
                this.SyncByte2 != (byte)Protocol.RPLIDAR_ANS_SYNC_BYTE2)
            {
                return false;
            }
            return true;
        }
    }
}
