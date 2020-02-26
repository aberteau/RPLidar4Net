using System;

namespace RPLidar4Net.Core.Api
{
    public class ScanDataResponseHelper
    {
        public static ScanDataResponse ToScanDataResponse(byte[] bytes)
        {
            if (bytes.Length < Constants.ScanDataResponseLength)
                throw new Exception("RESULT_INVALID_ANS_TYPE");

            ScanDataResponse scanDataResponse = new ScanDataResponse();
            scanDataResponse.SyncAndQuality = bytes[0];
            scanDataResponse.AngleQ6AndCheckbit = BitConverter.ToUInt16(bytes, 1);
            scanDataResponse.DistanceQ2 = BitConverter.ToUInt16(bytes, 3);
            return scanDataResponse;
        }

        public static Point ToPoint(ScanDataResponse scanDataResponse)
        {
            Point point = new Point();
            point.Distance = scanDataResponse.DistanceQ2 / 4.0f;
            point.Angle = (scanDataResponse.AngleQ6AndCheckbit >> Constants.RPLIDAR_RESP_MEASUREMENT_ANGLE_SHIFT) / 64.0f;
            point.Quality = (scanDataResponse.SyncAndQuality >> Constants.RPLIDAR_RESP_MEASUREMENT_QUALITY_SHIFT);
            int startFlag = (scanDataResponse.SyncAndQuality & Constants.RPLIDAR_RESP_MEASUREMENT_SYNCBIT);
            point.StartFlag = (startFlag == 1);
            return point;
        }
    }
}
