using System;
using RPLidar4Net.Core.Api;

//
// http://www.slamtec.com/download/lidar/documents/en-us/rplidar_interface_protocol_en.pdf
//

namespace RPLidarSerial.RPLidar
{
    /// <summary>
    /// Scan Response
    /// </summary>
    public class PointFormatResponse
    {
        public MeasurementNode MeasurementNode { get; set; }

        /// <summary>
        /// Measured Angle in Radians
        /// </summary>
        public double AngleRadians { get; set; }
        /// <summary>
        /// X coordinate
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y coordinate
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Whether packet was valid or not
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Parse byte sequence from lidar device
        /// Calculates angle and distance, then plots X,Y for a coordinate system with the lidar device at 0,0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void parseData(byte[] data)
        {
            MeasurementNode = MeasurementNodeHelper.ToNode(data);
            this.IsValid = true;

            this.AngleRadians = (Math.PI / 180) * MeasurementNode.Angle;

            //Calculate Plot Point
            this.X = -MeasurementNode.Distance * Math.Sin(this.AngleRadians);
            this.Y = MeasurementNode.Distance * Math.Cos(this.AngleRadians);

            //According to datasheet
            if (MeasurementNode.Distance <= 0)
                this.IsValid = false;
        }
    }
}
