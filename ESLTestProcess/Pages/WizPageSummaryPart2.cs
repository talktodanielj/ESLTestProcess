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

                _testParameters.Add(new Tuple<string, string>("Background RSSI", TestParameters.RF_BGR_RSSI));
                _testParameters.Add(new Tuple<string, string>("HUB Acknowledgment", TestParameters.RF_HUB_ACK));
                _testParameters.Add(new Tuple<string, string>("Ack RSSI value", TestParameters.RF_ACK_RSSI));

                _testParameters.Add(new Tuple<string, string>("Accelerometer Base X", TestParameters.ACCELEROMETER_X_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Base Y", TestParameters.ACCELEROMETER_Y_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Base Z", TestParameters.ACCELEROMETER_Z_BASE));

                _testParameters.Add(new Tuple<string, string>("Accelerometer Long Edge X", TestParameters.ACCELEROMETER_X_LONG_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Long Edge Y", TestParameters.ACCELEROMETER_Y_LONG_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Long Edge Z", TestParameters.ACCELEROMETER_Z_LONG_EDGE));

                _testParameters.Add(new Tuple<string, string>("Accelerometer Short Edge X", TestParameters.ACCELEROMETER_X_SHORT_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Short Edge Y", TestParameters.ACCELEROMETER_Y_SHORT_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Short Edge Z", TestParameters.ACCELEROMETER_Z_SHORT_EDGE));

                _activeTblLayoutPanel = tblSummaryPart2;
                GenerateTable(_testParameters.ToArray());
            }
        }


        private void wizardPageSummaryPart2_Enter(object sender, EventArgs e)
        {
            _activeTblLayoutPanel = tblSummaryPart2;
            TimeOutCallback(null);
        }


    }
}
