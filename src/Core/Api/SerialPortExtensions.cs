using System;
using System.IO.Ports;
using System.Threading;

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

        public static byte[] Read(this SerialPort serialPort, int count, int timeout)
        {
            //Receive buffer
            byte[] data = new byte[count];
            //Wait for buffer to fill
            DateTime time = DateTime.Now;

            //Make sure we dont run longer than specified timeout
            while ((DateTime.Now - time).TotalMilliseconds < timeout)
            {
                if (serialPort.BytesToRead < count)
                {
                    Thread.Sleep(10);
                }
                else
                {
                    serialPort.Read(data, 0, count);
                    return data;
                }
            }

            return data;
        }
    }
}
