using System;
using System.Drawing;

namespace RPLidar4Net.Core
{
    class PointFHelper
    {
        public static PointF ToPointF(PointF origin, float rotation, float angle, float distance)
        {
            double angleRadian = AngleHelper.DegreeToRadian(angle + rotation);
            double dblX = origin.X + Math.Cos(angleRadian) * distance;
            double dblY = origin.Y + Math.Sin(angleRadian) * distance;

            float x = Convert.ToSingle(dblX);
            float y = Convert.ToSingle(dblY);
            PointF pointF = new PointF(x, y);
            return pointF;
        }
    }
}
