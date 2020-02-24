using System;
using System.Collections.Generic;
using System.Text;

namespace RPLidar4Net.Core
{
    public class AngleHelper
    {
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
