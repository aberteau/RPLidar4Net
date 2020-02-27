using System;
using System.Collections.Generic;

namespace RPLidar4Net.Core
{
    public class NewScanEventArgs : EventArgs
    {
        public NewScanEventArgs(IEnumerable<Point> points)
        {
            Points = points;
        }

        public IEnumerable<Point> Points { get; }
    }
}