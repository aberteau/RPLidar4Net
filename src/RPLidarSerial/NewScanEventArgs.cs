using System;
using System.Collections.Generic;
using RPLidar4Net.Core.Api;

namespace RPLidarSerial
{
    public class NewScanEventArgs : EventArgs
    {
        public NewScanEventArgs(IEnumerable<MeasurementNode> nodes)
        {
            Nodes = nodes;
        }

        public IEnumerable<MeasurementNode> Nodes { get; }
    }
}