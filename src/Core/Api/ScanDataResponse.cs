using System;
using System.Runtime.InteropServices;

namespace RPLidar4Net.Core.Api
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>LR001_SLAMTEC_rplidar_protocol_v2.1_en / p.16</remarks>
    public struct ScanDataResponse
    {
        public byte SyncAndQuality { get; set; }
        public ushort AngleQ6AndCheckbit { get; set; }
        public ushort DistanceQ2 { get; set; }
    }
}