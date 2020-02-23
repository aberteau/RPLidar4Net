using System;
using System.Runtime.InteropServices;

namespace RPLidar4Net.Core.Api
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>LR001_SLAMTEC_rplidar_protocol_v2.1_en / p.16</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct ScanDataResponse
    {
        public byte sync_quality;
        public ushort angle_q6_checkbit;
        public ushort distance_q2;

        public ScanDataResponse(byte[] bytes)
        {
            //Byte0
            sync_quality = bytes[0];
            //Byte 1+2
            angle_q6_checkbit = BitConverter.ToUInt16(bytes, 1);
            //Byte 3+4
            distance_q2 = BitConverter.ToUInt16(bytes, 3);
            //5Bytes total
        }

    }
}