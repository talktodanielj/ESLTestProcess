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

        private void FlashLEDBtnCallback(object sender)
        {
            if (_activeLEDBtn == 0)
            {
                if (_altColour)
                {
                    btnLED1.BackColor = Color.LawnGreen;
                    btnLED1.ForeColor = Color.White;
                }
                else
                {
                    btnLED1.BackColor = _originalBtnColour;
                    btnLED1.ForeColor = Color.Black;
                }
            }

            if (_activeLEDBtn == 1)
            {
                if (_altColour)
                {
                    btnLED2.BackColor = Color.Red;
                    btnLED2.ForeColor = Color.White;
                }
                else
                {
                    btnLED2.BackColor = _originalBtnColour;
                    btnLED2.ForeColor = Color.Black;
                }
            }


            _altColour = !_altColour;
        }

        private void ResetTestParameter(string testParameter)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            if (testRun != null)
            {
                var testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == testParameter);
                if (testResponse != null)
                {
                    testResponse.response_outcome = (Int16)TestStatus.Unknown;
                    testResponse.response_raw = new byte[0];
                    testResponse.response_value = "Unknown";

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                }
            }
        }

        Color _originalBtnColour = Color.White;
        int _activeLEDBtn = 0;

        private void wizardPageResultsStatus_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tbllnitialStatus.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Node Id", TestParameters.NODE_ID));
                _testParameters.Add(new Tuple<string, string>("Hub Id", TestParameters.HUB_ID));
                _testParameters.Add(new Tuple<string, string>("Battery Volatge", TestParameters.BATTERY_VOLTAGE));
                _testParameters.Add(new Tuple<string, string>("Temperature", TestParameters.TEMPERATURE_READING));

                _activeTblLayoutPanel = tbllnitialStatus;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageResultsStatus_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            wizardPageResultsStatus.AllowNext = false;

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Node Id", TestParameters.NODE_ID));
            _testParameters.Add(new Tuple<string, string>("Hub Id", TestParameters.HUB_ID));
            _testParameters.Add(new Tuple<string, string>("Battery Volatge", TestParameters.BATTERY_VOLTAGE));
            _testParameters.Add(new Tuple<string, string>("Temperature", TestParameters.TEMPERATURE_READING));

            _activeTblLayoutPanel = tbllnitialStatus;

            if (_originalBtnColour == Color.White)
                _originalBtnColour = btnLED1.BackColor;

            btnLED1.BackColor = _originalBtnColour;
            btnLED1.ForeColor = Color.Black;
            btnLED2.BackColor = _originalBtnColour;
            btnLED2.ForeColor = Color.Black;

            _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);

            ResetTestParameter(TestParameters.BATTERY_VOLTAGE);
            ResetTestParameter(TestParameters.TEMPERATURE_READING);
            ResetTestParameter(TestParameters.NODE_ID);
            ResetTestParameter(TestParameters.HUB_ID);

            pictureBoxLED1.Image = ESLTestProcess.Properties.Resources.test_spinner;
            pictureBoxLED2.Image = ESLTestProcess.Properties.Resources.test_spinner;

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageResultsStatus_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);

            _timeOutTimer.Change(10000, Timeout.Infinite);

        }

        void wizardPageResultsStatus_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            byte[] rawData;
            response testResponse = null;

            switch (e.ResponseId)
            {
                case Parameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
                    break;

                case Parameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_NODE_ID);
                    break;

                case Parameters.TEST_ID_NODE_ID:
                    string nodeId = new string(new[] { (char)e.RawData[3], (char)e.RawData[2], (char)e.RawData[5], (char)e.RawData[4], (char)e.RawData[7], (char)e.RawData[6] });
                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.NODE_ID);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = nodeId;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_HUB_ID);
                    break;

                case Parameters.TEST_ID_HUB_ID:
                    string hubId = new string(new[] { (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[2], (char)e.RawData[3] });
                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.HUB_ID);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = hubId;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BATTERY_LEVEL);
                    break;

                case Parameters.TEST_ID_BATTERY_LEVEL:
                    rawData = e.RawData.Skip(2).Take(3).Reverse().ToArray();
                    string batteryData = new string(new[] { (char)rawData[0], (char)rawData[1], (char)rawData[2] });
                    int batLevel10mV = int.Parse(batteryData);
                    double batteryLevel = batLevel10mV / 100.0;
                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.BATTERY_VOLTAGE);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = string.Format("{0:0.00}V", batteryLevel);
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });

                    _log.Info("Got battery level result");
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_TEMPERATURE_LEVEL);
                    break;

                case Parameters.TEST_ID_TEMPERATURE_LEVEL:
                    rawData = e.RawData.Skip(2).Take(4).ToArray();
                    string temperatureData = new string(new[] { (char)rawData[2], (char)rawData[3] });
                    int temperature = Convert.ToInt32(temperatureData, 16);
                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.TEMPERATURE_READING);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = string.Format("{0:0.0}C", temperature);
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    _log.Info("Got temperature level");

                    _altColour = false;
                    _activeLEDBtn = 0;
                    _flashLedBtnTimer.Change(0, 500);
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_FLASH_GREEN_LED);
                    break;

                case Parameters.TEST_ID_START_FLASH_GREEN_LED:
                    _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _altColour = true;
                    FlashLEDBtnCallback(null);
                    pictureBoxLED1.Image = ESLTestProcess.Properties.Resources.tick;

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.LED_GREEN_FLASH);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = "pass";
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    _log.Info("Got green LED flash");


                    _altColour = false;
                    _activeLEDBtn = 1;
                    _flashLedBtnTimer.Change(0, 500);
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_FLASH_RED_LED);
                    break;

                case Parameters.TEST_ID_START_FLASH_RED_LED:
                    _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _altColour = true;
                    FlashLEDBtnCallback(null);
                    pictureBoxLED2.Image = ESLTestProcess.Properties.Resources.tick;

                    _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.LED_RED_FLASH);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = "pass";
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    _log.Info("Got green RED flash");

                    AllowResultsPageToMoveNext();
                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    TimeOutCallback(null);
                    break;


                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }

        private void AllowResultsPageToMoveNext()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(AllowResultsPageToMoveNext));
                return;
            }
            
            wizardPageResultsStatus.AllowNext = true;
        }

        private void wizardPageResultsStatus_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _flashColourTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageResultsStatus_ProcessResponseEventHandler;
        }

    }
}
