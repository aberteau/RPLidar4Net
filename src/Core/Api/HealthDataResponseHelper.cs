using System;

namespace RPLidar4Net.Core.Api
{
    public class HealthDataResponseHelper
    {
        public static HealthDataResponse ToHealthDataResponse(byte[] data)
        {
            HealthDataResponse dataResponse = new HealthDataResponse();
            dataResponse.Status = data[0];
            dataResponse.ErrorCode = BitConverter.ToUInt16(data, 1);
            return dataResponse;
        }
    }
}
