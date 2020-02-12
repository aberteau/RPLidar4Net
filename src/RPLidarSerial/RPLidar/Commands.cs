using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
// http://www.slamtec.com/download/lidar/documents/en-us/rplidar_interface_protocol_en.pdf
//

namespace RPLidarSerial.RPLidar
{
    public enum Commands : uint
    {
        // Commands without payload and response
        RPLIDAR_CMD_STOP = 0x25,
        RPLIDAR_CMD_SCAN = 0x20,
        RPLIDAR_CMD_FORCE_SCAN = 0x21,
        RPLIDAR_CMD_RESET = 0x40,

        // Commands without payload but have response
        RPLIDAR_CMD_GET_DEVICE_INFO = 0x50,
        RPLIDAR_CMD_GET_DEVICE_HEALTH = 0x52,
    }

    public enum Result : uint
    {
        RESULT_OK               = 0,
        RESULT_FAIL_BIT         = 0x80000000,
        RESULT_ALREADY_DONE     = 0x20,
        RESULT_INVALID_DATA     = (0x8000 | RESULT_FAIL_BIT),
        RESULT_OPERATION_FAIL   = (0x8001 | RESULT_FAIL_BIT),
        RESULT_OPERATION_TIMEOUT   = (0x8002 | RESULT_FAIL_BIT),
        RESULT_OPERATION_STOP     = (0x8003 | RESULT_FAIL_BIT),
        RESULT_OPERATION_NOT_SUPPORT     = (0x8004 | RESULT_FAIL_BIT),
        RESULT_FORMAT_NOT_SUPPORT     = (0x8005 | RESULT_FAIL_BIT),
        RESULT_INSUFFICIENT_MEMORY    = (0x8006 | RESULT_FAIL_BIT),

    }
    public enum Protocol : uint
    {
        RPLIDAR_CMD_SYNC_BYTE = 0xA5,
        RPLIDAR_CMDFLAG_HAS_PAYLOAD = 0x80,
        RPLIDAR_ANS_SYNC_BYTE1 = 0xA5,
        RPLIDAR_ANS_SYNC_BYTE2 = 0x5A,
    }
    public enum Calibration
    {
        RPLIDAR_RESP_MEASUREMENT_SYNCBIT        = (0x1<<0),
        RPLIDAR_RESP_MEASUREMENT_QUALITY_SHIFT  = 2,
        RPLIDAR_RESP_MEASUREMENT_CHECKBIT       = (0x1<<0),
        RPLIDAR_RESP_MEASUREMENT_ANGLE_SHIFT    = 1,
    }
    public enum ReponseType
    {
        RPLIDAR_ANS_TYPE_MEASUREMENT = 0x81,
        RPLIDAR_ANS_TYPE_DEVINFO = 0x4,
        RPLIDAR_ANS_TYPE_DEVHEALTH = 0x6,
    }

    public enum ReponseMode
    {
        RPLIDAR_RESP_MODE_SINGLE = 0x0,
        RPLIDAR_RESP_MODE_MULTI = 0x1,
        RPLIDAR_RESP_MODE_RESV1 = 0x2,
        RPLIDAR_RESP_MODE_RESV2 = 0x3,
    }
    public enum StatusCode
    {
        RPLIDAR_STATUS_OK = 0x0,
        RPLIDAR_STATUS_WARNING = 0x1,
        RPLIDAR_STATUS_ERROR = 0x2,
    }
    public static class Command
    {
        public static byte[] GetBytes(uint sync_byte, uint command_byte)
        {
            return new byte[] { (byte)sync_byte, (byte)command_byte };
        }
    }

}
