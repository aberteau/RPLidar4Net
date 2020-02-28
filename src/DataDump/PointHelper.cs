using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RPLidar4Net.Core;

namespace RPLidar4Net.DataDump
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
            IEnumerable<Point> list = points.Where(n => n != null).ToList();
            return list;
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
    }
}
