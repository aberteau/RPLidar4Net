using System;

namespace RPLidar4Net.Api.Data
{
    /// <summary>
    /// Health Response
    /// </summary>
    public class HealthDataResponse
        : IDataResponse
    {
        public DataType Type { get; } = DataType.GetHealth;

        /// <summary>
        /// Status Code
        /// 0x0 = OK, 0x1 = Warning, 0x2 = Error
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Error Code Result
        /// </summary>
        public int ErrorCode { get; set; }
    }
}
