using System;
using System.Linq;
using System.Text;
using RPLidar4Net.Api.Data;

namespace RPLidar4Net.Api.Helpers
{
    public class AccBoardFlagResponseHelper
    {
        public static AccBoardDataResponse ToAccBoardFlagDataResponse(byte[] data)
        {
            AccBoardDataResponse dataResponse = new AccBoardDataResponse();

            dataResponse.IsSupported = (BitConverter.ToUInt32(data, 0) & (UInt32)Constants.SL_LIDAR_ANS_TYPE_ACC_BOARD_FLAG) != 0;

            return dataResponse;
        }
    }
}
