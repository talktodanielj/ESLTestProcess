using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                _testParameters.Add(new Tuple<string, string>("Ext SK3 test 2", TestViewParameters.EXT_SK3_TEST2));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test 1", TestViewParameters.EXT_SK5_TEST1));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test 2", TestViewParameters.EXT_SK5_TEST2));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC", TestViewParameters.EXT_SK5_TEST_ADC));

                _activeTblLayoutPanel = tbllnitialStatus;
                GenerateTable(_testParameters.ToArray());

                // Add the LED parameters after the table has been generated
                _testParameters.Add(new Tuple<string, string>("Red LED", TestViewParameters.LED_RED_FLASH));
                _testParameters.Add(new Tuple<string, string>("Green LED", TestViewParameters.LED_GREEN_FLASH));

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
            _testParameters.Add(new Tuple<string, string>("Ext SK3 test 2", TestViewParameters.EXT_SK3_TEST2));
            _testParameters.Add(new Tuple<string, string>("Ext SK5 test 1", TestViewParameters.EXT_SK5_TEST1));
            _testParameters.Add(new Tuple<string, string>("Ext SK5 test 2", TestViewParameters.EXT_SK5_TEST2));
            _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC", TestViewParameters.EXT_SK5_TEST_ADC));
            _testParameters.Add(new Tuple<string, string>("Red LED", TestViewParameters.LED_RED_FLASH));
            _testParameters.Add(new Tuple<string, string>("Green LED", TestViewParameters.LED_GREEN_FLASH));

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
            ResetTestParameter(TestViewParameters.EXT_SK3_TEST1);
            ResetTestParameter(TestViewParameters.EXT_SK3_TEST2);
            ResetTestParameter(TestViewParameters.EXT_SK5_TEST1);
            ResetTestParameter(TestViewParameters.EXT_SK5_TEST2);
            ResetTestParameter(TestViewParameters.EXT_SK5_TEST_ADC);
            ResetTestParameter(TestViewParameters.LED_GREEN_FLASH);
            ResetTestParameter(TestViewParameters.LED_RED_FLASH);

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

        private int _extHeaderTestStage = 0;

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

                    _extHeaderTestStage = 0;
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_EXT_SK3_TEST);
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC1);
                    break;

                case TestParameters.TEST_ID_EXT_SK3:
                    _log.Info("ext3");
                    if (_extHeaderTestStage == 0)
                    {
                        _extHeaderTestStage = 1;
                        byte[] requestExtSk3Test = TestParameters.REQUEST_EXT_SK3_TEST;
                        requestExtSk3Test[2] = 1; // Set the mode to 1
                        CommunicationManager.Instance.SendCommand(requestExtSk3Test);
                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC1);
                    }
                    else if (_extHeaderTestStage == 1)
                    {
                        _extHeaderTestStage = 2;
                        byte[] requestExtSkTest = TestParameters.REQUEST_EXT_SK3_TEST;
                        requestExtSkTest[2] = 2; // Set the mode to 2
                        CommunicationManager.Instance.SendCommand(requestExtSkTest);
                        Thread.Sleep(10);
                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC2);
                    }
                    else if (_extHeaderTestStage == 2)
                    {
                        _extHeaderTestStage = 3;
                        byte[] requestExtSkTest = TestParameters.REQUEST_EXT_SK3_TEST;
                        requestExtSkTest[2] = 3; // Set the mode to 3
                        CommunicationManager.Instance.SendCommand(requestExtSkTest);
                        Thread.Sleep(10);
                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC2);
                    }
                    else if (_extHeaderTestStage == 3)
                    {
                        _extHeaderTestStage = 4;
                        byte[] requestExtSkTest = TestParameters.REQUEST_EXT_SK3_TEST;
                        requestExtSkTest[2] = 4; // Set the mode to 4
                        CommunicationManager.Instance.SendCommand(requestExtSkTest);
                    }
                    else
                    {
                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_VSUPPLY_DUT);
                    }

                    break;

                case TestParameters.TESTJIG_DAC1:
                    _log.Info("dac1");

                    if (_extHeaderTestStage == 0)
                    {
                        var expectedSk3Test1 = int.Parse(ConfigurationManager.AppSettings["ext_sk3_test1"]);
                        int modeZeroResult = e.RawData[2];
                        if (modeZeroResult > (expectedSk3Test1 - 20) && modeZeroResult < (expectedSk3Test1 + 20))
                            SetTestResponse(modeZeroResult.ToString(), TestViewParameters.EXT_SK3_TEST1, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(modeZeroResult.ToString(), TestViewParameters.EXT_SK3_TEST1, e.RawData, TestStatus.Fail);
                    }

                    if (_extHeaderTestStage == 1)
                    {
                        int modeOneResult = e.RawData[2];
                        var expectedSk3Test2 = int.Parse(ConfigurationManager.AppSettings["ext_sk3_test2"]);
                        if (modeOneResult > (expectedSk3Test2 - 20) && modeOneResult < (expectedSk3Test2 + 20))
                            SetTestResponse(modeOneResult.ToString(), TestViewParameters.EXT_SK3_TEST2, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(modeOneResult.ToString(), TestViewParameters.EXT_SK3_TEST2, e.RawData, TestStatus.Fail);
                    }

                    break;

                case TestParameters.TESTJIG_DAC2:
                    _log.Info("dac2");

                    if (_extHeaderTestStage == 2)
                    {
                        int modeTwoResult = e.RawData[2];
                        var expectedSk5Test1 = int.Parse(ConfigurationManager.AppSettings["ext_sk5_test1"]);
                        if (modeTwoResult > (expectedSk5Test1 - 20) && modeTwoResult < (expectedSk5Test1 + 20))
                            SetTestResponse(modeTwoResult.ToString(), TestViewParameters.EXT_SK5_TEST1, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(modeTwoResult.ToString(), TestViewParameters.EXT_SK5_TEST1, e.RawData, TestStatus.Fail);
                    }

                    if (_extHeaderTestStage == 3)
                    {
                        int modeThreeResult = e.RawData[2];
                        var expectedSk5Test2 = int.Parse(ConfigurationManager.AppSettings["ext_sk5_test2"]);
                        if (modeThreeResult > (expectedSk5Test2 - 20) && modeThreeResult < (expectedSk5Test2 + 20))
                            SetTestResponse(modeThreeResult.ToString(), TestViewParameters.EXT_SK5_TEST2, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(modeThreeResult.ToString(), TestViewParameters.EXT_SK5_TEST2, e.RawData, TestStatus.Fail);
                    }

                    break;

                case TestParameters.TEST_ID_RESPONSE_EXT_SK5_ADC:

                    string adcChannel8Data = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5] });
                    int adcChannel8 = Convert.ToInt32(adcChannel8Data, 16);
                    string adcChannel9Data = new string(new[] { (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8], (char)e.RawData[9] });
                    int adcChannel9 = Convert.ToInt32(adcChannel9Data, 16);
                    string adcChannel10Data = new string(new[] { (char)e.RawData[10], (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13] });
                    int adcChannel10 = Convert.ToInt32(adcChannel10Data, 16);


                    var expectedAdc8 = int.Parse(ConfigurationManager.AppSettings["ext_sk5_adc_channel8"]);
                    var expectedAdc9 = int.Parse(ConfigurationManager.AppSettings["ext_sk5_adc_channel9"]);
                    var expectedAdc10 = int.Parse(ConfigurationManager.AppSettings["ext_sk5_adc_channel10"]);

                    string data = adcChannel8.ToString() + "/" + adcChannel9.ToString() + "/" + adcChannel10.ToString();

                    if ((adcChannel8 > (expectedAdc8 - 20) && adcChannel8 < (expectedAdc8 + 20))
                        && (adcChannel9 > (expectedAdc9 - 20) && adcChannel9 < (expectedAdc9 + 20))
                        && (adcChannel10 > (expectedAdc10 - 20) && adcChannel10 < (expectedAdc10 + 20)))
                        SetTestResponse(data, TestViewParameters.EXT_SK5_TEST_ADC, e.RawData, TestStatus.Pass);
                    else
                        SetTestResponse(data, TestViewParameters.EXT_SK5_TEST_ADC, e.RawData, TestStatus.Fail);

                    break;

                case TestParameters.TESTJIG_VSUPPLY_DUT:

                    var result = (int)e.RawData[2];
                    _testJigVoltage = result * 0.02588;
                    var voltageSupplyData = string.Format("{0} V", _testJigVoltage.ToString("F"));
                    SetTestResponse(voltageSupplyData, TestViewParameters.VOLTAGE_SUPPLY, e.RawData, TestStatus.Pass);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BATTERY_LEVEL);
                    break;

                case TestParameters.TEST_ID_BATTERY_LEVEL:

                    string batLevelData = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8] });
                    int batLevel10mV = int.Parse(batLevelData);

                    double batteryLevel = batLevel10mV / 10.0;

                    double voltageDiffernce = double.Parse(ConfigurationManager.AppSettings["voltage_difference"]);

                    if (batteryLevel < (_testJigVoltage + voltageDiffernce) && batteryLevel > (_testJigVoltage - voltageDiffernce))
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

                    double temperatureMax = double.Parse(ConfigurationManager.AppSettings["temperature_max"]);
                    double temperatureMin = double.Parse(ConfigurationManager.AppSettings["temperature_min"]);

                    if (temperature >= temperatureMin && temperature <= temperatureMax)
                        SetTestResponse(string.Format("{0:0.0} C", temperature), TestViewParameters.TEMPERATURE_READING, e.RawData, TestStatus.Pass);
                    else
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
                                        
                    int ledGreenStatus = e.RawData[2];
                    int ledRedStatus = e.RawData[3];

                    if (_requestingGreenLED)
                    {
                        _detectedGreenLED = ledGreenStatus == 0;
                    }

                    if (_requestingRedLED)
                    {
                        _detectedRedLED = ledRedStatus == 0;
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
