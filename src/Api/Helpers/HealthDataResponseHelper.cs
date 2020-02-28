using System;
using RPLidar4Net.Api.Data;

namespace RPLidar4Net.Api.Helpers
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
