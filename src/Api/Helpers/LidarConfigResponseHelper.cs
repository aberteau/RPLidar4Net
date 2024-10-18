using System;
using System.Linq;
using System.Text;
using RPLidar4Net.Api.Data;

namespace RPLidar4Net.Api.Helpers
{
    public class LidarConfigDataResponseHelper
    {
        public static LidarConfigDataResponse ToLidarConfigDataResponse(byte[] data)
        {
            LidarConfigDataResponse dataResponse = new LidarConfigDataResponse();
            switch(data[0])
            {
                case (byte)LidarConfigType.DesiredRotationFrequency: dataResponse.DesiredRotationFrequencyRPM = BitConverter.ToUInt16(data, 4); dataResponse.DesiredRotationFrequencyPWM = BitConverter.ToUInt16(data, 6); break;
                case (byte)LidarConfigType.ScanModeCount: dataResponse.ScanModeCount = BitConverter.ToUInt16(data, 4); break;
                case (byte)LidarConfigType.ScaneModeTypical: dataResponse.TypicalScanMode = (byte)BitConverter.ToUInt16(data, 4); break;
                case (byte)LidarConfigType.ScanModeAnsType: dataResponse.AnswerType = data[4]; break;
                case (byte)LidarConfigType.ScanModeName: dataResponse.ScanModeName = Encoding.UTF8.GetString(data.Skip(4).Take(data.Length - 5).ToArray()); break;
            }

            return dataResponse;
        }
    }
}
