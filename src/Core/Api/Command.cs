namespace RPLidar4Net.Core.Api
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>LR001_SLAMTEC_rplidar_protocol_v2.1_en / p.13</remarks>
    public enum Command
        : byte
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
}
