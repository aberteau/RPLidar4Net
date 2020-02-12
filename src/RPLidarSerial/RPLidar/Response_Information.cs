using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPLidarSerial.RPLidar
{
    /// <summary>
    /// Response Information
    /// </summary>
    public class Response_Information : iRPLidarResponse
    {
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        private int _Length = 20;
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        public int Length { get { return _Length; } }
        /// <summary>
        /// Raw Byte information
        /// </summary>
        public byte[] Raw { get; set; }

        /// <summary>
        /// Device Serial Number
        /// </summary>
        private string _serialNumber { get; set; }
        /// <summary>
        /// Device Serial Number
        /// </summary>
        public string SerialNumber {
            get { return _serialNumber; }
        }
        /// <summary>
        /// Device Firmware Version
        /// </summary>
        private string _firmwareVersion { get; set; }
        /// <summary>
        /// Device Firmware Version
        /// </summary>
        public string FirmwareVersion
        {
            get { return _firmwareVersion; }
        }

        private string _hardwareVersion { get; set; }
        /// <summary>
        /// Device Hardware Version
        /// </summary>
        public string HardwareVersion
        {
            get { return _hardwareVersion; }
        }
        private string _modelID { get; set; }
        /// <summary>
        /// Device Model ID
        /// </summary>
        public string ModelID
        {
            get { return _modelID; }
        }

        /// <summary>
        /// Parse Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void parseData(byte[] data)
        {
            this.Raw = data;
            if (data.Length < _Length) throw new Exception("RESULT_INVALID_ANS_TYPE");
            //Model ID
            byte model = data[0];
            this._modelID = model.ToString();
            // Firmware version number, the minor value part, decimal
            byte firmware_version_minor = data[1];
            // Firmware version number, the major value part, integer
            byte firmware_version_major = data[2];
            this._firmwareVersion = firmware_version_major + "." + firmware_version_minor;
            //Hardware version number
            byte hardware_version = data[3];
            this._hardwareVersion = hardware_version.ToString();
            // 128bit unique serial number 
            byte[] serial_number = new byte[16];
            for (int i = 4; i < this._Length; i++)
            {
                serial_number[i - 4] = data[i];
            }
            string serial = BitConverter.ToString(serial_number).Replace("-", "");
            this._serialNumber = serial;
        }

    }

}
