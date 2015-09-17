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
        private void wizardPageSummaryPart2_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblSummaryPart2.RowCount == 1)
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

                _activeTblLayoutPanel = tblSummaryPart2;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageSummaryPart2_Enter(object sender, EventArgs e)
        {
            AddRetestLabelToWizard(wizardPageSummaryPart2);
            _activeTblLayoutPanel = tblSummaryPart2;
            _rollbackFromSummaryPage2 = false;
            Task.Run(() =>
            {
                Thread.Sleep(2000);
                if(!_rollBackFromProgramPage)
                    TimeOutCallback(null);
            });
        }

        private bool _rollbackFromSummaryPage2;
        private void wizardPageSummaryPart2_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            _rollbackFromSummaryPage2 = true;
        }

    }
}
