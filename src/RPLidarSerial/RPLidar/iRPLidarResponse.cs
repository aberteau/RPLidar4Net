using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPLidarSerial.RPLidar
{

    public interface iRPLidarResponse
    {
        /// <summary>
        /// Total Message Bytes
        /// </summary>
        int Length
        {
            get;
        }
        /// <summary>
        /// Raw Byte information
        /// </summary>
        byte[] Raw
        {
            get;
            set;
        }
        /// <summary>
        /// Parse Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        void parseData(byte[] data);
    }

}
