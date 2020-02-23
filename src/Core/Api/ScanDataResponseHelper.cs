using System;
using System.Collections.Generic;
using System.Text;

namespace RPLidar4Net.Core.Api
{
    public class ScanDataResponseHelper
    {
        public static ScanDataResponse ToScanDataResponse(byte[] bytes)
        {
            if (bytes.Length < Constants.ScanDataResponseLength)
                throw new Exception("RESULT_INVALID_ANS_TYPE");

            ScanDataResponse scanDataResponse = new ScanDataResponse(bytes);
            return scanDataResponse;
        }
    }
}
