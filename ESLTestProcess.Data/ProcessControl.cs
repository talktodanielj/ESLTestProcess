using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public sealed class ProcessControl
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // -- Singleton pattern -- //
        private static volatile ProcessControl _instance;
        private static readonly object SyncRoot = new object();

        public static ProcessControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new ProcessControl();
                    }
                }
                return _instance;
            }
        }

        public event EventHandler<TestResponseEventArgs> TestResponseHandler;

        private session _currentSession;

        private run _currentTestRun;
        public run GetCurrentTestRun()
        {
            return _currentTestRun;
        }

        public void BeginNewTestRun()
        {

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.NODE_ID,
                response_report_column = 1,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.HUB_ID,
                response_report_column = 2,
                response_value = "Unknown"
            });


            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.BATTERY_VOLTAGE,
                response_report_column = 4,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.TEMPERATURE_READING,
                response_report_column = 5,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.LED_GREEN_FLASH,
                response_report_column = 5,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.LED_RED_FLASH,
                response_report_column = 5,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.PIEZO_TEST,
                response_report_column = 5,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.REED_TEST,
                response_report_column = 5,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.RTC_SET,
                response_report_column = 5,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.RTC_GET,
                response_report_column = 5,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_X_BASE,
                response_report_column = 6,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_Y_BASE,
                response_report_column = 7,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_Z_BASE,
                response_report_column = 8,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_X_LONG_EDGE,
                response_report_column = 9,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_Y_LONG_EDGE,
                response_report_column = 10,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_Z_LONG_EDGE,
                response_report_column = 11,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_X_SHORT_EDGE,
                response_report_column = 12,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_Y_SHORT_EDGE,
                response_report_column = 13,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_Z_SHORT_EDGE,
                response_report_column = 14,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.RF_BGR_RSSI,
                response_report_column = 15,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.RF_HUB_ACK,
                response_report_column = 16,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.RF_ACK_RSSI,
                response_report_column = 17,
                response_value = "Unknown"
            });
        }


        public bool RecordCurrentTestRun()
        {
            return false;
        }

        public void TestGetIntialStatus()
        {

            BeginNewTestRun();
            Task.Factory.StartNew(() =>
            {
                /*CommunicationManager.Instance.SendCommand(Commands.REQUEST_BEGIN_TEST);

                byte[] buffer = new byte[100];
                // Assum the idividual responses will be less than 100 bytes and that we get the complete resonse
                var rxCount = CommunicationManager.Instance.SerialPort.Read(buffer, 0, buffer.Length);

                if (rxCount > 0)
                {


                }
                */



                var responseBattV = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.BATTERY_VOLTAGE);

                if (responseBattV != null)
                {
                    responseBattV.response_outcome = (Int16)TestStatus.Warning;
                    responseBattV.response_raw = new byte[] { 0x00, 0x12 };
                    responseBattV.response_value = BitConverter.ToString(responseBattV.response_raw);
                    SignalResponse(responseBattV);
                }

                Thread.Sleep(1000);



            });





            // Simulation
            /*
            Task.Factory.StartNew(() =>
            {
                var responsePicID = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.PIC24_ID);

                if (responsePicID != null)
                {
                    responsePicID.response_outcome = (Int16)TestStatus.Pass;
                    responsePicID.response_raw = new byte[] { 0x54, 0x34 };
                    responsePicID.response_value = BitConverter.ToString(responsePicID.response_raw);
                    SignalResponse(responsePicID);
                }

                Thread.Sleep(2000);

                var responseBattV = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.BATTERY_VOLTAGE);

                if (responseBattV != null)
                {
                    responseBattV.response_outcome = (Int16)TestStatus.Warning;
                    responseBattV.response_raw = new byte[] { 0x00, 0x12 };
                    responseBattV.response_value = BitConverter.ToString(responseBattV.response_raw);
                    SignalResponse(responseBattV);
                }

                Thread.Sleep(1000);

                var responseEpromId = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.EPROM_ID);

                if (responseEpromId != null)
                {
                    responseEpromId.response_outcome = (Int16)TestStatus.Pass;
                    responseEpromId.response_raw = new byte[] { 0x22, 0x22 };
                    responseEpromId.response_value = BitConverter.ToString(responseEpromId.response_raw);
                    SignalResponse(responseEpromId);
                }

                Thread.Sleep(3000);

            });
             * */

        }

        public void TestBaseAccelerometerValues()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                var responseAccelX = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_X_BASE);
                if (responseAccelX != null)
                {
                    responseAccelX.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelX.response_raw = new byte[] { 0x00, 0x00 };
                    responseAccelX.response_value = BitConverter.ToString(responseAccelX.response_raw);
                    SignalResponse(responseAccelX);
                }

                Thread.Sleep(2000);
                var responseAccelY = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_Y_BASE);
                if (responseAccelY != null)
                {
                    responseAccelY.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelY.response_raw = new byte[] { 0x00, 0xFF };
                    responseAccelY.response_value = BitConverter.ToString(responseAccelY.response_raw);
                    SignalResponse(responseAccelY);
                }

                Thread.Sleep(2000);
                var responseAccelZ = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_Z_BASE);
                if (responseAccelZ != null)
                {
                    responseAccelZ.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelZ.response_raw = new byte[] { 0xFF, 0xFF };
                    responseAccelZ.response_value = BitConverter.ToString(responseAccelZ.response_raw);
                    SignalResponse(responseAccelZ);
                }
            });
        }

        public void TestXYAccelerometerValues()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                var responseAccelX = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_X_LONG_EDGE);
                if (responseAccelX != null)
                {
                    responseAccelX.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelX.response_raw = new byte[] { 0x00, 0x00 };
                    responseAccelX.response_value = BitConverter.ToString(responseAccelX.response_raw);
                    SignalResponse(responseAccelX);
                }

                Thread.Sleep(2000);
                var responseAccelY = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_Y_LONG_EDGE);
                if (responseAccelY != null)
                {
                    responseAccelY.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelY.response_raw = new byte[] { 0x00, 0xFF };
                    responseAccelY.response_value = BitConverter.ToString(responseAccelY.response_raw);
                    SignalResponse(responseAccelY);
                }

                Thread.Sleep(2000);
                var responseAccelZ = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_Z_LONG_EDGE);
                if (responseAccelZ != null)
                {
                    responseAccelZ.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelZ.response_raw = new byte[] { 0xFF, 0xFF };
                    responseAccelZ.response_value = BitConverter.ToString(responseAccelZ.response_raw);
                    SignalResponse(responseAccelZ);
                }
            });
        }

        public void TestYZAccelerometerValues()
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                var responseAccelX = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_X_SHORT_EDGE);
                if (responseAccelX != null)
                {
                    responseAccelX.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelX.response_raw = new byte[] { 0x00, 0x00 };
                    responseAccelX.response_value = BitConverter.ToString(responseAccelX.response_raw);
                    SignalResponse(responseAccelX);
                }

                Thread.Sleep(2000);
                var responseAccelY = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_Y_SHORT_EDGE);
                if (responseAccelY != null)
                {
                    responseAccelY.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelY.response_raw = new byte[] { 0x00, 0xFF };
                    responseAccelY.response_value = BitConverter.ToString(responseAccelY.response_raw);
                    SignalResponse(responseAccelY);
                }

                Thread.Sleep(2000);
                var responseAccelZ = _currentTestRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_Z_SHORT_EDGE);
                if (responseAccelZ != null)
                {
                    responseAccelZ.response_outcome = (Int16)TestStatus.Pass;
                    responseAccelZ.response_raw = new byte[] { 0xFF, 0xFF };
                    responseAccelZ.response_value = BitConverter.ToString(responseAccelZ.response_raw);
                    SignalResponse(responseAccelZ);
                }

            });
        }

        private void SignalResponse(response response)
        {
            if (TestResponseHandler != null)
            {
                TestResponseHandler(null, new TestResponseEventArgs
                {
                    Parameter = response.response_parameter,
                    RawValue = response.response_raw,
                    Status = (TestStatus)response.response_outcome,
                    Value = response.response_value
                });
            }
        }



        public void PrepareForTestRun()
        {
            Task.Factory.StartNew(() =>
            {
                CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);

                byte[] buffer = new byte[100];
                // Assume the idividual responses will be less than 100 bytes and that we get the complete response
                var rxCount = CommunicationManager.Instance.SerialPort.Read(buffer, 0, buffer.Length);

                int expectedLength = 5;

                if (CommunicationManager.IsValidPacket(buffer, rxCount, expectedLength))
                {
                    byte[] payload = CommunicationManager.ExtractPayload(buffer, expectedLength);

                    _currentTestRun.pcb_unit = new pcb_unit();
                    //_currentTestRun.pcb_unit.

                   //  short.Parse(BitConverter.ToString(payload)); 

                }
                else
                {
                }
            });
        }

        public void InitialiaseTestRun(string manufactureSerial)
        {
            _currentTestRun = new run();
            _currentTestRun.pcb_unit = new pcb_unit();
            _currentTestRun.pcb_unit.pcb_unit_serial_sticker_manufacture = manufactureSerial;
            // Add the new run to the current session
            _currentSession.runs.Add(_currentTestRun);
            DataManager.Instance.SaveSession(_currentSession);
        }

        public void StartTestSession(string technicianName)
        {
            var technicain = DataManager.Instance.GetTechnician(technicianName);
            _currentSession = new session();
             _currentSession.technician = technicain;
            _currentSession.session_time_stamp = DateTime.Now;

            DataManager.Instance.AddSession(_currentSession);
        }
    }
}
