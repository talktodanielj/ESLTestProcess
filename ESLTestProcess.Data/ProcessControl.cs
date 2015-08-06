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

        private run _currentTestRun;

        public run GetCurrentTestRun()
        {
            return _currentTestRun;
        }

        public void BeginNewTestRun()
        {
            _currentTestRun = new run();

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.PIC24_ID,
                response_report_column = 0,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.EPROM_ID,
                response_report_column = 1,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.TRANSCEVEIER_ID,
                response_report_column = 2,
                response_value = "Unknown"
            });

            _currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestParameters.ACCELEROMETER_ID,
                response_report_column = 3,
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
    }
}
