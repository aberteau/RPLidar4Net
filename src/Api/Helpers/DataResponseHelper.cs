using System;
using System.Collections.Generic;
using System.Text;
using RPLidar4Net.Api.Data;

namespace RPLidar4Net.Api.Helpers
{
    public class DataResponseHelper
    {
        public static IDataResponse ToDataResponse(DataType dataType, byte[] dataResponseBytes)
        {
            switch (dataType)
            {
                case DataType.GetHealth:
                    return HealthDataResponseHelper.ToHealthDataResponse(dataResponseBytes);

                case DataType.GetInfo:
                    return InfoDataResponseHelper.ToInfoDataResponse(dataResponseBytes);
            }
            return null;
        }
    }
}
