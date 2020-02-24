﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPLidar4Net.Core
{
    public class PointHelper
    {
        public static async Task<IEnumerable<Point>> ReadPointsAsync(string path)
        {
            using (StreamReader reader = File.OpenText(path))
            {
                string[] lines = await reader.ReadLinesAsync();
                IEnumerable<Point> points = ToPoints(lines);
                return points;
            }
        }

        public static IEnumerable<Point> ToPoints(IEnumerable<String> lines)
        {
            IEnumerable<Point> points = lines.WhereIsPointDataLine().Select(l => ToPoint(l)).ToList();
            return points;
        }

        public static Point ToPoint(String line)
        {
            string[] values = line.Split(' ');

            if (values.Length != 3)
                return null;

            Point point = new Point();
            CultureInfo cultureInfo = new CultureInfo("en-US");
            point.Angle = Convert.ToSingle(values[0], cultureInfo);
            point.Distance = Convert.ToSingle(values[1], cultureInfo);
            point.Quality = Convert.ToInt32(values[2]);
            return point;
        }

        public static PointF ToPointF(PointF origin, float rotation, float angle, float distance)
        {
            double angleRadian = AngleHelper.DegreeToRadian(angle + rotation);
            double dblX = origin.X + Math.Cos(angleRadian) *  distance;
            double dblY = origin.Y + Math.Sin(angleRadian) * distance;

            float x = Convert.ToSingle(dblX);
            float y = Convert.ToSingle(dblY);
            PointF pointF = new PointF(x, y);
            return pointF;
        }
    }
}
