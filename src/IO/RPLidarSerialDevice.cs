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
    /// <summary>
    /// Serial connection for Slamtec RPLidar
    /// Tested on RPLidar A1 http://www.slamtec.com/en/Lidar/A1
    /// https://download.slamtec.com/api/download/rplidar-protocol/2.1.1?lang=en
    /// </summary>
    public class RPLidarSerialDevice
    {
        /// <summary>
        /// Serial Port Connection
        /// </summary>
        private SerialPort _serialPort;

        private int _readWriteTimeout;

        /// <summary>
        /// Connection Status
        /// </summary>
        private bool _isConnected;

        /// <summary>
        /// Motor Status
        /// </summary>
        private bool _motorRunning;

        /// <summary>
        /// Scanning Status
        /// </summary>
        private bool _isScanning;

        /// <summary>
        /// Estimated motor speed
        /// </summary>
        private double _motorSpeed;

        public double MotorSpeed
        {
            get => _motorSpeed;
            set
            {
                _motorSpeed = value;
                RaiseMotorSpeedChanged();
            }
        }

        public event EventHandler MotorSpeedChanged;

        private void RaiseMotorSpeedChanged()
        {
            MotorSpeedChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Scanning Thread
        /// </summary>
        private Thread _scanThread = null;

        public event EventHandler<NewScanEventArgs> NewScan;

        private void RaiseNewScan(IEnumerable<Point> points)
        {
            if (NewScan != null)
            {
                NewScanEventArgs eventArgs = new NewScanEventArgs(points);
                NewScan.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Robo Peak Lidar 360 Scanner, serial connection
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="timeout"></param>
        public RPLidarSerialDevice(string portName = "com4", int baudRate = 115200, int timeout = 1000)
        {
            // Create a new SerialPort
            _serialPort = new SerialPort();
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            //Setup RPLidar specifics
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            // Set the read/write timeouts
            _serialPort.ReadTimeout = timeout;
            _serialPort.WriteTimeout = timeout;
            _readWriteTimeout = timeout;
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
        /// Send Request to RPLidar
        /// </summary>
        /// <param name="command"></param>
        public IDataResponse SendRequest(Command command, byte[] payload = null, bool includePayloadSize = true)
        {
            if (_isConnected)
            {
                //Clear input buffer of any junk
                _serialPort.DiscardInBuffer();

                _serialPort.SendRequest(command, payload, includePayloadSize);

                bool hasResponse = CommandHelper.GetHasResponse(command);

                //We must sleep after executing some commands
                bool sleep = CommandHelper.GetMustSleep(command);
                if (sleep) {
                    Thread.Sleep(20);
                }

                if (!hasResponse)
                    return null;

                ResponseDescriptor responseDescriptor = ReadResponseDescriptor();

                // Scan Responses are handled in the Scanning thread
                if (responseDescriptor.DataType != DataType.Scan)
                {
                    IDataResponse response = ReadDataResponse(responseDescriptor.DataResponseLength, responseDescriptor.DataType);
                    return response;
                }
            }
            return null;
        }

        private IDataResponse ReadDataResponse(uint dataResponseLength, DataType dataType)
        {
            byte[] dataResponseBytes = Read(dataResponseLength, _readWriteTimeout);
            IDataResponse response = DataResponseHelper.ToDataResponse(dataType, dataResponseBytes);
            return response;
        }

        private ResponseDescriptor ReadResponseDescriptor()
        {
            byte[] bytes = Read(Constants.ResponseDescriptorLength, _readWriteTimeout);

            string hexString = ByteHelper.ToHexString(bytes);
            Log.Information("ReadResponseDescriptor -- bytes : {@hexString}", hexString);

            ResponseDescriptor responseDescriptor = ResponseDescriptorHelper.ToResponseDescriptor(bytes);

            Log.Information("ReadResponseDescriptor -- responseDescriptor : {@responseDescriptor}", responseDescriptor);

            return responseDescriptor;
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
            this.SendRequest(Command.Reset);
        }
        /// <summary>
        /// Get Device Information
        /// Serial No. etc.
        /// </summary>
        public InfoDataResponse GetInfo()
        {
            return (InfoDataResponse)this.SendRequest(Command.GetInfo);

        }
        /// <summary>
        /// Get Device Health Status
        /// </summary>
        public HealthDataResponse GetHealth()
        {
            return (HealthDataResponse)this.SendRequest(Command.GetHealth);
        }
        /// <summary>
        /// Get number of ScanModes via Lidar Conf
        /// </summary>
        public int GetScanModeCount()
        {
            var response = (LidarConfigDataResponse)this.SendRequest(Command.GetLidarConf, CommandHelper.GetLidarConfigPayload(LidarConfigType.ScanModeCount));
            return response.ScanModeCount;
        }
        /// <summary>
        /// Get typical ScanMode via Lidar Conf
        /// </summary>
        public byte GetTypicalScanMode()
        {
            var response = (LidarConfigDataResponse)this.SendRequest(Command.GetLidarConf, CommandHelper.GetLidarConfigPayload(LidarConfigType.ScaneModeTypical));
            return response.TypicalScanMode;
        }
        /// <summary>
        /// Get name of ScanMode via Lidar Conf
        /// </summary>
        public string GetScanModeName(byte scanMode)
        {
            var response = (LidarConfigDataResponse)this.SendRequest(Command.GetLidarConf, CommandHelper.GetLidarConfigPayload(LidarConfigType.ScanModeName, scanMode));
            return response.ScanModeName;
        }
        /// <summary>
        /// Get answer command type of ScanMode via Lidar Conf
        /// </summary>
        public byte GetScanModeAnswerType(byte scanMode)
        {
            var response = (LidarConfigDataResponse)this.SendRequest(Command.GetLidarConf, CommandHelper.GetLidarConfigPayload(LidarConfigType.ScanModeAnsType, scanMode));
            return response.AnswerType;
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
                    this.SendRequest(Command.ForceScan);
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
                    this.SendRequest(Command.Scan);
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
                    this.SendRequest(Command.Stop);
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
            DateTime lastStartFlagDateTime = new DateTime();
            IList<Point> points = new List<Point>();

            //Loop while we're scanning
            while (this._isScanning)
            {
                ScanDataResponse scanDataResponse = ReadScanDataResponse(_readWriteTimeout);
                Point point = ScanDataResponseHelper.ToPoint(scanDataResponse);

                //Check for new 360 degree scan bit
                if (point.StartFlag)
                {
                    MotorSpeed = (1 / (DateTime.Now - lastStartFlagDateTime).TotalSeconds) * 60;
                    lastStartFlagDateTime = DateTime.Now;

                    RaiseNewScan(points);

                    points = new List<Point>();
                }

                if (point.IsValid)
                    points.Add(point);
            }
        }


        private ScanDataResponse ReadScanDataResponse(int timeout)
        {
            byte[] bytes = ReadScanDataResponseBytes(timeout);
            ScanDataResponse scanDataResponse = ScanDataResponseHelper.ToScanDataResponse(bytes);
            return scanDataResponse;
        }

        /// <summary>
        /// Waits for Serial Data
        /// </summary>
        /// <param name="responseLength"></param>
        /// <param name="timeout">Timeout in milliseconds</param>
        /// <returns></returns>
        public byte[] Read(UInt32 responseLength, int timeout)
        {
            try
            {
                byte[] data = _serialPort.Read((int)responseLength, timeout);
                return data;
            }
            catch
            {
                //Set connection status
                this._isConnected = false;
                //Then go through the motions
                this.Disconnect();

                return new byte[0];
            }
        }

        private byte[] ReadScanDataResponseBytes(int timeout)
        {
            DateTime startTime = DateTime.Now;
            byte[] nodebuf = new byte[Constants.ScanDataResponseLength];
            int recvPos = 0;
            try
            {
                while ((DateTime.Now - startTime).TotalMilliseconds < timeout && _isScanning)
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
                            int tmp2 = currentByte & (int)Constants.RPLIDAR_RESP_MEASUREMENT_CHECKBIT;
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
