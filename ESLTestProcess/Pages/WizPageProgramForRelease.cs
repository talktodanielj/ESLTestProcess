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

        private void wizardPageProgramForRelease_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblProgramForRelease.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Node Id", TestViewParameters.RELEASE_NODE_ID));
                _testParameters.Add(new Tuple<string, string>("Hub Id", TestViewParameters.RELEASE_HUB_ID));

                _activeTblLayoutPanel = tblProgramForRelease;
                GenerateTable(_testParameters.ToArray());
            }

        }

        private void wizardPageProgramForRelease_Enter(object sender, EventArgs e)
        {
            AddRetestLabelToWizard(wizardPageProgramForRelease);

            var testRun = ProcessControl.Instance.GetCurrentTestRun();

            lblProgramNodeId.Text = string.Format("Node ID: {0}", testRun.pcb_unit.pcb_unit_id.ToString());
            lblSerial.Text = string.Format("Serial #: {0}", testRun.pcb_unit.pcb_unit_serial_sticker_manufacture);
            lblProgramHubId.Text = string.Format("Hub ID: {0}", ConfigurationManager.AppSettings["release_hub_id"]);

            _testParameters.Clear();
            _testParameters.Add(new Tuple<string, string>("Node Id", TestViewParameters.RELEASE_NODE_ID));
            _testParameters.Add(new Tuple<string, string>("Hub Id", TestViewParameters.RELEASE_HUB_ID));

            _activeTblLayoutPanel = tblProgramForRelease;

            ResetTestParameter(TestViewParameters.RELEASE_NODE_ID);
            ResetTestParameter(TestViewParameters.RELEASE_HUB_ID);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageProgramForRelease_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;

            cameFromFinishCommand = true;

            //Task.Run(() =>
            //{
            //    //while (true)
            //    //{

            //    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_SHUTDOWN_DUT);
            //    Thread.Sleep(500);
            //    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_PWR_DUT);
            //    //Thread.Sleep(300);
            //    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_RUN_CUR);
            //    //Thread.Sleep(300);
            //    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_START_SLEEP);
            //    //Thread.Sleep(100);
            //    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_SLEEP_CUR);
            //    //Thread.Sleep(300);
            //    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
            //    //Thread.Sleep(500);
            //    //}

            //});


        }


        private void wizardPageProgramForRelease_Leave(object sender, EventArgs e)
        {
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
            _byteStreamHandler.ProcessResponseEventHandler -= wizardPageProgramForRelease_ProcessResponseEventHandler;
            ProcessControl.Instance.SaveTestSession();
            RemoveRetestLabelFromWizard(wizardPageProgramForRelease);
        }


        private void wizardPageProgramForRelease_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            cameFromFinishCommand = false;
            wizardPageProgramForRelease_Leave(sender, e);
        }

        private void btnProgramNode_Click(object sender, EventArgs e)
        {
            _timeOutTimer.Change(5000, Timeout.Infinite);

            // Program the unit with the hub ID
            byte[] releaseHubId = ASCIIEncoding.ASCII.GetBytes(ConfigurationManager.AppSettings["release_hub_id"]);
            byte[] commandProgramHubID = TestParameters.REQUEST_SET_HUB_ID;
            commandProgramHubID[2] = releaseHubId[0];
            commandProgramHubID[3] = releaseHubId[1];
            CommunicationManager.Instance.SendCommand(commandProgramHubID);
            Thread.Sleep(100);

            // Program the unit with the node ID
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            byte[] releaseNodeId = ASCIIEncoding.ASCII.GetBytes(testRun.pcb_unit.pcb_unit_id.ToString("000"));
            byte[] commandProgramNodeID = TestParameters.REQUEST_SET_NODE_ID;
            commandProgramNodeID[2] = releaseNodeId[0];
            commandProgramNodeID[3] = releaseNodeId[1];
            commandProgramNodeID[4] = releaseNodeId[2];

            CommunicationManager.Instance.SendCommand(commandProgramNodeID);
            CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);

        }

        private string _nodeId = "";

        void wizardPageProgramForRelease_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();

            switch (e.ResponseId)
            {
                case TestParameters.PARSE_ERROR:
                    _log.Info("Got a parse error");
                    //CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                    break;

                case TestParameters.TEST_ID_BEGIN_TEST:
                    _log.Info("Got begin test command");
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_NODE_ID);
                    break;

                case TestParameters.TEST_ID_NODE_ID:
                    _nodeId = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7] }).Trim();
                    SetTestResponse(_nodeId, TestViewParameters.RELEASE_NODE_ID, e.RawData, TestStatus.Pass);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_HUB_ID);
                    break;

                case TestParameters.TEST_ID_HUB_ID:
                    string hubId = new string(new[] { (char)e.RawData[2], (char)e.RawData[3], (char)e.RawData[4], (char)e.RawData[5], (char)e.RawData[6], (char)e.RawData[7] }).Trim();
                    SetTestResponse(hubId, TestViewParameters.RELEASE_HUB_ID, e.RawData, TestStatus.Pass);
                    _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);

                    // The process is complete - mark the test
                    ProcessControl.Instance.MarkCurrentRunComplete();

                    TimeOutCallback(false);
                    break;

                default:
                    _log.Info("Unexpected test response");
                    break;
            }
        }
    }
}
