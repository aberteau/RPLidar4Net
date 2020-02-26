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

    public enum Calibration
    {
        RPLIDAR_RESP_MEASUREMENT_CHECKBIT       = (0x1<<0)
    }

    public enum StatusCode
    {
        RPLIDAR_STATUS_OK = 0x0,
        RPLIDAR_STATUS_WARNING = 0x1,
        RPLIDAR_STATUS_ERROR = 0x2,
    }
}
