namespace RPLidar4Net.Core.Api
{
    public enum DataType : byte
    {
        // SCAN (p. 14)
        SCAN = 0x81,
        // GET_INFO (p. 33)
        GET_INFO = 0x04,
        // GET_HEALTH (p.35)
        GET_HEALTH = 0x06
    }
}