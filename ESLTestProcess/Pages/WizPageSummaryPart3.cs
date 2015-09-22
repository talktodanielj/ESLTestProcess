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
        private void wizardPageSummaryPart3_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblSummaryPart3.RowCount == 1)
            {
                _testParameters.Clear();

                _testParameters.Add(new Tuple<string, string>("Background RSSI", TestViewParameters.RF_BGR_RSSI));
                _testParameters.Add(new Tuple<string, string>("HUB Acknowledgment", TestViewParameters.RF_HUB_ACK));
                _testParameters.Add(new Tuple<string, string>("Ack RSSI value", TestViewParameters.RF_ACK_RSSI));

                _testParameters.Add(new Tuple<string, string>("Accelerometer Base X", TestViewParameters.ACCELEROMETER_X_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Base Y", TestViewParameters.ACCELEROMETER_Y_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Base Z", TestViewParameters.ACCELEROMETER_Z_BASE));

                _testParameters.Add(new Tuple<string, string>("Accelerometer Long Edge X", TestViewParameters.ACCELEROMETER_X_LONG_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Long Edge Y", TestViewParameters.ACCELEROMETER_Y_LONG_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Long Edge Z", TestViewParameters.ACCELEROMETER_Z_LONG_EDGE));

                _testParameters.Add(new Tuple<string, string>("Accelerometer Short Edge X", TestViewParameters.ACCELEROMETER_X_SHORT_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Short Edge Y", TestViewParameters.ACCELEROMETER_Y_SHORT_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Short Edge Z", TestViewParameters.ACCELEROMETER_Z_SHORT_EDGE));

                _activeTblLayoutPanel = tblSummaryPart3;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageSummaryPart3_Enter(object sender, EventArgs e)
        {
            AddRetestLabelToWizard(wizardPageSummaryPart3);
            _activeTblLayoutPanel = tblSummaryPart3;

            var testRun = ProcessControl.Instance.GetCurrentTestRun();

            //if (!DataManager.Instance.AllTestsPassed(testRun.run_id, testRun.pcb_unit.pcb_unit_id))
            //{
            //    stepWizardControl1.SelectedPage.AllowNext = false;
            //    stepWizardControl1.SelectedPage.IsFinishPage = true;
            //}
            //else
            //{
            //    stepWizardControl1.SelectedPage.AllowNext = true;
            //    stepWizardControl1.SelectedPage.IsFinishPage = false;
            //}

            TimeOutCallback(false);
            
        }

        private void wizardPageSummaryPart3_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
        }

    }
}
