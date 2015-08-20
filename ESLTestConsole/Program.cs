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
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_UNIT_ID);
                        else if (input.Trim() == "3")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_CHIP_IDS);
                        else if (input.Trim() == "4")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_BATTERY_LEVEL);
                        else if (input.Trim() == "5")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_TEMPERATURE_LEVEL);
                        else if (input.Trim() == "6")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_FLASH_GREEN_LED);
                        else if (input.Trim() == "7")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_FLASH_RED_LED);
                        else if (input.Trim() == "8")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_BUTTON_TEST);
                        else if (input.Trim() == "9")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_STOP_BUTTON_TEST);
                        else if (input.Trim() == "A")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_ACCELEROMETER_TEST);
                        else if (input.Trim() == "B")
                            CommunicationManager.Instance.SendCommand(Commands.REQUEST_START_PIEZO_TEST);
                        else
                            CommunicationManager.Instance.SendCommand(new byte[] { 0x00 });
                    }

                }
            }

            Console.ReadLine();

        }

        static void WriteTestOptions()
        {
            Console.WriteLine("Enter an option listed below to selected a test to run");
            Console.WriteLine("\t1)  REQUEST_BEGIN_TEST");
            Console.WriteLine("\t2)  REQUEST_UNIT_ID");
            Console.WriteLine("\t3)  REQUEST_CHIP_IDS");
            Console.WriteLine("\t4)  REQUEST_BATTERY_LEVEL");
            Console.WriteLine("\t5)  REQUEST_TEMPERATURE_LEVEL");
            Console.WriteLine("\t6)  REQUEST_START_FLASH_GREEN_LED");
            Console.WriteLine("\t7)  REQUEST_START_FLASH_RED_LED");
            Console.WriteLine("\t8)  REQUEST_START_BUTTON_TEST");
            Console.WriteLine("\t9)  REQUEST_STOP_BUTTON_TEST");
            Console.WriteLine("\tA)  REQUEST_START_ACCELEROMETER_TEST");
            Console.WriteLine("\tB)  REQUEST_START_PIEZO_TEST");
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

                        float batteryLevel = batLevel10mV / 100;

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
