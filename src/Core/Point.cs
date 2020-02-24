using System;

namespace RPLidar4Net.Core
{
    public class Point
    {
        /// <summary>
        /// Heading angle of the measurement (Unit : degree)
        /// </summary>
        public float Angle { get; set; }

        /// <summary>
        /// Measured distance value between the rotating core of the RPLIDAR and the sampling point (Unit : mm)
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// Quality of the measurement
        /// </summary>
        public Int32 Quality { get; set; }

        /// <summary>
        /// New 360 degree scan indicator
        /// </summary>
        public bool StartFlag { get; set; }

        public bool IsValid => Distance > 0f;
    }
}