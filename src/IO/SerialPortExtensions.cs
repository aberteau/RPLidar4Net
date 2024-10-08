using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using RPLidar4Net.Api.Data;
using RPLidar4Net.Api.Helpers;
using RPLidar4Net.Core;
using Serilog;

namespace RPLidar4Net.IO
{
    public static class SerialPortExtensions
    {
        public static void SendRequest(this SerialPort serialPort, Command command, byte[] payload, bool includePayloadSize)
        {
            Log.Information("SendRequest -- command : {@Command}", command);

            // cf. Request Packets’ Format (p. 6)
            byte commandByte = CommandHelper.GetByte(command);

            var packetBytes = new List<byte>();
            packetBytes.Add(Constants.SYNC_BYTE);
            packetBytes.Add(commandByte);

            //Add payload
            if (payload != null)
            {
                if (includePayloadSize)
                    packetBytes.Add((byte)payload.Length);
                packetBytes.AddRange(payload);
                byte checksum = 0;
                foreach (var b in packetBytes) 
                    checksum ^= b;
                packetBytes.Add(checksum);
            }

            var packet = packetBytes.ToArray();
            string hexString = ByteHelper.ToHexString(packet);
            Log.Information("SendRequest -- packetBytes : {@HexString}", hexString);

            serialPort.Write(packet, 0, packet.Length);
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

                    string hexString = ByteHelper.ToHexString(data);
                    Log.Information("Read -- data : {@HexString}", hexString);

                    return data;
                }
            }

            return data;
        }
    }
}
