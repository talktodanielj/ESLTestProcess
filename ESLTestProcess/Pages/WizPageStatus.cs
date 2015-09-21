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

        Color _originalBtnColour = Color.White;
        int _activeLEDBtn = 0;

        private void wizardPageResultsStatus_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tbllnitialStatus.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Node Id", TestViewParameters.NODE_ID));
                _testParameters.Add(new Tuple<string, string>("Hub Id", TestViewParameters.HUB_ID));
                _testParameters.Add(new Tuple<string, string>("Voltage Supply", TestViewParameters.VOLTAGE_SUPPLY));
                _testParameters.Add(new Tuple<string, string>("Battery Voltage", TestViewParameters.BATTERY_VOLTAGE));
                _testParameters.Add(new Tuple<string, string>("Temperature", TestViewParameters.TEMPERATURE_READING));
                _testParameters.Add(new Tuple<string, string>("Ext SK3 test 1", TestViewParameters.EXT_SK3_TEST1));

                _activeTblLayoutPanel = tbllnitialStatus;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageResultsStatus_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            wizardPageResultsStatus.AllowNext = false;

            AddRetestLabelToWizard(wizardPageResultsStatus);

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Node Id", TestViewParameters.NODE_ID));
            _testParameters.Add(new Tuple<string, string>("Hub Id", TestViewParameters.HUB_ID));
            _testParameters.Add(new Tuple<string, string>("Voltage Supply", TestViewParameters.VOLTAGE_SUPPLY));
            _testParameters.Add(new Tuple<string, string>("Battery Voltage", TestViewParameters.BATTERY_VOLTAGE));
            _testParameters.Add(new Tuple<string, string>("Temperature", TestViewParameters.TEMPERATURE_READING));
            _testParameters.Add(new Tuple<string, string>("Ext SK3 test 1", TestViewParameters.EXT_SK3_TEST1));

            _activeTblLayoutPanel = tbllnitialStatus;

            if (_originalBtnColour == Color.White)
                _originalBtnColour = btnLED1.BackColor;

            btnLED1.BackColor = _originalBtnColour;
            btnLED1.ForeColor = Color.Black;
            btnLED2.BackColor = _originalBtnColour;
            btnLED2.ForeColor = Color.Black;

            _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);

            ResetTestParameter(TestViewParameters.BATTERY_VOLTAGE);
            ResetTestParameter(TestViewParameters.TEMPERATURE_READING);
            ResetTestParameter(TestViewParameters.NODE_ID);
            ResetTestParameter(TestViewParameters.HUB_ID);

            pictureBoxLED1.Image = ESLTestProcess.Properties.Resources.test_spinner;
            pictureBoxLED2.Image = ESLTestProcess.Properties.Resources.test_spinner;

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageResultsStatus_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);

            _timeOutTimer.Change(15000, Timeout.Infinite);

        }

        private double _testJigVoltage = 0;
        private bool _requestingGreenLED;
        private bool _requestingRedLED;
        private bool _detectedGreenLED;
        private bool _detectedRedLED;

        void wizardPageResultsStatus_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            byte[] rawData;

            switch (e.ResponseId)
            {
                case TestParameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_NODE_ID);
                    break;

                case TestParameters.TEST_ID_NODE_ID:
                    string nodeId = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7] });
                    SetTestResponse(nodeId, TestViewParameters.NODE_ID, e.RawData, TestStatus.Pass);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_HUB_ID);
                    break;

                case TestParameters.TEST_ID_HUB_ID:
                    string hubId = new string(new[] { (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[2], (char)e.RawData[3] });
                    SetTestResponse(hubId, TestViewParameters.HUB_ID, e.RawData, TestStatus.Pass);
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_VSUPPLY_DUT);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_EXT_SK3_TEST);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC1);
                    break;

                case TestParameters.TEST_ID_EXT_SK3:
                    _log.Info("ext3");



                    break;

                case TestParameters.TESTJIG_DAC1:
                    _log.Info("dac");



                    break;

                case TestParameters.TESTJIG_VSUPPLY_DUT:

                    var result = (int)e.RawData[2];
                    //_testJigVoltage = result * 0.039215686;
                    _testJigVoltage = result * 0.02588;
                    var voltageSupplyData = string.Format("{0} V", _testJigVoltage.ToString("F"));
                    SetTestResponse(voltageSupplyData, TestViewParameters.VOLTAGE_SUPPLY, e.RawData, TestStatus.Pass);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BATTERY_LEVEL);
                    break;

                case TestParameters.TEST_ID_BATTERY_LEVEL:

                    string batLevelData = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8] });
                    int batLevel10mV = int.Parse(batLevelData);

                    double batteryLevel = batLevel10mV / 10.0;

                    if (batteryLevel < (_testJigVoltage + 0.1) && batteryLevel > (_testJigVoltage - 0.1))
                        SetTestResponse(string.Format("{0:0.00} V", batteryLevel), TestViewParameters.BATTERY_VOLTAGE, e.RawData, TestStatus.Pass);
                    else
                        SetTestResponse(string.Format("{0:0.00} V", batteryLevel), TestViewParameters.BATTERY_VOLTAGE, e.RawData, TestStatus.Fail);

                    _log.Info("Got battery level result");
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_TEMPERATURE_LEVEL);
                    break;

                case TestParameters.TEST_ID_TEMPERATURE_LEVEL:
                    rawData = e.RawData.Skip(2).Take(4).ToArray();
                    string temperatureData = new string(new[] { (char)rawData[2], (char)rawData[3] });
                    int temperature = Convert.ToInt32(temperatureData, 16);

                    _log.Info("Got temperature level");

                    SetTestResponse(string.Format("{0:0.0} C", temperature), TestViewParameters.TEMPERATURE_READING, e.RawData, TestStatus.Pass);
                    _altColour = false;
                    _activeLEDBtn = 0;
                    _flashLedBtnTimer.Change(0, 500);

                    _detectedGreenLED = false;
                    _requestingGreenLED = true;
                    _requestingRedLED = false;
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_START_FLASH_GREEN_LED);
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_LED_STATUS);
                    break;

                case TestParameters.TESTJIG_LED_STATUS:

                    _log.Info("Got LED status");

                    int ledGreenStatus = e.RawData[3];
                    int ledRedStatus = e.RawData[2];

                    if (_requestingGreenLED)
                    {
                        _detectedGreenLED = ledGreenStatus == 1;
                    }

                    if (_requestingRedLED)
                    {
                        _detectedRedLED = ledRedStatus == 1;
                    }

                    break;

                case TestParameters.TEST_ID_START_FLASH_GREEN_LED:
                    _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _altColour = true;
                    FlashLEDBtnCallback(null);

                    if (_detectedGreenLED)
                    {
                        pictureBoxLED1.Image = ESLTestProcess.Properties.Resources.tick;
                        SetTestResponse("PASS", TestViewParameters.LED_GREEN_FLASH, e.RawData, TestStatus.Pass);
                    }
                    else
                    {
                        pictureBoxLED1.Image = ESLTestProcess.Properties.Resources.cross;
                        SetTestResponse("FAIL", TestViewParameters.LED_GREEN_FLASH, e.RawData, TestStatus.Fail);
                    }

                    _altColour = false;
                    _activeLEDBtn = 1;
                    _flashLedBtnTimer.Change(0, 500);

                    _detectedRedLED = false;
                    _requestingGreenLED = false;
                    _requestingRedLED = true;

                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_START_FLASH_RED_LED);
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_LED_STATUS);

                    break;

                case TestParameters.TEST_ID_START_FLASH_RED_LED:
                    _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _altColour = true;
                    FlashLEDBtnCallback(null);
                    pictureBoxLED2.Image = ESLTestProcess.Properties.Resources.tick;

                    _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);

                    if (_detectedRedLED)
                    {
                        pictureBoxLED2.Image = ESLTestProcess.Properties.Resources.tick;
                        SetTestResponse("PASS", TestViewParameters.LED_RED_FLASH, e.RawData, TestStatus.Pass);
                    }
                    else
                    {
                        pictureBoxLED2.Image = ESLTestProcess.Properties.Resources.cross;
                        SetTestResponse("FAIL", TestViewParameters.LED_RED_FLASH, e.RawData, TestStatus.Fail);
                    }

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
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageResultsStatus);
        }

        //private void wizardPageResultsStatus_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        //{
        //    wizardPageResultsStatus_Leave(sender, e);
        //}

    }
}
