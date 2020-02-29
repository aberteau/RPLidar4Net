using System;

namespace RPLidar4Net.Api.Data
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>LR001_SLAMTEC_rplidar_protocol_v2.1_en / p.8</remarks>
    public class ResponseDescriptor
    {
        /// <summary>
        /// Size of a single incoming data response packet in bytes
        /// </summary>
        public UInt32 DataResponseLength { get; set; }

        /// <summary>
        /// Request/response mode of the current session
        /// </summary>
        public SendMode SendMode { get; set; }

        /// <summary>
        /// Type of the incoming data response packets
        /// </summary>
        public DataType DataType { get; set; }
    }
}
