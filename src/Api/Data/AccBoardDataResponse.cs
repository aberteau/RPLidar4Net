using System;

namespace RPLidar4Net.Api.Data
{
    /// <summary>
    /// AccBoardData Response
    /// </summary>
    public class AccBoardDataResponse
        : IDataResponse
    {
        public DataType Type { get; } = DataType.AccBoardFlag;

        public bool IsSupported { get; set; }
    }
}
