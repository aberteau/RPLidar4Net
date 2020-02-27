namespace RPLidar4Net.Core.Api
{
    public class Constants
    {
        public const int ResponseDescriptorLength = 7;
        public const int ScanDataResponseLength = 5;

        public const byte RPLIDAR_RESP_MEASUREMENT_SYNCBIT = (0x1 << 0);
        public const byte RPLIDAR_RESP_MEASUREMENT_QUALITY_SHIFT = 2;
        public const byte RPLIDAR_RESP_MEASUREMENT_ANGLE_SHIFT = 1;

        public const byte SYNC_BYTE = 0xA5;
        public const byte HAS_PAYLOAD_FLAG = 0x80;
        public const byte StartFlag1 = 0xA5;
        public const byte StartFlag2 = 0x5A;
    }
}
