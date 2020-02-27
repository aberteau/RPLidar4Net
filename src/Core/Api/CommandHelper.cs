namespace RPLidar4Net.Core.Api
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
    }
}
