using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPLidarSerial.RPLidar
{
    /// <summary>
    /// Health Response
    /// </summary>
    public class Response_Health : iRPLidarResponse
    {
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        private int _Length = 3;
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        public int Length { get { return _Length; } }
        /// <summary>
        /// Raw Byte information
        /// </summary
        public byte[] Raw { get; set; }
        /// <summary>
        /// Status Code
        /// 0x0 = OK, 0x1 = Warning, 0x2 = Error
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Error Code Result
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Parse Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void parseData(byte[] data)
        {
            this.Raw = data;
            if (data.Length < _Length) throw new Exception("RESULT_INVALID_ANS_TYPE");
            this.Status = data[0]; 
            this.ErrorCode = BitConverter.ToUInt16(data, 1);
        }

    }
}
