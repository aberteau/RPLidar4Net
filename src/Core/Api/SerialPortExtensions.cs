using System.IO.Ports;

namespace RPLidar4Net.Core.Api
{
    public static class SerialPortExtensions
    {
        public static void SendRequest(this SerialPort serialPort, Command command)
        {
            // cf. Request Packets’ Format (p. 6)
            byte commandByte = CommandHelper.GetByte(command);

            byte[] packetBytes = { Constants.SYNC_BYTE, commandByte };

            //TODO: Add payload

            serialPort.Write(packetBytes, 0, packetBytes.Length);
        }
    }
}
