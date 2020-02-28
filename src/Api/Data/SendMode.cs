namespace RPLidar4Net.Api.Data
{
    public enum SendMode
        : byte
    {
        SingleRequestSingleResponse = 0x0,
        SingleRequestMultipleResponse = 0x1,
        ReservedForFutureUse1 = 0x2,
        ReservedForFutureUse2 = 0x3,
    }
}