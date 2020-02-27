using System;

namespace RPLidar4Net.Core.Api
{
    /// <summary>
    /// Response Information
    /// </summary>
    public class InfoDataResponse
        : IDataResponse
    {
        /// <summary>
        /// Device Serial Number
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Device Firmware Version
        /// </summary>
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Device Hardware Version
        /// </summary>
        public string HardwareVersion { get; set; }

        /// <summary>
        /// Device Model ID
        /// </summary>
        public string ModelID { get; set; }
    }

}
