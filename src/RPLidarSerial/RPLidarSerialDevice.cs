using RPLidarSerial.RPLidar;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace RPLidarSerial
{
    /// <summary>
    /// Serial connection for Slamtec Robopeak Lidar A1 (A2 untested)
    /// http://www.slamtec.com/en/Lidar/A1
    /// http://www.slamtec.com/download/lidar/documents/en-us/robopeak_2d_lidar_brief_en.pdf
    /// 
    /// http://www.slamtec.com/download/lidar/documents/en-us/rplidar_interface_protocol_en.pdf
    /// </summary>
    public class RPLidarSerialDevice
    {

        /// <summary>
        /// Serial Port Connection
        /// </summary>
        private SerialPort _serialPort;
        /// <summary>
        /// Connection Status
        /// </summary>
        private bool _isConnected { get; set; }
        /// <summary>
        /// Motor Status
        /// </summary>
        private bool _motorRunning { get; set; }
        /// <summary>
        /// Scanning Status
        /// </summary>
        private bool _isScanning { get; set; }
        /// <summary>
        /// Estimated motor speed
        /// </summary>
        private double _motorSpeed { get; set; }
        /// <summary>
        /// Returns connection status
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
        }
        /// <summary>
        /// Get the current estimated motorspeed
        /// </summary>
        public double MotorSpeed
        {
            get { return _motorSpeed; }
        }

        private string _serialNo { get; set; }
        public string SerialNumber
        {
            get { return _serialNo; }
        }

        /// <summary>
        /// Writes out scanned coordinates to console
        /// </summary>
        public bool WriteOutCoordinates { get; set; }
        /// <summary>
        /// Verbose output
        /// </summary>
        public bool Verbose { get; set; }
        /// <summary>
        /// Scanning Thread
        /// </summary>
        private Thread _scanThread = null;
        /// <summary>
        /// Single scan result, full revolution
        /// </summary>
        private List<Response_PointFormat> Frame = new List<Response_PointFormat>();
        /// <summary>
        /// List of Scan Frames
        /// </summary>
        private List<List<Response_PointFormat>> _Frames = new List<List<Response_PointFormat>>();
        /// <summary>
        /// Gets or Sets Frame sets
        /// </summary>
        public List<List<Response_PointFormat>> Frames { get { return _Frames; } set { _Frames = value; } }
        /// <summary>
        /// Data Events
        /// </summary>
        /// <param name="Frame"></param>
        public delegate void DataHandler(List<Response_PointFormat> Frame);
        public event DataHandler Data; 
        /// <summary>
        /// Robo Peak Lidar 360 Scanner, serial connection
        /// </summary>
        /// <param name="portname"></param>
        /// <param name="baudate"></param>
        /// <param name="timeout"></param>
        public RPLidarSerialDevice(string portname = "com4", int baudate = 115200, int timeout = 1000)
        {
            this.WriteOutCoordinates = false;
            // Create a new SerialPort
            _serialPort = new SerialPort();
            _serialPort.PortName = portname;
            _serialPort.BaudRate = baudate;
            //Setup RPLidar specifics
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            // Set the read/write timeouts
            _serialPort.ReadTimeout = timeout;
            _serialPort.WriteTimeout = timeout;
        }

        /// <summary>
        /// Connect to serial port
        /// </summary>
        public void Connect()
        {
            if (this._isConnected)
            {
                Console.WriteLine("Sorry, says here that you're already connected");
            }
            else
            {
                // Connect Serial
                _serialPort.Open();
                this._isConnected = true;
                Console.WriteLine("Connected to RPLidar on {0}", _serialPort.PortName);
            }
        }
        /// <summary>
        /// Disconnect Serial connection to RPLIDAR
        /// </summary>
        public void Disconnect()
        {
            if (_serialPort != null)
            {

                //Stop scan
                if (_isScanning)
                {
                    StopScan();
                }
               
                _serialPort.Close();
                this._isConnected = false;
            }
        }
        /// <summary>
        /// Dispose Object
        /// </summary>
        public void Dispose()
        {
            if (_serialPort != null)
            {
                 if (_isConnected)
                {
                    Disconnect();
                }

                _serialPort.Dispose();

                _serialPort = null;
            }
        }
        /// <summary>
        /// Send Serial Command to RPLidar
        /// Todo: Implement Command with Payload and Checksum.
        /// </summary>
        /// <param name="Command"></param>
        public iRPLidarResponse SendCommand(RPLidar4Net.Core.Api.Command command)
        {
            if (_isConnected)
            {
                //Always combine CMD_SYNC_BYTE with the command
                byte[] cmdBytes = Command.GetBytes((uint)Protocol.RPLIDAR_CMD_SYNC_BYTE, (uint)command);
                //Clear input buffer of any junk
                _serialPort.DiscardInBuffer();
                //Write command bytes
                _serialPort.Write(cmdBytes, 0, cmdBytes.Length);

                //Handle Command Response, reponse format is dependent on command type
                iRPLidarResponse DataResponse = null;
                //We must sleep after executing some commands
                bool sleep = false;
                switch(command)
                {
                    case RPLidar4Net.Core.Api.Command.Scan:
                        DataResponse = null;
                        //Reponses are handled in the Scanning thread
                        break;
                    case RPLidar4Net.Core.Api.Command.GetHealth:
                        DataResponse = new Response_Health();
                        break;
                    case RPLidar4Net.Core.Api.Command.GetInfo:
                        DataResponse = new Response_Information();
                        break;
                    case RPLidar4Net.Core.Api.Command.Reset:
                        sleep = true;
                        DataResponse = null;
                        break;
                    case RPLidar4Net.Core.Api.Command.Stop:
                        sleep = true;
                        DataResponse = null;
                        break;
                    case RPLidar4Net.Core.Api.Command.ForceScan:
                        DataResponse = null;
                        //Reponses are handled in the Scanning thread
                        //Use with care, returns results without motor rotation synchronization
                        break;
                    default:
                        DataResponse = null;
                        break;
                }
                
                //We must sleep after executing some commands
                if (sleep) { 
                    Thread.Sleep(20);
                }
                //Read additional data if any
                if (DataResponse != null)
                {
                    //Read Command Header Response
                    iRPLidarResponse hdrType = new Response_Command();
                    //Poll for data and parse response for the CMD
                    hdrType.parseData(Reponse(1000, hdrType));

                    //Poll for the command data and parse response
                    DataResponse.parseData(Reponse(1000, DataResponse));

                    return DataResponse;
                }
            }
            return null;
        }

        /// <summary>
        /// Start RPLidar Motor
        /// </summary>
        public void StartMotor()
        {
            if (_isConnected)
            {
                _serialPort.DtrEnable = false;
                _motorRunning = true;
            }
        }
        /// <summary>
        /// Stop RPLidar Motor
        /// </summary>
        public void StopMotor()
        {
            if (_isConnected)
            {
                _serialPort.DtrEnable = true;
                _motorRunning = false;
            }
        }

        /// <summary>
        /// Reset RPLidar
        /// </summary>
        public void Reset()
        {
            this.SendCommand(RPLidar4Net.Core.Api.Command.Reset);
        }
        /// <summary>
        /// Get Device Information
        /// Serial No. etc.
        /// </summary>
        public Response_Information GetDeviceInfo()
        {
            return (Response_Information)this.SendCommand(RPLidar4Net.Core.Api.Command.GetInfo);
            
        }
        /// <summary>
        /// Get Device Health Status
        /// </summary>
        public Response_Health GetDeviceHealth()
        {
            return (Response_Health)this.SendCommand(RPLidar4Net.Core.Api.Command.GetHealth);
        }
        /// <summary>
        /// Force Start Scanning
        /// Use with care, returns results without motor rotation synchronization
        /// </summary>
        public void ForceScan()
        {
            //Not already scanning
            if (!_isScanning)
            {
                //Have to be connected
                if (_isConnected)
                {
                    _isScanning = true;
                    //Motor must be running
                    if (!_motorRunning)
                        this.StartMotor();
                    //Start Scan
                    this.SendCommand(RPLidar4Net.Core.Api.Command.ForceScan);
                    //Start Scan read thread
                    _scanThread = new Thread(ScanThread);
                    _scanThread.Start();
                }
            }
        }
        /// <summary>
        /// Start Scanning
        /// </summary>
        public void StartScan()
        {
            //Not already scanning
            if (!_isScanning)
            {
                //Have to be connected
                if (_isConnected)
                {
                    _isScanning = true;
                    //Motor must be running
                    if (!_motorRunning)
                        this.StartMotor();
                    //Start Scan
                    this.SendCommand(RPLidar4Net.Core.Api.Command.Scan);
                    //Start Scan read thread
                    _scanThread = new Thread(ScanThread);
                    _scanThread.Start();
                }
            }
        }
        /// <summary>
        /// Stop Scanning
        /// </summary>
        public void StopScan()
        {
            if (this._isScanning)
            {
                //Avoid thread lock with main thread/gui
                Thread myThread = new System.Threading.Thread(delegate()
                {
                    this._isScanning = false;
                    _scanThread.Join();
                    this.SendCommand(RPLidar4Net.Core.Api.Command.Stop);
                });
                myThread.Start();
                myThread.Join();
                return;
            }
        }
        /// <summary>
        /// Thread used for scanning
        /// Populates a list of Measurements, and adds that list to 
        /// </summary>
        public void ScanThread()
        {
            DateTime _LastSyncBit = new DateTime();
            //Make some room for a measurement
            Response_PointFormat measurementType = null;
            //Start writing output (for piping to csv)
            if (WriteOutCoordinates)
                Console.WriteLine("X:Y");
            //Loop while we're scanning
            while (this._isScanning)
            {
                //Init Point format
                measurementType = new Response_PointFormat();
                //Poll for data and parse response
                measurementType.parseData(waitPoint(1000));
                //measurementType.parseData(Reponse(1000, (iRPLidarResponse)measurementType));
                //Check for new 360 degree scan bit
                if (measurementType.CheckBit)
                {
                    _motorSpeed = (1 / (DateTime.Now - _LastSyncBit).TotalSeconds)*60;
                    _LastSyncBit = DateTime.Now;
                    //New Frame
                    _Frames.Add(Frame);
                    if (Data != null)
                    {
                        //Publish new frame
                        Data(Frame);
                    }
                    //Keep it below 10 frames
                    if (_Frames.Count > 10) { _Frames.RemoveAt(0); }
                    //Start a new Frame list
                    Frame = new List<Response_PointFormat>();
                    if (Verbose)
                        Console.WriteLine(":: Syncbit, last # of valid measurements: " + _Frames[_Frames.Count-1].Count);
                }

                if (WriteOutCoordinates)
                    Console.WriteLine("{0}:{1}",measurementType.X.ToString(), measurementType.Y.ToString());
                //Dont collect invalid data
                if(measurementType.IsValid)
                    Frame.Add(measurementType);
            }
        }
    
        /// <summary>
        /// Waits for Serial Data
        /// </summary>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns></returns>
        public byte[] Reponse(int timeout, iRPLidarResponse ResponseType)
        {
            //Receive buffer
            byte[] data = new byte[ResponseType.Length];
            //Wait for buffer to fill
            DateTime time = DateTime.Now;
            try
            {
                //Make sure we dont run longer than specified timeout
                while ((DateTime.Now - time).TotalMilliseconds < timeout)
                {
                    //Do we have enough bytes?
                    if (_serialPort.BytesToRead < ResponseType.Length)
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        //Read 'em
                        _serialPort.Read(data, 0, ResponseType.Length);
                        return data;
                    }
                }
            }
            catch
            {
                //Set connection status
                this._isConnected = false;
                //Then go through the motions
                this.Disconnect();
            }
            return data;
        }
        private byte[] waitPoint(int timeout)
        {
            DateTime _startTime = DateTime.Now;
            byte[] nodebuf = new byte[5];
            int recvPos = 0;
            try
            {
                while ((DateTime.Now - _startTime).TotalMilliseconds < timeout && _isScanning)
                {
                    int currentByte = _serialPort.ReadByte();
                    if (currentByte < 0)
                        continue;

                    switch (recvPos)
                    {
                        case 0://Byte 0, sync bit and its inverse
                            int tmp = ((byte)currentByte >> 1);
                            int result = (tmp ^ currentByte) & 0x1;
                            if (result == 1)
                            {
                                //Pass
                            }
                            else
                            {
                                continue;
                            }
                            break;
                        case 1://Expect the highest bit to be 1
                            int tmp2 = currentByte & (int)Calibration.RPLIDAR_RESP_MEASUREMENT_CHECKBIT;
                            if (tmp2 == 1)
                            {
                                //pass
                            }
                            else
                            {
                                recvPos = 0;
                                continue;
                            }
                            break;
                    }
                    nodebuf[recvPos++] = Convert.ToByte(currentByte);

                    if (recvPos == 5)
                    {
                        return nodebuf;
                    }
                }
            }
            catch
            {
                //Set connection status
                this._isConnected = false;
                //Then go through the motions
                this.Disconnect();
            }
            return nodebuf;
        }
    }
}
