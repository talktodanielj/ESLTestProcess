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

                _testParameters.Add(new Tuple<string, string>("Piezo test", TestViewParameters.PIEZO_TEST));
                _testParameters.Add(new Tuple<string, string>("Reed test", TestViewParameters.REED_TEST));
                _testParameters.Add(new Tuple<string, string>("Set RTC", TestViewParameters.RTC_SET));
                _testParameters.Add(new Tuple<string, string>("Get RTC", TestViewParameters.RTC_GET));

                _testParameters.Add(new Tuple<string, string>("Key ENT", TestViewParameters.KEY_ENT));
                _testParameters.Add(new Tuple<string, string>("Key 1/6", TestViewParameters.KEY_1_6));
                _testParameters.Add(new Tuple<string, string>("Key 2/7", TestViewParameters.KEY_2_7));
                _testParameters.Add(new Tuple<string, string>("Key 3/8", TestViewParameters.KEY_3_8));
                _testParameters.Add(new Tuple<string, string>("Key 4/9", TestViewParameters.KEY_4_9));
                _testParameters.Add(new Tuple<string, string>("Key 5/0", TestViewParameters.KEY_5_0));

                _activeTblLayoutPanel = tblSummaryPart1;
                GenerateTable(_testParameters.ToArray());
            }
        }


        private void wizardPageSummaryPart1_Enter(object sender, EventArgs e)
        {
            _activeTblLayoutPanel = tblSummaryPart1;
            AddRetestLabelToWizard(wizardPageSummaryPart1);

            Task.Run(() =>
            {
                Thread.Sleep(2000);
                if (!_rollbackFromSummaryPage2)
                    TimeOutCallback(null);
            });
        }

        private void wizardPageSummaryPart1_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

    }
}
