using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using log4net;
using System.Reflection;
using System.Configuration;

namespace ESLTestProcess.Data
{
    public sealed class CommunicationManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // -- Singleton pattern -- //
        private static volatile CommunicationManager _instance;
        private static readonly object SyncRoot = new object();

        public static CommunicationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new CommunicationManager();
                    }
                }
                return _instance;
            }
        }

        private SerialPort _serialPort;

        public bool OpenConnection()
        {
            bool success = false;
            try
            {
                string comPort = ConfigurationManager.AppSettings["serial_port"];
                int baudRate = int.Parse(ConfigurationManager.AppSettings["serial_baud_rate"]);
                Parity parity = (Parity)Enum.Parse(typeof(Parity), ConfigurationManager.AppSettings["serial_parity"]);
                StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), ConfigurationManager.AppSettings["serial_stop_bits"]);
                int dataBits = int.Parse(ConfigurationManager.AppSettings["serial_data_bits"]);

                // Close an existing connection if there is one
                CloseConnection();

                _serialPort = new SerialPort(comPort, baudRate, parity, dataBits, stopBits);
                _serialPort.Open();

                success = true;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return success;
        }

        public bool CloseConnection()
        {
            bool success = false;
            try
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                        _serialPort.Close();
                    _serialPort.Dispose();
                    _serialPort = null;
                }
                success = true;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            return success;

        }

        // Format is 0x02-CmdID-0x03-CS
        public bool SendCommand(byte[] buffer)
        {
            lock (SyncRoot)
            {
                bool success = false;
                try
                {
                    //byte checkSum = CreateChecksum(buffer);

                    //List<byte> command = new List<byte>();
                    //command.AddRange(buffer);
                    //command.Add(checkSum); 
                    Console.WriteLine("Sending {0}", BitConverter.ToString(buffer));

                    foreach (var dataByte in buffer)
                    {
                        // Need this sleep or else the node code doest keep up...
                        System.Threading.Thread.Sleep(15);
                        _serialPort.Write(new[] { dataByte }, 0, 1);
                        _serialPort.BaseStream.Flush();
                    }

                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
                return success;
            }
        }

        public SerialPort SerialPort
        {
            get { return _serialPort; }
        }

        private static byte CreateChecksum(byte[] payload)
        {
            byte checkSum = 0;

            foreach (byte item in payload)
            {
                checkSum += item;
            }

            return checkSum;
        }

        public static bool IsValidPacket(byte[] buffer, int rxCount, int expectedLength)
        {
            bool success = false;
            try
            {
                if (rxCount > 0)
                {
                    if (rxCount == expectedLength)
                    {
                        if (buffer[0] == 0x02 && buffer[expectedLength - 3] == 0x03)
                        {
                            var checkSum = CommunicationManager.CreateChecksum(buffer.Take(expectedLength - 1).ToArray());
                            if (checkSum == buffer[expectedLength - 1])
                            {
                                success = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Debug(ex);
            }
            return success;
        }

        public static byte[] ExtractPayload(byte[] buffer, int expectedLength)
        {
            return buffer.Skip(2).Take(expectedLength - 4).ToArray();
        }
    }
}

