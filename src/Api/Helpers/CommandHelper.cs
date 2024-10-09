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
            return command != Command.Stop && command != Command.Reset && command != Command.MotorSpeedControl && command != Command.SetMotorPWM;
        }

        public static bool GetMustSleep(Command command)
        {
            return command == Command.Reset || command == Command.Stop;
        }

        public static byte[] GetLidarConfigPayload(LidarConfigType configType, UInt16 scanMode = 0)
        {
            byte[] commandBytes = BitConverter.GetBytes((int)configType);
            byte[] modeBytes = new byte[0];
            switch (configType)
            {
                case LidarConfigType.ScanModeName: modeBytes = BitConverter.GetBytes(scanMode); break;
                case LidarConfigType.ScanModeAnsType: modeBytes = BitConverter.GetBytes(scanMode); break;
            }

            if (BitConverter.IsLittleEndian)
                return commandBytes.Concat(modeBytes).ToArray();
            else
                return commandBytes.Reverse().Concat(modeBytes).Reverse().ToArray();
        }
    }
}
