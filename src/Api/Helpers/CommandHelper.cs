using RPLidar4Net.Api.Data;
using System;
using System.Collections;
using System.Linq;

namespace RPLidar4Net.Api.Helpers
{
    public class CommandHelper
    {
        public static byte GetByte(Command command)
        {
            return (byte)command;
        }

        public static bool GetHasResponse(Command command)
        {
            return command != Command.Stop && command != Command.Reset;
        }

        public static bool GetMustSleep(Command command)
        {
            return command == Command.Reset || command == Command.Stop;
        }

        public static byte[] GetLidarConfigPayload(LidarConfigType configType, UInt16 scanMode = 0)
        {
            byte[] commandBytes;
            byte[] modeBytes = new byte[0];
            switch (configType)
            {
                case LidarConfigType.ScanModeCount: commandBytes = BitConverter.GetBytes((int)LidarConfigType.ScanModeCount); break;
                case LidarConfigType.ScaneModeTypical: commandBytes = BitConverter.GetBytes((int)LidarConfigType.ScaneModeTypical); break;
                case LidarConfigType.ScanModeName: commandBytes = BitConverter.GetBytes((int)LidarConfigType.ScanModeName); modeBytes = BitConverter.GetBytes(scanMode); break;
                case LidarConfigType.ScanModeAnsType: commandBytes = BitConverter.GetBytes((int)LidarConfigType.ScanModeAnsType); modeBytes = BitConverter.GetBytes(scanMode); break;
                default: commandBytes = null; break;
            }

            if (BitConverter.IsLittleEndian)
                return commandBytes.Concat(modeBytes).ToArray();
            else
                return commandBytes.Reverse().Concat(modeBytes).Reverse().ToArray();
        }
    }
}
