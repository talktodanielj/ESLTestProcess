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
            _activeTblLayoutPanel = tblPiezoPanel;

            ResetTestParameter(TestParameters.PIEZO_TEST);
            ResetTestParameter(TestParameters.REED_TEST);
            ResetTestParameter(TestParameters.RTC_SET);
            ResetTestParameter(TestParameters.RTC_GET);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPagePiezo_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
        }

        void wizardPagePiezo_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            response testResponse = null;

            switch (e.ResponseId)
            {
                case Parameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_BEGIN_TEST);
                    break;

                case Parameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    CommunicationManager.Instance.SendCommand(Parameters.REQUEST_START_PIEZO_TEST);
                    break;

                case Parameters.TEST_ID_START_PIEZO_TEST:

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
                    //CommunicationManager.Instance.SendCommand(Parameters.REQUEST_REED_SWITCH_TEST);
                    break;


                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }

        private void wizardPagePiezo_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPagePiezo_ProcessResponseEventHandler;
        }
    }
}
