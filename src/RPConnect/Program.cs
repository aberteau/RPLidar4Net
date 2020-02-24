using System;
using System.Globalization;
using System.Linq;
using RPLidar4Net.Core;
using RPLidarSerial.RPLidar;
using RPLidarSerial;

namespace RPLidarSerialSimpleConnect
{
    public class Program
    {
        static RPLidarSerialDevice RPLidar;
        private static CultureInfo _cultureInfo;

        public static void Main(string[] args)
        {
            _cultureInfo = new CultureInfo("en-US");

            //New RPLidar object
            RPLidar = new RPLidarSerialDevice("com6");
            //Set output parameters
            RPLidar.Verbose = false;
            try
            {
                //Connect RPLidar
                RPLidar.Connect();
                //Reset - Not really sure how this is supposed to work, reconnecting USB works too
                //RPLidar.Reset();
                //Stop motor
                RPLidar.StopMotor();
                //Get Device Information
                InformationResponse responseInformation = RPLidar.GetDeviceInfo();
                //Get Device Health
                RPLidar.GetDeviceHealth();
                //Get Data Event
                RPLidar.NewScan += RPLidar_NewScan;
                //Start Scan Thread
                RPLidar.StartScan();
            }
            catch (System.IO.IOException ex)
            {
                HandleError("Serial connection failed: " + ex.Message);
            }
            catch(Exception ex)
            {
                HandleError("Something bad happend \n\t:" + ex.Source + "\n\t:" + ex.Message);
            }

            Console.WriteLine("Press any key to exit");         
            Console.ReadKey();

            //Stop Scanning
            RPLidar.StopScan();
            //Disconnect
            RPLidar.Disconnect();
            //Dispose Object
            RPLidar.Dispose();
        }

        private static void RPLidar_NewScan(object sender, NewScanEventArgs eventArgs)
        {
            Point[] points = eventArgs.Points.ToArray();
            foreach(Point point in points)
            {
                Console.WriteLine("Distance: " + point.Distance + " Angle: " + point.Angle);
            }


            //if (points.Any())
            //{
            //    string chrono = DateTime.Now.ToString("yyMMdd HHmmss fff");
            //    var filePath = $@"F:\UserData\Amael\OneDrive\R&D\Lidar\RPLIDAR A1\Scan Data\SlamtecRobopeakLidar\{chrono}.txt";

            //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            //    {
            //        foreach (Point point in points)
            //        {
            //            file.WriteLine($"{point.Angle.ToString(_cultureInfo)} {point.Distance.ToString(_cultureInfo)} {point.Quality}");
            //        }
            //    }
            //}

        }

        private static void HandleError(string Message)
        {
            Console.WriteLine(Message);
            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
