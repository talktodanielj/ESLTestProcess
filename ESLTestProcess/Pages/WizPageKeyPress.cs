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
        int _activeKey = -1;
        bool _altColour = false;

        private void FlashColourCallback(object sender)
        {
            if (_altColour)
                SetKeyColour(Color.YellowGreen, _activeKey);
            else
                SetKeyColour(Color.ForestGreen, _activeKey);

            _altColour = !_altColour;
        }

        private void SetKeyColour(Color activeColour, int selectedKey)
        {
            Button btnRef = null;
            switch (selectedKey)
            {
                case KEY_ENT:
                    btnRef = this.btnEnt;
                    break;
                case KEY_1_6:
                    btnRef = btn1_6;
                    break;
                case KEY_2_7:
                    btnRef = btn2_7;
                    break;
                case KEY_3_8:
                    btnRef = btn3_8;
                    break;
                case KEY_4_9:
                    btnRef = btn4_9;
                    break;
                case KEY_5_0:
                    btnRef = btn5_0;
                    break;
                default:
                    break;
            }
            if (btnRef != null)
                btnRef.BackColor = activeColour;
        }


        private void wizardPageKeyPress_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            _testParameters.Clear();
            stepWizardControl1.SelectedPage.AllowNext = false;
            _activeTblLayoutPanel = null;

            AddRetestLabelToWizard(wizardPageKeyPress);

            SetKeyColour(Color.ForestGreen, KEY_ENT);
            SetKeyColour(Color.ForestGreen, KEY_1_6);
            SetKeyColour(Color.ForestGreen, KEY_2_7);
            SetKeyColour(Color.ForestGreen, KEY_3_8);
            SetKeyColour(Color.ForestGreen, KEY_4_9);
            SetKeyColour(Color.ForestGreen, KEY_5_0);

            _flashColourTimer.Change(0, 500);
            _byteStreamHandler.ProcessResponseEventHandler += wizardPageKeyPress_ProcessResponseEventHandler;

            _activeKey = KEY_ENT;

            gotKEY_1_6 = gotKEY_2_7 = gotKEY_3_8 = gotKEY_4_9 = gotKEY_5_0 = gotKEY_ENT = false;

            CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);

            Task.Run(() =>
            {
                Thread.Sleep(15000);
                _flashColourTimer.Change(Timeout.Infinite, Timeout.Infinite);
                this.BeginInvoke(new MethodInvoker(delegate
                    {
                        wizardPageKeyPress.AllowNext = true;
                    }));
            });
        }

        private const int KEY_ENT = 2;
        private const int KEY_1_6 = 1;
        private const int KEY_2_7 = 32;
        private const int KEY_3_8 = 16;
        private const int KEY_4_9 = 8;
        private const int KEY_5_0 = 4;
        private const int KEY_END_TEST = 100;

        private bool gotKEY_ENT;
        private bool gotKEY_1_6;
        private bool gotKEY_2_7;
        private bool gotKEY_3_8;
        private bool gotKEY_4_9;
        private bool gotKEY_5_0;

        void wizardPageKeyPress_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            if (e.ResponseId == TestParameters.PARSE_ERROR)
            {
                _log.Info("Got a parse error");

                CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
            }
            else if (e.ResponseId == TestParameters.TEST_ID_BEGIN_TEST)
            {
                _log.Info("Got begin test command");
                Console.WriteLine("Sending keypress test command");

                byte[] commandBytes = TestParameters.REQUEST_START_BUTTON_TEST;
                commandBytes[2] = 10; // Give 10 seconds to complete the test
                commandBytes[3] = (byte)KEY_5_0; // The final key in the sequence that indicates the test should stop
                CommunicationManager.Instance.SendCommand(commandBytes);
            }
            else if (e.ResponseId == TestParameters.TEST_ID_BUTTONS_INITIALISED)
            {
                // Send the command to the test jig to begin testing
                Thread.Sleep(100);
                CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BUTTON_PRESS_SEQ);
            }
            else if (e.ResponseId == TestParameters.TEST_END)
            {
                _log.Info("Detected end of test");

                if (_activeKey == KEY_END_TEST)
                {
                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _flashColourTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    Thread.Sleep(2000);
                    CheckFinalKeyResponse(TestViewParameters.KEY_1_6);
                    CheckFinalKeyResponse(TestViewParameters.KEY_2_7);
                    CheckFinalKeyResponse(TestViewParameters.KEY_3_8);
                    CheckFinalKeyResponse(TestViewParameters.KEY_4_9);
                    CheckFinalKeyResponse(TestViewParameters.KEY_5_0);
                    CheckFinalKeyResponse(TestViewParameters.KEY_ENT);

                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        wizardPageKeyPress.AllowNext = true;

                        if (gotKEY_1_6 && gotKEY_2_7 && gotKEY_3_8 && gotKEY_4_9 && gotKEY_5_0 && gotKEY_ENT)
                            stepWizardControl1.NextPage();
                    }));

                }
            }
            else if (e.ResponseId == TestParameters.TEST_ID_BUTTON_TEST)
            {
                var currentKey = _activeKey;

                switch (e.RawData[2])
                {
                    case KEY_1_6:
                        gotKEY_1_6 = true;
                        _log.Info("Key 1/6");
                        RecordKeyResponse(TestViewParameters.KEY_1_6, e.RawData);
                        _activeKey = KEY_2_7;
                        break;
                    case KEY_ENT:
                        gotKEY_ENT = true;
                        _log.Info("Key ENT");
                        RecordKeyResponse(TestViewParameters.KEY_ENT, e.RawData);
                        _activeKey = KEY_1_6;
                        break;
                    case KEY_5_0:
                        gotKEY_5_0 = true;
                        _log.Info("Key 5/0");
                        RecordKeyResponse(TestViewParameters.KEY_5_0, e.RawData);
                        _activeKey = KEY_END_TEST;
                        break;
                    case KEY_4_9:
                        gotKEY_4_9 = true;
                        _log.Info("Key 4/9");
                        RecordKeyResponse(TestViewParameters.KEY_4_9, e.RawData);
                        _activeKey = KEY_5_0;
                        break;
                    case KEY_3_8:
                        gotKEY_3_8 = true;
                        _log.Info("Key 3/8");
                        RecordKeyResponse(TestViewParameters.KEY_3_8, e.RawData);
                        _activeKey = KEY_4_9;
                        break;
                    case KEY_2_7:
                        gotKEY_2_7 = true;
                        _log.Info("Key 2/7");
                        RecordKeyResponse(TestViewParameters.KEY_2_7, e.RawData);
                        _activeKey = KEY_3_8;
                        break;
                    default:
                        _log.Info("Invalid key code");
                        break;
                }
                SetKeyColour(Color.YellowGreen, currentKey);

            }
            else
            {
                _log.InfoFormat("Response id is {0}", (int)e.ResponseId);
            }
        }

        private void RecordKeyResponse(string keyId, byte[] rawData)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            var testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == keyId);
            testResponse.response_raw = BitConverter.ToString(rawData);
            testResponse.response_value = "PASS";
            testResponse.response_outcome = (Int16)TestStatus.Pass;

        }

        private void CheckFinalKeyResponse(string keyId)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            var testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == keyId);
            if (testResponse.response_outcome == (Int16)TestStatus.Unknown)
            {
                testResponse.response_raw = "";
                testResponse.response_value = "Fail";
                testResponse.response_outcome = (Int16)TestStatus.Fail;
            }
        }


        private void wizardPageKeyPress_Leave(object sender, EventArgs e)
        {
            _flashColourTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageKeyPress_ProcessResponseEventHandler;
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageKeyPress);
        }

        private void wizardPageKeyPress_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            wizardPageKeyPress_Leave(sender, e);
        }
    }
}
