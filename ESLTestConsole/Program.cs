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
        static void Main(string[] args)
        {
            if (CommunicationManager.Instance.OpenConnection())
            {
                CommunicationManager.Instance.SerialPort.DataReceived += SerialPort_DataReceived;

                WriteTestOptions();

                Console.WriteLine("");
                Console.WriteLine("TEST OUPUT:");

                StringBuilder sbuilder = new StringBuilder();

                while (true)
                {
                    string input = Console.ReadLine();
                    if (input != null)
                    {
                        if (input.Contains("q"))
                            break;
                        else if (input.Trim() == "1")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_BEGIN_TEST);
                        else if (input.Trim() == "2")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_NODE_ID);
                        else if (input.Trim() == "3")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_BATTERY_LEVEL);
                        else if (input.Trim() == "4")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_TEMPERATURE_LEVEL);
                        else if (input.Trim() == "5")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_FLASH_GREEN_LED);
                        else if (input.Trim() == "6")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_FLASH_RED_LED);
                        else if (input.Trim() == "7")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_BUTTON_TEST);
                        else if (input.Trim() == "a")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_ACCELEROMETER_TEST);
                        else if (input.Trim() == "p")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_PIEZO_TEST);
                        else if (input.Trim() == "h")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_HUB_ID);
                        else if (input.Trim() == "r")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_REED_SWITCH_TEST);
                        else if (input.Trim() == "d")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_DUMP_EPROM_TO_CONSOLE);
                        else if (input.Trim() == "t")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_RTC_VALUE);
                        else if (input.Contains("n"))
                        {
                            byte[] commandBytes = Commands.REQUEST_SET_NODE_ID;
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
                            byte[] commandBytes = Commands.REQUEST_SET_HUB_ID;
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
                            byte[] commandBytes = Commands.REQUEST_SET_RTC_VALUE;

                            commandBytes[2] = ToBcd(DateTime.Now.Year - 2000)[0];
                            commandBytes[3] = ToBcd(DateTime.Now.Month)[0];
                            commandBytes[4] = ToBcd(DateTime.Now.Day)[0];
                            commandBytes[5] = ToBcd(DateTime.Now.Hour)[0];
                            commandBytes[6] = ToBcd(DateTime.Now.Minute)[0];
                            commandBytes[7] = ToBcd(DateTime.Now.Second)[0];

                            CommunicationManager.Instance.SendCommand(commandBytes);

                        }

                        else
                            WriteTestOptions();
                    }

                }
            }

            Console.ReadLine();

        }


        public static byte[] ToBcd(int value)
        {
            if (value < 0 || value > 99999999)
                throw new ArgumentOutOfRangeException("value");
            byte[] ret = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                ret[i] = (byte)(value % 10);
                value /= 10;
                ret[i] |= (byte)((value % 10) << 4);
                value /= 10;
            }
            return ret;
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
            Console.WriteLine("\t q exists the test console");
            Console.WriteLine("--------------------------------------------");
        }

        static bool expectingBatteyLevel = false;

        static void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = CommunicationManager.Instance.SerialPort.ReadExisting();

            if (data != null)
            {
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
