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
        private void wizardPageProgramPCB_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblPCBUnitId.RowCount == 1)
            {
                _activeTblLayoutPanel = tblPCBUnitId;

                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("PCB Id", TestViewParameters.NODE_ID));
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
            _testParameters.Add(new Tuple<string, string>("PCB Id", TestViewParameters.NODE_ID));
            _activeTblLayoutPanel = tblPCBUnitId;

            ResetTestParameter(TestViewParameters.NODE_ID);

            _byteStreamHandler.ProcessResponseEventHandler += wizardPageProgramPCB_ProcessResponseEventHandler;
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
            _timeOutTimer.Change(2000, Timeout.Infinite);

            //Task.Run(() =>
            //{
            //    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_PWR_DUT);
            //    Thread.Sleep(2000);
            //    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
            //});
        }


        void wizardPageProgramPCB_ProcessResponseEventHandler(object sender, ByteStreamHandler.ProcessResponseEventArgs e)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            response testResponse = null;

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
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_NODE_ID);

                    break;
                case TestParameters.TEST_ID_NODE_ID:
                    string nodeId = new string(new[] { (char)e.RawData[3], (char)e.RawData[2], (char)e.RawData[5], (char)e.RawData[4], (char)e.RawData[7], (char)e.RawData[6] });
                    SetTestResponse(nodeId, TestViewParameters.NODE_ID, e.RawData, TestStatus.Pass);
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_HUB_ID);
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
