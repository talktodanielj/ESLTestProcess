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
                _testParameters.Add(new Tuple<string, string>("Voltage Supply", TestViewParameters.VOLTAGE_SUPPLY));
                _testParameters.Add(new Tuple<string, string>("Battery Voltage", TestViewParameters.BATTERY_VOLTAGE));
                _testParameters.Add(new Tuple<string, string>("Temperature", TestViewParameters.TEMPERATURE_READING));
                _testParameters.Add(new Tuple<string, string>("Ext SK3 test 1", TestViewParameters.EXT_SK3_TEST1));
                _testParameters.Add(new Tuple<string, string>("Ext SK3 test 2", TestViewParameters.EXT_SK3_TEST2));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test 1", TestViewParameters.EXT_SK5_TEST1));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test 2", TestViewParameters.EXT_SK5_TEST2));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC", TestViewParameters.EXT_SK3_TEST_ADC8));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC", TestViewParameters.EXT_SK3_TEST_ADC9));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC", TestViewParameters.EXT_SK3_TEST_ADC10));

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
            _testParameters.Add(new Tuple<string, string>("Voltage Supply", TestViewParameters.VOLTAGE_SUPPLY));
            _testParameters.Add(new Tuple<string, string>("Battery Voltage", TestViewParameters.BATTERY_VOLTAGE));
            _testParameters.Add(new Tuple<string, string>("Temperature", TestViewParameters.TEMPERATURE_READING));
            _testParameters.Add(new Tuple<string, string>("Ext SK3 test 1", TestViewParameters.EXT_SK3_TEST1));
            _testParameters.Add(new Tuple<string, string>("Ext SK3 test 2", TestViewParameters.EXT_SK3_TEST2));
            _testParameters.Add(new Tuple<string, string>("Ext SK5 test 1", TestViewParameters.EXT_SK5_TEST1));
            _testParameters.Add(new Tuple<string, string>("Ext SK5 test 2", TestViewParameters.EXT_SK5_TEST2));
            _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC 8", TestViewParameters.EXT_SK3_TEST_ADC8));
            _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC 9", TestViewParameters.EXT_SK3_TEST_ADC9));
            _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC 10", TestViewParameters.EXT_SK3_TEST_ADC10));

            _activeTblLayoutPanel = tbllnitialStatus;

            ResetTestParameter(TestViewParameters.BATTERY_VOLTAGE);
            ResetTestParameter(TestViewParameters.TEMPERATURE_READING);
            ResetTestParameter(TestViewParameters.EXT_SK3_TEST1);
            ResetTestParameter(TestViewParameters.EXT_SK3_TEST2);
            ResetTestParameter(TestViewParameters.EXT_SK5_TEST1);
            ResetTestParameter(TestViewParameters.EXT_SK5_TEST2);
            ResetTestParameter(TestViewParameters.EXT_SK3_TEST_ADC8);
            ResetTestParameter(TestViewParameters.EXT_SK3_TEST_ADC9);
            ResetTestParameter(TestViewParameters.EXT_SK3_TEST_ADC10);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageResultsStatus_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;

            CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);

            _timeOutTimer.Change(15000, Timeout.Infinite);

        }

        private double _testJigVoltage = 0;
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

                    _extHeaderTestStage = 0;
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_EXT_TEST);
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC1);

                    break;

                case TestParameters.TEST_ID_EXT:
                    _log.Info("ext");
                    if (_extHeaderTestStage == 0)
                    {
                        _extHeaderTestStage = 1;
                        byte[] requestExtSk5Test = TestParameters.REQUEST_EXT_TEST;
                        requestExtSk5Test[2] = 1; // Set the mode to 1
                        CommunicationManager.Instance.SendCommand(requestExtSk5Test);
                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC1);
                    }
                    else if (_extHeaderTestStage == 1)
                    {
                        _extHeaderTestStage = 2;
                        byte[] requestExtSkTest = TestParameters.REQUEST_EXT_TEST;
                        requestExtSkTest[2] = 2; // Set the mode to 2
                        CommunicationManager.Instance.SendCommand(requestExtSkTest);
                        Thread.Sleep(10);
                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC2);
                    }
                    else if (_extHeaderTestStage == 2)
                    {
                        _extHeaderTestStage = 3;
                        byte[] requestExtSkTest = TestParameters.REQUEST_EXT_TEST;
                        requestExtSkTest[2] = 3; // Set the mode to 3
                        CommunicationManager.Instance.SendCommand(requestExtSkTest);
                        Thread.Sleep(10);
                        CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_DAC2);
                    }
                    else if (_extHeaderTestStage == 3)
                    {
                        _extHeaderTestStage = 4;
                        byte[] requestExtSkTest = TestParameters.REQUEST_EXT_TEST;
                        requestExtSkTest[2] = 4; // Set the mode to 4. This triggers the TEST_ID_RESPONSE_EXT_SK3_ADC
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
                        var expectedSk5Test1 = int.Parse(ConfigurationManager.AppSettings["ext_sk5_test1"]);
                        int modeZeroResult = e.RawData[2];
                        if (modeZeroResult > (expectedSk5Test1 - 20) && modeZeroResult < (expectedSk5Test1 + 20))
                            SetTestResponse(modeZeroResult.ToString(), TestViewParameters.EXT_SK5_TEST1, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(modeZeroResult.ToString(), TestViewParameters.EXT_SK5_TEST1, e.RawData, TestStatus.Fail);
                    }

                    if (_extHeaderTestStage == 1)
                    {
                        int modeOneResult = e.RawData[2];
                        var expectedSk5Test2 = int.Parse(ConfigurationManager.AppSettings["ext_sk5_test2"]);
                        if (modeOneResult > (expectedSk5Test2 - 20) && modeOneResult < (expectedSk5Test2 + 20))
                            SetTestResponse(modeOneResult.ToString(), TestViewParameters.EXT_SK5_TEST2, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(modeOneResult.ToString(), TestViewParameters.EXT_SK5_TEST2, e.RawData, TestStatus.Fail);
                    }

                    break;

                case TestParameters.TESTJIG_DAC2:
                    _log.Info("dac2");

                    if (_extHeaderTestStage == 2)
                    {
                        int modeTwoResult = e.RawData[2];
                        var expectedSk5Test1 = int.Parse(ConfigurationManager.AppSettings["ext_sk3_test1"]);
                        if (modeTwoResult > (expectedSk5Test1 - 20) && modeTwoResult < (expectedSk5Test1 + 20))
                            SetTestResponse(modeTwoResult.ToString(), TestViewParameters.EXT_SK3_TEST1, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(modeTwoResult.ToString(), TestViewParameters.EXT_SK3_TEST1, e.RawData, TestStatus.Fail);
                    }

                    if (_extHeaderTestStage == 3)
                    {
                        int modeThreeResult = e.RawData[2];
                        var expectedSk5Test2 = int.Parse(ConfigurationManager.AppSettings["ext_sk3_test2"]);
                        if (modeThreeResult > (expectedSk5Test2 - 20) && modeThreeResult < (expectedSk5Test2 + 20))
                            SetTestResponse(modeThreeResult.ToString(), TestViewParameters.EXT_SK3_TEST2, e.RawData, TestStatus.Pass);
                        else
                            SetTestResponse(modeThreeResult.ToString(), TestViewParameters.EXT_SK3_TEST2, e.RawData, TestStatus.Fail);
                    }

                    break;

                case TestParameters.TEST_ID_RESPONSE_EXT_SK3_ADC:

                    string adcChannel8Data = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5] });
                    int adcChannel8 = Convert.ToInt32(adcChannel8Data, 16);
                    string adcChannel9Data = new string(new[] { (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8], (char)e.RawData[9] });
                    int adcChannel9 = Convert.ToInt32(adcChannel9Data, 16);
                    string adcChannel10Data = new string(new[] { (char)e.RawData[10], (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13] });
                    int adcChannel10 = Convert.ToInt32(adcChannel10Data, 16);


                    var expectedAdc8 = int.Parse(ConfigurationManager.AppSettings["ext_sk3_adc_channel8"]);
                    var expectedAdc9 = int.Parse(ConfigurationManager.AppSettings["ext_sk3_adc_channel9"]);
                    var expectedAdc10 = int.Parse(ConfigurationManager.AppSettings["ext_sk3_adc_channel10"]);

                    string data = adcChannel8.ToString() + "/" + adcChannel9.ToString() + "/" + adcChannel10.ToString();

                    if (adcChannel8 > (expectedAdc8 - 20) && adcChannel8 < (expectedAdc8 + 20))
                        SetTestResponse(adcChannel8.ToString(), TestViewParameters.EXT_SK3_TEST_ADC8, e.RawData, TestStatus.Pass);
                    else
                        SetTestResponse(adcChannel8.ToString(), TestViewParameters.EXT_SK3_TEST_ADC9, e.RawData, TestStatus.Fail);

                     if (adcChannel9 > (expectedAdc9 - 20) && adcChannel9 < (expectedAdc9 + 20))
                         SetTestResponse(adcChannel9.ToString(), TestViewParameters.EXT_SK3_TEST_ADC9, e.RawData, TestStatus.Pass);
                    else
                         SetTestResponse(adcChannel9.ToString(), TestViewParameters.EXT_SK3_TEST_ADC9, e.RawData, TestStatus.Fail);

                    if (adcChannel10 > (expectedAdc10 - 20) && adcChannel10 < (expectedAdc10 + 20))
                        SetTestResponse(adcChannel10.ToString(), TestViewParameters.EXT_SK3_TEST_ADC10, e.RawData, TestStatus.Pass);
                    else
                        SetTestResponse(adcChannel10.ToString(), TestViewParameters.EXT_SK3_TEST_ADC10, e.RawData, TestStatus.Fail);

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
