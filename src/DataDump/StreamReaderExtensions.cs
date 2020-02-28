using System;
using System.IO;
using System.Threading.Tasks;

namespace RPLidar4Net.DataDump
{
    public static class StreamReaderExtensions
    {
        public static async Task<String[]> ReadLinesAsync(this StreamReader reader)
        {
            string fileText = await reader.ReadToEndAsync();
            string[] lines = fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return lines;
        }
    }
}