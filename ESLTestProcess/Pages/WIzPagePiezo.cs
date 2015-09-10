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
        private void wizardPagePiezo_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblPiezoPanel.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Piezo test", TestParameters.PIEZO_TEST));
                _testParameters.Add(new Tuple<string, string>("Reed test", TestParameters.REED_TEST));
                _testParameters.Add(new Tuple<string, string>("Set RTC", TestParameters.RTC_SET));
                _testParameters.Add(new Tuple<string, string>("Get RTC", TestParameters.RTC_GET));

                _activeTblLayoutPanel = tblPiezoPanel;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPagePiezo_Enter(object sender, EventArgs e)
        {
            _testExpired = false;

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Piezo test", TestParameters.PIEZO_TEST));
            _testParameters.Add(new Tuple<string, string>("Reed test", TestParameters.REED_TEST));
            _testParameters.Add(new Tuple<string, string>("Set RTC", TestParameters.RTC_SET));
            _testParameters.Add(new Tuple<string, string>("Get RTC", TestParameters.RTC_GET));

            _activeTblLayoutPanel = tblPiezoPanel;

            _gotPiezoTestResult = false;
            _piezoTestRetries = 0;

            _gotReedTestResult = false;
            _reedTestRetries = 0;
            
            ResetTestParameter(TestParameters.PIEZO_TEST);
            ResetTestParameter(TestParameters.REED_TEST);
            ResetTestParameter(TestParameters.RTC_SET);
            ResetTestParameter(TestParameters.RTC_GET);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPagePiezo_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
            _timeOutTimer.Change(10000, Timeout.Infinite);
        }

        DateTime _rtcDateTime;
        string _rtcDateTimeString = "";

        bool _gotPiezoTestResult = false;
        int _piezoTestRetries = 3;
        bool _gotReedTestResult = false;
        int _reedTestRetries = 3;


        void wizardPagePiezo_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
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
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_PIEZO_TEST);
                    break;
                case Parameters.TEST_END:
                    _log.Info("Got test end");

                    if (!_gotPiezoTestResult && _piezoTestRetries < 3)
                    {
                        _piezoTestRetries++;
                        CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_PIEZO_TEST);
                    }
                    else if (!_gotReedTestResult && _reedTestRetries < 3)
                    {
                        _reedTestRetries++;
                        CommunicationManager.Instance.SendCommand(Parameters.REQUEST_REED_SWITCH_TEST);
                    }
                    else
                    {
                        RequestSetRTC();
                    }
                    break;

                case Parameters.TEST_ID_START_PIEZO_TEST:

                    _gotPiezoTestResult = true;

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.PIEZO_TEST);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = "PASS";
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_REED_SWITCH_TEST);
                    break;

                case Parameters.TEST_ID_REED_SWITCH_TEST:

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.REED_TEST);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = "PASS";
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });

                    // Next test...
                    RequestSetRTC();

                    break;

                case Parameters.TEST_ID_SET_RTC_VALUE:

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.RTC_SET);
                    testResponse.response_raw = e.RawData;
                    _rtcDateTimeString =_rtcDateTime.ToString("dd/MM/yy HH:mm:ss");
                    testResponse.response_value = _rtcDateTimeString;
                    testResponse.response_outcome = (Int16)TestStatus.Pass;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_RTC_VALUE);
                    break;

                case Parameters.TEST_ID_RTC_VALUE:

                    var temperatureResponseValue = new string(new []{(char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], 
                                                             (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7], 
                                                             (char)e.RawData[8], (char)e.RawData[9], (char)e.RawData[10], 
                                                             (char)e.RawData[11], (char)e.RawData[12], (char)e.RawData[13], 
                                                             (char)e.RawData[14], (char)e.RawData[15], (char)e.RawData[16], 
                                                             (char)e.RawData[17], (char)e.RawData[18]});

                    var temperatureResponse = (Int16)TestStatus.Unknown;

                    if(_rtcDateTimeString.CompareTo(temperatureResponseValue) == 0)
                        temperatureResponse = (Int16)TestStatus.Pass;
                    else
                        temperatureResponse = (Int16)TestStatus.Fail;

                    testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == TestParameters.RTC_GET);
                    testResponse.response_raw = e.RawData;
                    testResponse.response_value = temperatureResponseValue;
                    testResponse.response_outcome = temperatureResponse;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        RawValue = testResponse.response_raw
                    });
                    
                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    TimeOutCallback(null);

                    break;
                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }

        private void RequestSetRTC()
        {
            _rtcDateTime = DateTime.Now;

            // Get the current date and time and set the RTC
            byte[] commandBytes = Parameters.REQUEST_SET_RTC_VALUE;

            commandBytes[2] = TestHelper.ToBcd(_rtcDateTime.Year - 2000)[0];
            commandBytes[3] = TestHelper.ToBcd(_rtcDateTime.Month)[0];
            commandBytes[4] = TestHelper.ToBcd(_rtcDateTime.Day)[0];
            commandBytes[5] = TestHelper.ToBcd(_rtcDateTime.Hour)[0];
            commandBytes[6] = TestHelper.ToBcd(_rtcDateTime.Minute)[0];
            commandBytes[7] = TestHelper.ToBcd(_rtcDateTime.Second)[0];

            CommunicationManager.Instance.SendCommand(commandBytes);
        }



        private void wizardPagePiezo_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPagePiezo_ProcessResponseEventHandler;
        }
    }
}
