using System;

namespace RPLidar4Net.Core.Api
{
    public class ScanDataResponseHelper
    {
        public static ScanDataResponse ToScanDataResponse(byte[] bytes)
        {
            if (bytes.Length < Constants.ScanDataResponseLength)
                throw new Exception("RESULT_INVALID_ANS_TYPE");

            ScanDataResponse scanDataResponse = new ScanDataResponse(bytes);
            return scanDataResponse;
        }

        public static Point ToPoint(ScanDataResponse scanDataResponse)
        {
            Point point = new Point();
            point.Distance = scanDataResponse.distance_q2 / 4.0f;
            point.Angle = (scanDataResponse.angle_q6_checkbit >> Constants.RPLIDAR_RESP_MEASUREMENT_ANGLE_SHIFT) / 64.0f;
            point.Quality = (scanDataResponse.sync_quality >> Constants.RPLIDAR_RESP_MEASUREMENT_QUALITY_SHIFT);
            int startFlag = (scanDataResponse.sync_quality & Constants.RPLIDAR_RESP_MEASUREMENT_SYNCBIT);
            point.StartFlag = (startFlag == 1);
            return point;
        }
    }
}
