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

        private void wizardPageAccelTestStep1_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerStep1.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestViewParameters.ACCELEROMETER_X_LONG_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestViewParameters.ACCELEROMETER_Y_LONG_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestViewParameters.ACCELEROMETER_Z_LONG_EDGE));

                _activeTblLayoutPanel = tblAccelerometerStep1;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageAccelTestStep1_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            _accelerometerTestStep1Running = false;
            stepWizardControl1.SelectedPage.AllowNext = false;

            AddRetestLabelToWizard(wizardPageAccelTestStep1);

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestViewParameters.ACCELEROMETER_X_LONG_EDGE));
            _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestViewParameters.ACCELEROMETER_Y_LONG_EDGE));
            _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestViewParameters.ACCELEROMETER_Z_LONG_EDGE));

            _activeTblLayoutPanel = tblAccelerometerStep1;

            ResetTestParameter(TestViewParameters.ACCELEROMETER_X_LONG_EDGE);
            ResetTestParameter(TestViewParameters.ACCELEROMETER_Y_LONG_EDGE);
            ResetTestParameter(TestViewParameters.ACCELEROMETER_Z_LONG_EDGE);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageAccelTestStep1_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
        }

        private string _accelerometerStep1XData;
        private string _accelerometerStep1YData;
        private string _accelerometerStep1ZData;

        bool _accelerometerTestStep1Running = false;

        void wizardPageAccelTestStep1_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            response testResponse = null;

            switch (e.ResponseId)
            {
                case TestParameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    Thread.Sleep(10);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_START_ACCELEROMETER_TEST);
                    _timeOutTimer.Change(5000, Timeout.Infinite);
                    break;

                case TestParameters.TEST_END:
                    _log.Info("Got test end");

                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_X_LONG_EDGE, "accelerometer_X_test1_max", "accelerometer_X_test1_min", _accelerometerStep1XData);
                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_Y_LONG_EDGE, "accelerometer_Y_test1_max", "accelerometer_Y_test1_min", _accelerometerStep1YData);
                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_Z_LONG_EDGE, "accelerometer_Z_test1_max", "accelerometer_Z_test1_min", _accelerometerStep1ZData);

                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    Thread.Sleep(1000);
                    // Reset this so we can retrigger the run
                    _accelerometerTestStep1Running = false;
                    TimeOutCallback(null);
                    break;

                case TestParameters.TEST_ID_START_ACCELEROMETER_TEST:
                    _accelerometerStep1XData = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8], (char)e.RawData[9] });
                    SetTestResponse(_accelerometerStep1XData, TestViewParameters.ACCELEROMETER_X_LONG_EDGE, e.RawData, TestStatus.Unknown);
                    _accelerometerStep1YData = new string(new[] { (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13], (char)e.RawData[14], (char)e.RawData[15], (char)e.RawData[16], (char)e.RawData[17], (char)e.RawData[18] });
                    SetTestResponse(_accelerometerStep1YData, TestViewParameters.ACCELEROMETER_Y_LONG_EDGE, e.RawData, TestStatus.Unknown);
                    _accelerometerStep1ZData = new string(new[] { (char)e.RawData[20], (char)e.RawData[21], (char)e.RawData[22], (char)e.RawData[23], (char)e.RawData[24], (char)e.RawData[25], (char)e.RawData[26], (char)e.RawData[27] });
                    SetTestResponse(_accelerometerStep1ZData, TestViewParameters.ACCELEROMETER_Z_LONG_EDGE, e.RawData, TestStatus.Unknown);
                    break;

                default:
                    _log.Info("Unexpected test response");
                    break;
            };
        }

        private void wizardPageAccelTestStep1_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageAccelTestStep1_ProcessResponseEventHandler;
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageAccelTestStep1);
        }

        private void wizardPageAccelTestStep1_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            wizardPageAccelTestStep1_Leave(sender, e);
        }

    }
}
