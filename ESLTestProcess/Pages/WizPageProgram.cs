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
        private void wizardPageProgramPCB_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblPCBUnitId.RowCount == 1)
            {
                _activeTblLayoutPanel = tblPCBUnitId;

                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Run Current", TestViewParameters.RUN_CURRENT));
                _testParameters.Add(new Tuple<string, string>("Sleep Current", TestViewParameters.SLEEP_CURRENT));
                _testParameters.Add(new Tuple<string, string>("Test Node Id", TestViewParameters.NODE_ID));
                _testParameters.Add(new Tuple<string, string>("Test Hub Id", TestViewParameters.HUB_ID));

                GenerateTable(_testParameters.ToArray());
            }

            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            _timeOutTimer.Change(8000, Timeout.Infinite);
            // Start the test process
            _testExpired = false;
            //ProcessControl.Instance.PrepareForTestRun();
        }

        private void wizardPageProgramPCB_Enter(object sender, EventArgs e)
        {
            _testExpired = false;
            AddRetestLabelToWizard(wizardPageProgramPCB);
            stepWizardControl1.SelectedPage.AllowNext = false;

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Run Current", TestViewParameters.RUN_CURRENT));
            _testParameters.Add(new Tuple<string, string>("Sleep Current", TestViewParameters.SLEEP_CURRENT));
            _testParameters.Add(new Tuple<string, string>("Test Node Id", TestViewParameters.NODE_ID));
            _testParameters.Add(new Tuple<string, string>("Test Hub Id", TestViewParameters.HUB_ID));
            _activeTblLayoutPanel = tblPCBUnitId;

            ResetTestParameter(TestViewParameters.NODE_ID);
            ResetTestParameter(TestViewParameters.HUB_ID);
            ResetTestParameter(TestViewParameters.RUN_CURRENT);
            ResetTestParameter(TestViewParameters.SLEEP_CURRENT);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageProgramPCB_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
            _timeOutTimer.Change(10000, Timeout.Infinite);

            Task.Run(() =>
            {
                CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_SHUTDOWN_DUT);
                Thread.Sleep(1000);
                CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_PWR_DUT);
                Thread.Sleep(3000);
                CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_RUN_CUR);
            });
        }


        void wizardPageProgramPCB_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
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

                    // Program the unit with the test ID
                    byte[] testNodeId = ASCIIEncoding.ASCII.GetBytes(ConfigurationManager.AppSettings["test_node_id"]);

                    byte[] commandProgramNodeID = TestParameters.REQUEST_SET_NODE_ID;

                    commandProgramNodeID[2] = testNodeId[0];
                    commandProgramNodeID[3] = testNodeId[1];
                    commandProgramNodeID[4] = testNodeId[2];

                    CommunicationManager.Instance.SendCommand(commandProgramNodeID);
                    break;

                case TestParameters.TESTJIG_RUN_CUR:
                    _log.Info("Got run current command");
                    var result = (int)e.RawData[2];
                    // Multiply by conversion factor to get current in mV
                    double current = result * 0.294117647;
                    var currentData = string.Format("{0} mA", current.ToString("F"));

                    double runCurrentMax = double.Parse(ConfigurationManager.AppSettings["run_current_max"]);
                    double runCurrentMin = double.Parse(ConfigurationManager.AppSettings["run_current_min"]);

                    if(current <= runCurrentMax && current >= runCurrentMin)
                        SetTestResponse(currentData, TestViewParameters.RUN_CURRENT, e.RawData, TestStatus.Pass);
                    else
                        SetTestResponse(currentData, TestViewParameters.RUN_CURRENT, e.RawData, TestStatus.Fail);

                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_START_SLEEP);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_SLEEP_CUR);
                    break;

                case TestParameters.TESTJIG_SLEEP_CUR:

                    var sleepCurrentResponse = (int)e.RawData[2];

                    double sleepCurrent = sleepCurrentResponse * (129.4 / 1000);

                    var sleepCurrentData = string.Format("{0} uA", sleepCurrent.ToString("F"));

                    double sleepCurrentMax = double.Parse(ConfigurationManager.AppSettings["sleep_current_max"]);
                    double sleepCurrentMin = double.Parse(ConfigurationManager.AppSettings["sleep_current_min"]);

                    if (sleepCurrent <= sleepCurrentMax && sleepCurrent >= sleepCurrentMin)
                        SetTestResponse(sleepCurrentData, TestViewParameters.SLEEP_CURRENT, e.RawData, TestStatus.Pass);
                    else
                        SetTestResponse(sleepCurrentData, TestViewParameters.SLEEP_CURRENT, e.RawData, TestStatus.Fail);

                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_SLEEP:

                    var sleepResponse = (int)e.RawData[2];

                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_SET_NODE_ID:

                    // Now request the node id back
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_NODE_ID);

                    break;

                case TestParameters.TEST_ID_NODE_ID:

                    // Value should be the same as what was set initially
                    string nodeId = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7] });

                    int nodeIdForTest = int.Parse(ConfigurationManager.AppSettings["test_node_id"]);
                    int actualNodeId = int.Parse(nodeId);

                    if (nodeIdForTest == actualNodeId)
                        SetTestResponse(nodeId, TestViewParameters.NODE_ID, e.RawData, TestStatus.Pass);
                    else
                        SetTestResponse(nodeId, TestViewParameters.NODE_ID, e.RawData, TestStatus.Fail);

                    // Now do the same for the hub Id
                    // Program the unit with the test ID
                    byte[] testHubId = ASCIIEncoding.ASCII.GetBytes(ConfigurationManager.AppSettings["test_hub_id"]);

                    byte[] commandProgramHubID = TestParameters.REQUEST_SET_HUB_ID;

                    commandProgramHubID[2] = testHubId[0];
                    commandProgramHubID[3] = testHubId[1];

                    CommunicationManager.Instance.SendCommand(commandProgramHubID);

                    break;

                case TestParameters.TEST_ID_SET_HUB_ID:
                    // Now request the hub id back
                    Thread.Sleep(100);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_HUB_ID);
                    break;

                case TestParameters.TEST_ID_HUB_ID:

                    // Value should be the same as what was set initially
                    string hubId = new string(new[] { (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[2], (char)e.RawData[3] });

                    int hubIdForTest = int.Parse(ConfigurationManager.AppSettings["test_node_id"]);
                    int actualHubId = int.Parse(hubId);

                    if (hubIdForTest == actualHubId)
                        SetTestResponse(hubId, TestViewParameters.HUB_ID, e.RawData, TestStatus.Pass);
                    else
                        SetTestResponse(hubId, TestViewParameters.HUB_ID, e.RawData, TestStatus.Fail);

                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    Thread.Sleep(300);
                    TimeOutCallback(null);
                    break;


                case TestParameters.TEST_END:
                    _log.Info("Got test end");

                    break;
                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }

        private void wizardPageProgramPCB_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageProgramPCB_ProcessResponseEventHandler;
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageProgramPCB);
        }

        private void wizardPageProgramPCB_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

        private void wizardPageProgramPCB_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            wizardPageProgramPCB_Leave(sender, e);
        }

    }
}
