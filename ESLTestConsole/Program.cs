using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestConsole
{
    class Program
    {

        static ByteStreamHandler _byteSTreamHandler = new ByteStreamHandler();

        static void Main(string[] args)
        {
            if (CommunicationManager.Instance.OpenConnection())
            {
                CommunicationManager.Instance.SerialPort.DataReceived += SerialPort_DataReceived;

                WriteTestOptions();

                Console.WriteLine("");
                Console.WriteLine("TEST OUPUT:");

                StringBuilder sbuilder = new StringBuilder();

                _byteSTreamHandler.ProcessResponseEventHandler += _byteSTreamHandler_ProcessResponseEventHandler;

                while (true)
                {
                    string input = Console.ReadLine();
                    if (input != null)
                    {
                        if (input.Contains("q"))
                            break;
                        else if (input.Trim() == "1")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
                        else if (input.Trim() == "2")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_NODE_ID);
                        else if (input.Trim() == "3")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BATTERY_LEVEL);
                        else if (input.Trim() == "4")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_TEMPERATURE_LEVEL);
                        else if (input.Trim() == "5")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_FLASH_GREEN_LED);
                        else if (input.Trim() == "6")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_FLASH_RED_LED);
                        else if (input.Trim() == "7")
                        {
                            byte[] commandBytes = Parameters.REQUEST_START_BUTTON_TEST;
                            //commandBytes[2] = 1; // Wait 1 second for a response
                            //commandBytes[3] = 4; // Looking for KEY_5_0
                            CommunicationManager.Instance.SendCommand(commandBytes);
                        }
                        else if (input.Trim() == "a")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_ACCELEROMETER_TEST);
                        else if (input.Trim() == "p")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_PIEZO_TEST);
                        else if (input.Trim() == "h")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_HUB_ID);
                        else if (input.Trim() == "r")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_REED_SWITCH_TEST);
                        else if (input.Trim() == "d")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_DUMP_EPROM_TO_CONSOLE);
                        else if (input.Trim() == "t")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_RTC_VALUE);
                        else if (input.Trim() == "b")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_GET_BGRSSI_VALUE);
                        else if (input.Trim() == "c")
                            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_CAPTURE_HUB);
                        else if (input.Contains("n"))
                        {
                            byte[] commandBytes = Parameters.REQUEST_SET_NODE_ID;
                            input = input.Trim();
                            if (input.Length > 1 && input.Length < 5)
                            {
                                var inputBytes = ASCIIEncoding.ASCII.GetBytes(input);

                                for (int i = inputBytes.Length - 1; i > 0; i--)
                                {
                                    if (i > 0)
                                    {
                                        commandBytes[1 + i] = inputBytes[i];
                                    }
                                }

                                CommunicationManager.Instance.SendCommand(commandBytes);
                            }
                            else
                            {
                                Console.WriteLine("Invalid node id, expecting nX to nXXX");
                            }
                        }
                        else if (input.Contains("i"))
                        {
                            byte[] commandBytes = Parameters.REQUEST_SET_HUB_ID;
                            input = input.Trim();
                            if (input.Length > 1 && input.Length < 4)
                            {
                                var inputBytes = ASCIIEncoding.ASCII.GetBytes(input);

                                for (int i = inputBytes.Length - 1; i > 0; i--)
                                {
                                    if (i > 0)
                                    {
                                        commandBytes[1 + i] = inputBytes[i];
                                    }
                                }

                                CommunicationManager.Instance.SendCommand(commandBytes);
                            }
                            else
                            {
                                Console.WriteLine("Invalid node id, expecting iX to iXX");
                            }
                        }
                        else if (input.Contains("s"))
                        {
                            byte[] commandBytes = Parameters.REQUEST_SET_RTC_VALUE;

                            commandBytes[2] = TestHelper.ToBcd(DateTime.Now.Year - 2000)[0];
                            commandBytes[3] = TestHelper.ToBcd(DateTime.Now.Month)[0];
                            commandBytes[4] = TestHelper.ToBcd(DateTime.Now.Day)[0];
                            commandBytes[5] = TestHelper.ToBcd(DateTime.Now.Hour)[0];
                            commandBytes[6] = TestHelper.ToBcd(DateTime.Now.Minute)[0];
                            commandBytes[7] = TestHelper.ToBcd(DateTime.Now.Second)[0];

                            CommunicationManager.Instance.SendCommand(commandBytes);

                        }
                        else
                            WriteTestOptions();
                    }
                }
            }
            Console.ReadLine();
        }

        static void _byteSTreamHandler_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            if (e.ResponseId == Parameters.PARSE_ERROR)
            {
                Console.WriteLine("Got a parse error");
            }
            else if (e.ResponseId == Parameters.TEST_ID_BEGIN_TEST)
            {
                Console.WriteLine("Got begin test command");

            }
            else if (e.ResponseId == Parameters.TEST_END)
            {
                Console.WriteLine("Got test end response");

            }
            else if (e.ResponseId == Parameters.TEST_ID_BUTTON_TEST)
            {
                switch (e.RawData[2])
                {
                    case 1:
                        Console.WriteLine("Key 1/6");
                        break;
                    case 2:
                        Console.WriteLine("Key ENT");
                        break;
                    case 4:
                        Console.WriteLine("Key 5/0");
                        break;
                    case 8:
                        Console.WriteLine("Key 4/9");
                        break;
                    case 16:
                        Console.WriteLine("Key 3/8");
                        break;
                    case 32:
                        Console.WriteLine("Key 2/7");
                        break;
                    default:
                        Console.WriteLine("Invalid key code");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Response id is {0}", (int)e.ResponseId);
            }

        }


        static void WriteTestOptions()
        {
            Console.WriteLine("Enter an option listed below to selected a test to run");
            Console.WriteLine("\t1)  REQUEST_BEGIN_TEST");
            Console.WriteLine("\t2)  REQUEST_NODE_ID");
            Console.WriteLine("\t3)  REQUEST_BATTERY_LEVEL");
            Console.WriteLine("\t4)  REQUEST_TEMPERATURE_LEVEL");
            Console.WriteLine("\t5)  REQUEST_START_FLASH_GREEN_LED");
            Console.WriteLine("\t6)  REQUEST_START_FLASH_RED_LED");
            Console.WriteLine("\t7)  REQUEST_START_BUTTON_TEST");
            Console.WriteLine("\ta)  REQUEST_START_ACCELEROMETER_TEST");
            Console.WriteLine("\tp)  REQUEST_START_PIEZO_TEST");
            Console.WriteLine("\tr)  REQUEST_REED_SWITCH_TEST");
            Console.WriteLine("\tn)  REQUEST_SET_NODE_ID Format nX to nXXXX");
            Console.WriteLine("\th)  REQUEST_GET_HUB_ID");
            Console.WriteLine("\ti)  REQUEST_SET_HUB_ID Format iX to iXXX");
            Console.WriteLine("\td)  REQUEST_DUMP_EPROM_TO_CONSOLE");
            Console.WriteLine("\tt)  REQUEST_RTC_VALUE");
            Console.WriteLine("\ts)  REQUEST_SET_RTC_VALUE");
            Console.WriteLine("\tb)  REQUEST_GET_BGRSSI_VALUE");
            Console.WriteLine("\tc)  REQUEST_CAPTURE_HUB");
            Console.WriteLine("\t q exists the test console");
            Console.WriteLine("--------------------------------------------");
        }

        static bool expectingBatteyLevel = false;

        static void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = CommunicationManager.Instance.SerialPort.ReadExisting();
            //Console.Write(data);
            //return;
            //if (data != null)
            //{
            //    var responseData = ASCIIEncoding.ASCII.GetBytes(data.Trim());
            //    _byteSTreamHandler.AddToBytesQueue(responseData);
            //}
            //return;

            if (data != null)
            {
                var responseData = ASCIIEncoding.ASCII.GetBytes(data.Trim());

                Console.WriteLine("Rx = {0}", BitConverter.ToString(responseData));

                if (responseData.Length > 1)
                {
                    if (responseData[0] == 0x02 && responseData[1] == Parameters.REQUEST_CAPTURE_HUB[1])
                    {
                        Console.WriteLine("Hub Ack");
                        Console.WriteLine(data);
                    }
                }
                
                if (data.Contains("Battery Level (10mv):"))
                {
                    var rawValue = data.Split(':');
                    if (rawValue.Length == 2)
                    {
                        var bytes = ConvertHexStringToByteArray(rawValue[1]);

                        short batLevel10mV = bytes[1];
                        batLevel10mV |= (short)(bytes[0] << 8);

                        double batteryLevel = batLevel10mV / 100.0;

                        data = data.Trim();
                        Console.Write(data.Replace("-0x", ""));
                        Console.WriteLine(" = {0:0.00}V", batteryLevel);

                    }
                    else
                    {
                        Console.Write(data);
                    }
                }
                else if (data.Contains("Temperature Level:"))
                {
                    var rawValue = data.Split(':');
                    if (rawValue.Length == 2)
                    {
                        var bytes = ConvertHexStringToByteArray(rawValue[1]);

                        short tempLvl = bytes[1];
                        tempLvl |= (short)(bytes[0] << 8);

                        data = data.Trim();
                        Console.Write(data.Replace("-0x", ""));
                        Console.WriteLine(" = {0:0.0}C", tempLvl);
                    }
                    else
                    {
                        Console.Write(data);
                    }
                }
                else
                {
                    Console.Write(data);
                }
            }
        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            // Remove any whitespace charcters that may be in the string
            //hexString = hexString.Replace(" ", "").Replace("\r\n", "");
            hexString = hexString.Replace("\r\n", "");

            string[] hexValues;

            if (hexString.Contains('-'))
                hexValues = hexString.Split('-');
            else
                hexValues = hexString.Split(' ');

            List<byte> byteStream = new List<byte>();

            foreach (string hexVal in hexValues)
            {
                if (hexVal != "")
                    byteStream.Add(Convert.ToByte(hexVal, 16));
            }

            return byteStream.ToArray();
        }
    }
}
