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
        private void wizardPageSummaryPart1_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblSummaryPart1.RowCount == 1)
            {
                _testParameters.Clear();

                _testParameters.Add(new Tuple<string, string>("Node Id", TestViewParameters.NODE_ID));
                _testParameters.Add(new Tuple<string, string>("Hub Id", TestViewParameters.HUB_ID));
                _testParameters.Add(new Tuple<string, string>("Battery Volatge", TestViewParameters.BATTERY_VOLTAGE));
                _testParameters.Add(new Tuple<string, string>("Temperature", TestViewParameters.TEMPERATURE_READING));
                _testParameters.Add(new Tuple<string, string>("Ext SK3 test 1", TestViewParameters.EXT_SK3_TEST1));
                _testParameters.Add(new Tuple<string, string>("Ext SK3 test 2", TestViewParameters.EXT_SK3_TEST2));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test 1", TestViewParameters.EXT_SK5_TEST1));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test 2", TestViewParameters.EXT_SK5_TEST2));
                _testParameters.Add(new Tuple<string, string>("Ext SK5 test ADC", TestViewParameters.EXT_SK5_TEST_ADC));
                _testParameters.Add(new Tuple<string, string>("Red LED", TestViewParameters.LED_RED_FLASH));
                _testParameters.Add(new Tuple<string, string>("Green LED", TestViewParameters.LED_GREEN_FLASH));

                _activeTblLayoutPanel = tblSummaryPart1;
                GenerateTable(_testParameters.ToArray());
            }
        }

        private void wizardPageSummaryPart1_Enter(object sender, EventArgs e)
        {
            _activeTblLayoutPanel = tblSummaryPart1;
            AddRetestLabelToWizard(wizardPageSummaryPart1);
            TimeOutCallback(false);
        }

        private void wizardPageSummaryPart1_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

    }
}
