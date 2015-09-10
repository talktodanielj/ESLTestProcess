using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESLTestProcess
{
    public partial class Main
    {
        private void wizardPageAccelerometerBase_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerBasline.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestParameters.ACCELEROMETER_X_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestParameters.ACCELEROMETER_Y_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestParameters.ACCELEROMETER_Z_BASE));

                _activeTblLayoutPanel = tblAccelerometerBasline;
                GenerateTable(_testParameters.ToArray());
            }

        }

        private void wizardPageAccelerometerBase_Enter(object sender, EventArgs e)
        {
            _testExpired = false;

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestParameters.ACCELEROMETER_X_BASE));
            _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestParameters.ACCELEROMETER_Y_BASE));
            _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestParameters.ACCELEROMETER_Z_BASE));

            _activeTblLayoutPanel = tblAccelerometerBasline;

            ResetTestParameter(TestParameters.PIEZO_TEST);
            ResetTestParameter(TestParameters.REED_TEST);
            ResetTestParameter(TestParameters.RTC_SET);
            ResetTestParameter(TestParameters.RTC_GET);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageAccelerometerBase_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
            _timeOutTimer.Change(10000, Timeout.Infinite);

        }

        void wizardPageAccelerometerBase_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            response testResponse = null;

            switch (e.ResponseId)
            {
                case Parameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    Thread.Sleep(10);
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
                    break;

                case Parameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_ACCELEROMETER_TEST);
                    break;

                case Parameters.TEST_END:
                    _log.Info("Got test end");

                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    Thread.Sleep(3);
                    TimeOutCallback(null);
                    break;

                case Parameters.TEST_ID_START_ACCELEROMETER_TEST:

                    string accelerometerData = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4],(char)e.RawData[5],(char)e.RawData[6],(char)e.RawData[7]});

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_X_BASE);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = accelerometerData;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });

                    accelerometerData = new string(new[] { (char)e.RawData[10], (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13], (char)e.RawData[14], (char)e.RawData[15] });

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_Y_BASE);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = accelerometerData;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });

                    accelerometerData = new string(new[] { (char)e.RawData[18], (char)e.RawData[19], (char)e.RawData[20], (char)e.RawData[21], (char)e.RawData[22], (char)e.RawData[23] });

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.ACCELEROMETER_Z_BASE);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = accelerometerData;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });

                    break;
                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }

        private void wizardPageAccelerometerBase_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageAccelerometerBase_ProcessResponseEventHandler;
        }

    }
}
