using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace ESLTestProcess
{
    public partial class Main
    {
        private void wizardPageAccelerometerBase_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerBaseline.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestViewParameters.ACCELEROMETER_X_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestViewParameters.ACCELEROMETER_Y_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestViewParameters.ACCELEROMETER_Z_BASE));

                _activeTblLayoutPanel = tblAccelerometerBaseline;
                GenerateTable(_testParameters.ToArray());
            }

        }

        private void wizardPageAccelerometerBase_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            _accelerometerBaseTestRunning = false;
            stepWizardControl1.SelectedPage.AllowNext = false;

            AddRetestLabelToWizard(wizardPageAccelerometerBase);

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestViewParameters.ACCELEROMETER_X_BASE));
            _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestViewParameters.ACCELEROMETER_Y_BASE));
            _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestViewParameters.ACCELEROMETER_Z_BASE));

            _activeTblLayoutPanel = tblAccelerometerBaseline;

            ResetTestParameter(TestViewParameters.ACCELEROMETER_X_BASE);
            ResetTestParameter(TestViewParameters.ACCELEROMETER_Y_BASE);
            ResetTestParameter(TestViewParameters.ACCELEROMETER_Z_BASE);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageAccelerometerBase_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            

        }

        private string _accelerometerBaseXData;
        private string _accelerometerBaseYData;
        private string _accelerometerBaseZData;

        bool _accelerometerBaseTestRunning = false;

        void wizardPageAccelerometerBase_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            response testResponse = null;

            switch (e.ResponseId)
            {
                case TestParameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    Thread.Sleep(10);
                    if(_accelerometerBaseTestRunning)
                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_START_ACCELEROMETER_TEST);
                    _timeOutTimer.Change(8000, Timeout.Infinite);
                    break;

                case TestParameters.TEST_END:
                    _log.Info("Got test end");

                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_X_BASE, "accelerometer_X_base_max", "accelerometer_X_base_min", _accelerometerBaseXData);
                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_Y_BASE, "accelerometer_Y_base_max", "accelerometer_Y_base_min", _accelerometerBaseYData);
                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_Z_BASE, "accelerometer_Z_base_max", "accelerometer_Z_base_min", _accelerometerBaseZData);

                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _accelerometerBaseTestRunning = false;
                    Thread.Sleep(1000);
                    TimeOutCallback(null);
                    break;

                case TestParameters.TEST_ID_START_ACCELEROMETER_TEST:

                    _accelerometerBaseXData = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8], (char)e.RawData[9] });
                    SetTestResponse(_accelerometerBaseXData, TestViewParameters.ACCELEROMETER_X_BASE, e.RawData, TestStatus.Unknown);
                    _accelerometerBaseYData = new string(new[] { (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13], (char)e.RawData[14], (char)e.RawData[15], (char)e.RawData[16], (char)e.RawData[17], (char)e.RawData[18] });
                    SetTestResponse(_accelerometerBaseYData, TestViewParameters.ACCELEROMETER_Y_BASE, e.RawData, TestStatus.Unknown);
                    _accelerometerBaseZData = new string(new[] { (char)e.RawData[20], (char)e.RawData[21], (char)e.RawData[22], (char)e.RawData[23], (char)e.RawData[24], (char)e.RawData[25], (char)e.RawData[26], (char)e.RawData[27] });
                    SetTestResponse(_accelerometerBaseZData, TestViewParameters.ACCELEROMETER_Z_BASE, e.RawData, TestStatus.Unknown);
                    break;
                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }

        private void CheckAccelerometerResult(byte[] rawData, string testParamater, string expectedMaxKey, string expectedMinKey, string actualValueData)
        {

            int expectedMax = int.Parse(ConfigurationManager.AppSettings[expectedMaxKey]);
            int expectedMin = int.Parse(ConfigurationManager.AppSettings[expectedMinKey]);
            int actualValue = 0;

            if (string.IsNullOrEmpty(actualValueData)) actualValueData = "0";
            
            int.TryParse(actualValueData.Trim(), out actualValue);

            if (actualValue <= expectedMax && actualValue >= expectedMin)
                SetTestResponse(actualValueData.Trim(), testParamater, rawData, TestStatus.Pass);
            else
                SetTestResponse(actualValueData.Trim(), testParamater, rawData, TestStatus.Fail);
        }
        
        private void wizardPageAccelerometerBase_Leave(object sender, EventArgs e)
        {
            _accelerometerBaseTestRunning = false;
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageAccelerometerBase_ProcessResponseEventHandler;
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageAccelerometerBase);
        }

        private void wizardPageAccelerometerBase_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            wizardPageAccelerometerBase_Leave(sender, e);
        }

    }
}
