﻿using ESLTestProcess.Data;
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
        private void wizardPageAccelTestStep2_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerStep2.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestViewParameters.ACCELEROMETER_X_SHORT_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestViewParameters.ACCELEROMETER_Y_SHORT_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestViewParameters.ACCELEROMETER_Z_SHORT_EDGE));

                _activeTblLayoutPanel = tblAccelerometerStep2;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageAccelTestStep2_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            _accelerometerTestStep2Running = false;
            stepWizardControl1.SelectedPage.AllowNext = false;
            AddRetestLabelToWizard(wizardPageAccelTestStep2);

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestViewParameters.ACCELEROMETER_X_SHORT_EDGE));
            _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestViewParameters.ACCELEROMETER_Y_SHORT_EDGE));
            _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestViewParameters.ACCELEROMETER_Z_SHORT_EDGE));

            _activeTblLayoutPanel = tblAccelerometerStep2;

            ResetTestParameter(TestViewParameters.ACCELEROMETER_X_SHORT_EDGE);
            ResetTestParameter(TestViewParameters.ACCELEROMETER_Y_SHORT_EDGE);
            ResetTestParameter(TestViewParameters.ACCELEROMETER_Z_SHORT_EDGE);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageAccelTestStep2_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            _timeOutTimer.Change(8000, Timeout.Infinite);
        }

        private string _accelerometerStep2XData;
        private string _accelerometerStep2YData;
        private string _accelerometerStep2ZData;

        bool _accelerometerTestStep2Running = false;

        void wizardPageAccelTestStep2_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {

            switch (e.ResponseId)
            {
                case TestParameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    Thread.Sleep(10);
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_START_ACCELEROMETER_TEST);
                    _timeOutTimer.Change(5000, Timeout.Infinite);
                    break;

                case TestParameters.TEST_END:
                    _log.Info("Got test end");

                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_X_SHORT_EDGE, "accelerometer_X_test2_max", "accelerometer_X_test2_min", _accelerometerStep2XData);
                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_Y_SHORT_EDGE, "accelerometer_Y_test2_max", "accelerometer_Y_test2_min", _accelerometerStep2YData);
                    CheckAccelerometerResult(e.RawData, TestViewParameters.ACCELEROMETER_Z_SHORT_EDGE, "accelerometer_Z_test2_max", "accelerometer_Z_test2_min", _accelerometerStep2ZData);

                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _accelerometerTestStep2Running = false;
                    Thread.Sleep(1000);
                    TimeOutCallback(null);
                    break;

                case TestParameters.TEST_ID_START_ACCELEROMETER_TEST:
                    _accelerometerStep2XData = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8], (char)e.RawData[9] });
                    SetTestResponse(_accelerometerStep2XData, TestViewParameters.ACCELEROMETER_X_SHORT_EDGE, e.RawData, TestStatus.Unknown);
                    _accelerometerStep2YData = new string(new[] { (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13], (char)e.RawData[14], (char)e.RawData[15], (char)e.RawData[16], (char)e.RawData[17], (char)e.RawData[18] });
                    SetTestResponse(_accelerometerStep2YData, TestViewParameters.ACCELEROMETER_Y_SHORT_EDGE, e.RawData, TestStatus.Unknown);
                    _accelerometerStep2ZData = new string(new[] { (char)e.RawData[20], (char)e.RawData[21], (char)e.RawData[22], (char)e.RawData[23], (char)e.RawData[24], (char)e.RawData[25], (char)e.RawData[26], (char)e.RawData[27] });
                    SetTestResponse(_accelerometerStep2ZData, TestViewParameters.ACCELEROMETER_Z_SHORT_EDGE, e.RawData, TestStatus.Unknown);
                    break;

                default:
                    _log.Info("Unexpected test response");
                    break;
            };
        }

        private void wizardPageAccelTestStep2_Leave(object sender, EventArgs e)
        {
            _accelerometerTestStep2Running = false;
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageAccelTestStep2_ProcessResponseEventHandler;
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageAccelTestStep2);
        }

        private void wizardPageAccelTestStep2_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            wizardPageAccelTestStep2_Leave(sender, e);
        }
    }
}
