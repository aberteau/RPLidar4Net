using System;

namespace RPLidar4Net.Api.Data
{
    /// <summary>
    /// Lidar Config Response
    /// </summary>
    public class LidarConfigDataResponse
        : IDataResponse
    {
        public DataType Type { get; } = DataType.LidarConfig;

        /// <summary>
        /// The number of scan modes supported by the device
        /// </summary>
        public int ScanModeCount { get; set; }

        /// <summary>
        /// The id of the preferred scan mode of the device
        /// </summary>
        public byte TypicalScanMode { get; set; }
     
        /// <summary>
        /// The user friendly name of the scan mode
        /// </summary>
        public byte AnswerType { get; set; }

        /// <summary>
        /// The user friendly name of the scan mode
        /// </summary>
        public string ScanModeName { get; set; }
    }
}
