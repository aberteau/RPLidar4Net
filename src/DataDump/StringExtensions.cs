using System;
using System.Collections.Generic;
using System.Linq;

namespace RPLidar4Net.DataDump
{
    public static class StringExtensions
    {
        public static IEnumerable<String> WhereIsPointDataLine(this IEnumerable<String> lines)
        {
            string[] filteredLines = lines.Where(l => !l.StartsWith("#")).ToArray();
            return filteredLines;
        }
    }
}
