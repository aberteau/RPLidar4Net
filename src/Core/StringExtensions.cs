using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPLidar4Net.Core
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
