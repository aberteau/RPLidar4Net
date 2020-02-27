using System;

namespace RPLidar4Net.Core.Api
{
    /// <summary>
    /// Health Response
    /// </summary>
    public class HealthDataResponse
        : IDataResponse
    {
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
            Status = data[0]; 
            ErrorCode = BitConverter.ToUInt16(data, 1);
        }

    }
}
