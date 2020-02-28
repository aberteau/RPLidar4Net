using System;
using RPLidar4Net.Api.Data;

namespace RPLidar4Net.Api.Helpers
{
    public class InfoDataResponseHelper
    {
        public static InfoDataResponse ToInfoDataResponse(byte[] data)
        {
            InfoDataResponse dataResponse = new InfoDataResponse();
            //Model ID
            byte model = data[0];
            dataResponse.ModelID = model.ToString();
            // Firmware version number, the minor value part, decimal
            byte firmwareVersionMinor = data[1];
            // Firmware version number, the major value part, integer
            byte firmwareVersionMajor = data[2];
            dataResponse.FirmwareVersion = firmwareVersionMajor + "." + firmwareVersionMinor;
            //Hardware version number
            byte hardwareVersion = data[3];
            dataResponse.HardwareVersion = hardwareVersion.ToString();
            // 128bit unique serial number 
            byte[] serialNumber = new byte[16];
            for (int i = 4; i < 20; i++)
            {
                serialNumber[i - 4] = data[i];
            }
            string serial = BitConverter.ToString(serialNumber).Replace("-", "");
            dataResponse.SerialNumber = serial;

            return dataResponse;
        }

    }

}
