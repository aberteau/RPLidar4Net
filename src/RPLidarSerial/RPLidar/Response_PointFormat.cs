using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

//
// http://www.slamtec.com/download/lidar/documents/en-us/rplidar_interface_protocol_en.pdf
//

namespace RPLidarSerial.RPLidar
{
    /// <summary>
    /// Scan Response
    /// </summary>
    public class Response_PointFormat : iRPLidarResponse
    {
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        private int _Length = 5;
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        public int Length { get { return _Length; } }
        /// <summary>
        /// Raw Bytes
        /// </summary>
        public byte[] Raw { get; set; }
        /// <summary>
        /// Syncbit, indicates start of a new 360 degree scan
        /// </summary>
        public bool SyncBit { get; set; }
        /// <summary>
        /// Inverse Syncbit
        /// </summary>
        public bool SyncBitInverse { get; set; }
        /// <summary>
        /// Sync Quality
        /// Related the reflected laser pulse strength.
        /// </summary>
        public int SyncQuality { get; set; }
        /// <summary>
        /// Check Bit
        /// </summary>
        public bool CheckBit { get; set; }
        /// <summary>
        /// Measured Angle in Degrees
        /// </summary>
        public double AngleDegrees { get; set; }
        /// <summary>
        /// Measured Angle in Radians
        /// </summary>
        public double AngleRadians { get; set; }
        /// <summary>
        /// Measured object distance related to RPLIDAR’s rotation center.
        /// In millimeter (mm) unit.
        /// Represents using fix point. 
        /// Set to 0 when the measurement is invalid.
        /// </summary>
        public double Distance { get; set; }
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

        public double distanceQ2 { get; set; }

       
        /// <summary>
        /// Parse byte sequence from lidar device
        /// Calculates angle and distance, then plots X,Y for a coordinate system with the lidar device at 0,0
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void parseData(byte[] data)
        {
          
            measurement_node_t _node = new measurement_node_t(data);
            //Distance in millimeters
            this.Distance = _node.distance_q2 / 4.0;
            this.AngleDegrees = (_node.angle_q6_checkbit >> (ushort)(Calibration.RPLIDAR_RESP_MEASUREMENT_ANGLE_SHIFT)) / 64.0;
            this.SyncQuality = (_node.sync_quality >> (ushort)(Calibration.RPLIDAR_RESP_MEASUREMENT_QUALITY_SHIFT));
            int checkBit = (_node.sync_quality & (ushort)(Calibration.RPLIDAR_RESP_MEASUREMENT_SYNCBIT));

            this.IsValid = true;
            //Save raw bytes
            this.Raw = data;
            //Validation check
            if (data.Length < _Length) throw new Exception("RESULT_INVALID_ANS_TYPE");
            //New 360 degree scan indicator
            if (checkBit == 1)
                this.CheckBit = true;
            //Also get radians for those feeling fancy
            this.AngleRadians = (Math.PI / 180) * this.AngleDegrees;

            //Calculate Plot Point
            this.X = -this.Distance * Math.Sin(this.AngleRadians);
            this.Y = this.Distance * Math.Cos(this.AngleRadians);

            //According to datasheet
            if (this.Distance <= 0)
                this.IsValid = false;
        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct measurement_node_t
    {
        public byte sync_quality;
        public ushort angle_q6_checkbit;
        public ushort distance_q2;

        public measurement_node_t(byte[] bytes)
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
