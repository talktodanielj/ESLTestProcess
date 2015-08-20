using ESLTestProcess.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.UnitTest
{
    [TestFixture]
    public class TestSerialComms
    {
        [Test]
        public void SetDeviceIntoTestMode()
        {
            Assert.IsTrue(CommunicationManager.Instance.OpenConnection());

            // Send and escape character to set default mode on the PCB
            CommunicationManager.Instance.SendCommand(new byte[] { 0x1B });
            
            byte[] enterProductionTestMode = new[] { (byte)'\r', (byte)'\n', (byte)'X' };

            CommunicationManager.Instance.SendCommand(enterProductionTestMode);

            Debug.WriteLine("TEST OUPUT:");

            StringBuilder sbuilder = new StringBuilder();

            char[] rxBuffer = new char[100];

            while (CommunicationManager.Instance.SerialPort.Read(rxBuffer, 0, rxBuffer.Length) > 0)
            {
                sbuilder.Clear();
                sbuilder.Append(rxBuffer);

                var dataString = sbuilder.ToString();

                Debug.WriteLine(dataString);

                if (dataString.Contains("Inside begin_production_tests"))
                {
                    Debug.WriteLine("Sending begin test command");
                    CommunicationManager.Instance.SendCommand(Commands.REQUEST_BEGIN_TEST);
                    break;
                }
            }

            while (CommunicationManager.Instance.SerialPort.Read(rxBuffer, 0, 1) > 0)
            {
                sbuilder.Clear();
                sbuilder.Append(rxBuffer);
                var dataString = sbuilder.ToString();
                Debug.WriteLine(dataString);
            }
                        
            byte[] inputBuffer = new byte[100];
            while (CommunicationManager.Instance.SerialPort.Read(inputBuffer, 0, inputBuffer.Length) > 0)
            {
                Debug.WriteLine(BitConverter.ToString(inputBuffer));    
            }


        }
    }
}
