namespace RPLidar4Net.Api.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>LR001_SLAMTEC_rplidar_protocol_v2.2_en / p.41</remarks>
    public enum LidarConfigType
        : int
    {
        DesiredRotationFrequency = 0x01, //Get the desired motor speed
        ScanModeCount = 0x70, //Get the amount of scan modes supported by the LIDAR
        ScanModeUsPerSample = 0x71, //Get microsecond cost per measurement sample for specific scan mode(in Q8 fixed point format)
        ScanModeMaxDistance = 0x74, //Get max measurement distance for specific scan mode (in m, Q8 fixed point format)
        ScanModeAnsType = 0x75, //Get the answer command type for this scan mode
        ScaneModeTypical = 0x7C, //Get the typical scan mode id of LIDAR
        ScanModeName = 0x7F //Get a user friendly name for the scan mode
    }
}
