namespace RPLidar4Net.Api.Data
{
    public class Constants
    {
        public const int ResponseDescriptorLength = 7;
        public const int ScanDataResponseLength = 5;

        public const byte RPLIDAR_RESP_MEASUREMENT_SYNCBIT = (0x1 << 0);
        public const byte RPLIDAR_RESP_MEASUREMENT_QUALITY_SHIFT = 2;
        public const byte RPLIDAR_RESP_MEASUREMENT_ANGLE_SHIFT = 1;

        public const byte RPLIDAR_RESP_MEASUREMENT_CHECKBIT = (0x1 << 0);
        public const byte SL_LIDAR_ANS_TYPE_ACC_BOARD_FLAG = 0xFF;
        public const byte SL_LIDAR_RESP_ACC_BOARD_FLAG_MOTOR_CTRL_SUPPORT_MASK = 0x01;
        public const byte A2A3_LIDAR_MINUM_MAJOR_ID = 2;
        public const byte BUILTIN_MOTORCTL_MINUM_MAJOR_ID = 6;

        public const byte SYNC_BYTE = 0xA5;
        public const byte HAS_PAYLOAD_FLAG = 0x80;
        public const byte StartFlag1 = 0xA5;
        public const byte StartFlag2 = 0x5A;
    }
}
