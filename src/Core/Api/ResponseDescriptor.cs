using System;
using System.Linq;

namespace RPLidar4Net.Core.Api
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>LR001_SLAMTEC_rplidar_protocol_v2.1_en / p.8</remarks>
    public class ResponseDescriptor
    {
        /// <summary>
        /// A response descriptor uses fixed two bytes (StartFlag1 and StartFlag2) pattern 0xA5 0x5A for the host system to identify the start of a response descriptor.
        /// </summary>
        public byte StartFlag1 { get; set; }

        public byte StartFlag2 { get; set; }

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
