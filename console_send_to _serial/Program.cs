using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_send_to__serial
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] randomData = new byte[1000];
            Random r = new Random();

            for (int i = 0; i < randomData.Length; i++)
            {
                randomData[i] = (byte)r.Next(0, 255);
            }
            
            if (CommunicationManager.Instance.OpenConnection())
            {
                CommunicationManager.Instance.SerialPort.DataReceived += SerialPort_DataReceived;

                while (true)
                {
                    CommunicationManager.Instance.SerialPort.Write(randomData, 0, randomData.Length);
                    System.Threading.Thread.Sleep(10);
                }

            }
        }

        static void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = CommunicationManager.Instance.SerialPort.ReadExisting();

            if (data != null)
            {
                Console.WriteLine(data);
            }
        }
        
    }
}
