namespace RPLidar4Net.Api.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>LR001_SLAMTEC_rplidar_protocol_v2.1_en / p.13</remarks>
    public enum Command
        : byte
    {
        Stop = 0x25,
        Reset = 0x40,
        Scan = 0x20,
        ExpressScan = 0x82,
        ForceScan = 0x21,
        GetInfo = 0x50,
        GetHealth = 0x52,
        GetSampleRate = 0x59,
        GetLidarConf = 0x84,
        MotorSpeedControl = 0xA8,
        SetMotorPWM = 0xF0,
        GetAccBoardFlag = 0xFF
    }
}
