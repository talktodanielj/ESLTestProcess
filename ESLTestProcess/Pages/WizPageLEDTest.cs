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
        private void wizardPageLEDTest_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tbllnitialStatus.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Firmware version", TestViewParameters.FIRMWARE_VERSION));
                _testParameters.Add(new Tuple<string, string>("Node Id", TestViewParameters.NODE_ID));
                _testParameters.Add(new Tuple<string, string>("Hub Id", TestViewParameters.HUB_ID));

                _activeTblLayoutPanel = tblLEDTest;
                GenerateTable(_testParameters.ToArray());

                // Add the LED parameters after the table has been generated
                _testParameters.Add(new Tuple<string, string>("Red LED", TestViewParameters.LED_RED_FLASH));
                _testParameters.Add(new Tuple<string, string>("Green LED", TestViewParameters.LED_GREEN_FLASH));
            }
        }

        private void wizardPageLEDTest_Enter(object sender, EventArgs e)
        {

            _testExpired = false;
            wizardPageLEDTest.AllowNext = false;

            AddRetestLabelToWizard(wizardPageLEDTest);

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Firmware version", TestViewParameters.FIRMWARE_VERSION));
            _testParameters.Add(new Tuple<string, string>("Node Id", TestViewParameters.NODE_ID));
            _testParameters.Add(new Tuple<string, string>("Hub Id", TestViewParameters.HUB_ID));
            _testParameters.Add(new Tuple<string, string>("Red LED", TestViewParameters.LED_RED_FLASH));
            _testParameters.Add(new Tuple<string, string>("Green LED", TestViewParameters.LED_GREEN_FLASH));


            if (_originalBtnColour == Color.White)
                _originalBtnColour = btnLED1.BackColor;

            btnLED1.BackColor = _originalBtnColour;
            btnLED1.ForeColor = Color.Black;
            btnLED2.BackColor = _originalBtnColour;
            btnLED2.ForeColor = Color.Black;

            _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);

            ResetTestParameter(TestViewParameters.FIRMWARE_VERSION);
            ResetTestParameter(TestViewParameters.NODE_ID);
            ResetTestParameter(TestViewParameters.HUB_ID);
            ResetTestParameter(TestViewParameters.LED_GREEN_FLASH);
            ResetTestParameter(TestViewParameters.LED_RED_FLASH);

            pictureBoxLED1.Image = ESLTestProcess.Properties.Resources.test_spinner;
            pictureBoxLED2.Image = ESLTestProcess.Properties.Resources.test_spinner;

            Task.Run(() =>
                {

                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_SHUTDOWN_DUT);
                    Thread.Sleep(500);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_PWR_DUT);
                    Thread.Sleep(3000);
                    _byteStreamHandler.ProcessResponseEventHandler += wizardPageLEDTest_ProcessResponseEventHandler;
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                });
            
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            _timeOutTimer.Change(16000, Timeout.Infinite);
        }

        private bool _requestingGreenLED;
        private bool _requestingRedLED;
        private bool _detectedGreenLED;
        private bool _detectedRedLED;


        void wizardPageLEDTest_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            byte[] rawData;

            switch (e.ResponseId)
            {
                case TestParameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");

                    string firmwareVersion = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], (char)e.RawData[8], (char)e.RawData[9], (char)e.RawData[10], (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13], (char)e.RawData[14], (char)e.RawData[15], (char)e.RawData[16]}).Trim();
                    firmwareVersion = firmwareVersion.Replace("\0", "");

                    SetTestResponse(firmwareVersion, TestViewParameters.FIRMWARE_VERSION, e.RawData, TestStatus.Pass);



                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_NODE_ID);
                    break;

                case TestParameters.TEST_ID_NODE_ID:
                    string nodeId = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7] }).Trim();
                    SetTestResponse(nodeId, TestViewParameters.NODE_ID, e.RawData, TestStatus.Pass);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_HUB_ID);
                    break;

                case TestParameters.TEST_ID_HUB_ID:
                    string hubId = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7] }).Trim();
                    SetTestResponse(hubId, TestViewParameters.HUB_ID, e.RawData, TestStatus.Pass);

                    _altColour = false;
                    _activeLEDBtn = 0;
                    _flashLedBtnTimer.Change(0, 500);

                    _detectedGreenLED = false;
                    _requestingGreenLED = true;
                    _requestingRedLED = false;

                    // Need to do this or the LEDs dont work ???? Ask RS about the test jig
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_SHUTDOWN_DUT);
                    //Thread.Sleep(200);
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_PWR_DUT);
                    //Thread.Sleep(2000);
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

                    // Need to do this or the LEDs dont work ???? Ask RS about the test jig
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_SHUTDOWN_DUT);
                    //Thread.Sleep(200);
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_PWR_DUT);
                    //Thread.Sleep(2000);
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

        private void wizardPageLEDTest_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _flashColourTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _flashLedBtnTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageLEDTest_ProcessResponseEventHandler;
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageLEDTest);
        }


    }
}
