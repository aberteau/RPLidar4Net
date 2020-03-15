using System;
using System.Collections.Generic;
using RPLidar4Net.Core;

namespace RPLidar4Net.IO
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