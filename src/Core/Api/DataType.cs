namespace RPLidar4Net.Core.Api
{
    public enum DataType : byte
    {
        // SCAN (p. 14)
        Scan = 0x81,
        // GET_INFO (p. 33)
        GetInfo = 0x04,
        // GET_HEALTH (p.35)
        GetHealth = 0x06
    }
}