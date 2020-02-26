namespace RPLidar4Net.Core.Api
{
    public enum DataType : byte
    {
        RPLIDAR_ANS_TYPE_MEASUREMENT = 0x81,
        RPLIDAR_ANS_TYPE_DEVINFO = 0x4,
        RPLIDAR_ANS_TYPE_DEVHEALTH = 0x6,
    }
}