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
        public run GetCurrentTestRun()
        {
            return _currentSession.runs.Last();
        }

        private void CreateNewTestRunResponses(run currentTestRun)
        {

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.NODE_ID,
                response_report_column = 1,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.HUB_ID,
                response_report_column = 2,
                response_value = "Unknown"
            });


            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.BATTERY_VOLTAGE,
                response_report_column = 4,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.TEMPERATURE_READING,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.LED_GREEN_FLASH,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.LED_RED_FLASH,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.PIEZO_TEST,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.REED_TEST,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.RTC_SET,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.RTC_GET,
                response_report_column = 5,
                response_value = "Unknown"
            });


            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.KEY_ENT,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.KEY_1_6,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.KEY_2_7,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.KEY_3_8,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.KEY_4_9,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.KEY_5_0,
                response_report_column = 5,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_X_BASE,
                response_report_column = 6,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_Y_BASE,
                response_report_column = 7,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_Z_BASE,
                response_report_column = 8,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_X_LONG_EDGE,
                response_report_column = 9,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_Y_LONG_EDGE,
                response_report_column = 10,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_Z_LONG_EDGE,
                response_report_column = 11,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_X_SHORT_EDGE,
                response_report_column = 12,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_Y_SHORT_EDGE,
                response_report_column = 13,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.ACCELEROMETER_Z_SHORT_EDGE,
                response_report_column = 14,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.RF_BGR_RSSI,
                response_report_column = 15,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.RF_HUB_ACK,
                response_report_column = 16,
                response_value = "Unknown"
            });

            currentTestRun.responses.Add(new response
            {
                response_outcome = (Int16)TestStatus.Unknown,
                response_parameter = TestViewParameters.RF_ACK_RSSI,
                response_report_column = 17,
                response_value = "Unknown"
            });
        }


        public bool RecordCurrentTestRun()
        {
            return false;
        }

        public bool IsRetest
        {
            get;
            set;
        }
        
        public void InitialiaseTestRun(string manufactureSerial)
        {
            var currentTestRun = new run();

            var testUnit = DataManager.Instance.GetTestUnit(manufactureSerial);
            if (testUnit != null)
            {
                IsRetest = true;
                // Check if this pcb unit is being tracked in the current DbContext
                pcb_unit localReference = null;
                foreach (var run in _currentSession.runs)
                {
                    if (testUnit.pcb_unit_id == run.pcb_unit.pcb_unit_id)
                        localReference = run.pcb_unit;
                }

                if (localReference != null)
                    currentTestRun.pcb_unit = localReference;
                else
                    currentTestRun.pcb_unit = testUnit;
            }
            else
            {
                IsRetest = false;
                currentTestRun.pcb_unit = new pcb_unit();
                currentTestRun.pcb_unit.pcb_unit_serial_sticker_manufacture = manufactureSerial;
                currentTestRun.pcb_unit.pcb_unit_serial_number = "TEST";
            }

            CreateNewTestRunResponses(currentTestRun);

            _currentSession = DataManager.Instance.AddRun(_currentSession, currentTestRun, !IsRetest);

        }

        public void StartTestSession(string technicianName)
        {
            var technicain = DataManager.Instance.GetTechnician(technicianName);
            _currentSession = new session();
            _currentSession.technician = technicain;
            _currentSession.session_time_stamp = DateTime.Now;

            _currentSession = DataManager.Instance.AddSession(_currentSession);
        }

        public void SaveTestSession()
        {
            DataManager.Instance.SaveResponses(_currentSession);
        }
    }
}
