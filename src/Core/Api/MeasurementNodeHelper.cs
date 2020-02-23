using System;

namespace RPLidar4Net.Core.Api
{
    public class MeasurementNodeHelper
    {
        public static MeasurementNode ToNode(ScanDataResponse scanDataResponse)
        {
            MeasurementNode measurementNode = new MeasurementNode();
            measurementNode.Distance = scanDataResponse.distance_q2 / 4.0f;
            measurementNode.Angle = (scanDataResponse.angle_q6_checkbit >> Constants.RPLIDAR_RESP_MEASUREMENT_ANGLE_SHIFT) / 64.0f;
            measurementNode.Quality = (scanDataResponse.sync_quality >> Constants.RPLIDAR_RESP_MEASUREMENT_QUALITY_SHIFT);
            int startFlag = (scanDataResponse.sync_quality & Constants.RPLIDAR_RESP_MEASUREMENT_SYNCBIT);
            measurementNode.StartFlag = (startFlag == 1);
            return measurementNode;
        }
    }
}