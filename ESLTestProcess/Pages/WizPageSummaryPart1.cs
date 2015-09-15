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

                _testParameters.Add(new Tuple<string, string>("Node Id", TestParameters.NODE_ID));
                _testParameters.Add(new Tuple<string, string>("Hub Id", TestParameters.HUB_ID));
                _testParameters.Add(new Tuple<string, string>("Battery Volatge", TestParameters.BATTERY_VOLTAGE));
                _testParameters.Add(new Tuple<string, string>("Temperature", TestParameters.TEMPERATURE_READING));

                _testParameters.Add(new Tuple<string, string>("Piezo test", TestParameters.PIEZO_TEST));
                _testParameters.Add(new Tuple<string, string>("Reed test", TestParameters.REED_TEST));
                _testParameters.Add(new Tuple<string, string>("Set RTC", TestParameters.RTC_SET));
                _testParameters.Add(new Tuple<string, string>("Get RTC", TestParameters.RTC_GET));

                _testParameters.Add(new Tuple<string, string>("Key ENT", TestParameters.KEY_ENT));
                _testParameters.Add(new Tuple<string, string>("Key 1/6", TestParameters.KEY_1_6));
                _testParameters.Add(new Tuple<string, string>("Key 2/7", TestParameters.KEY_2_7));
                _testParameters.Add(new Tuple<string, string>("Key 3/8", TestParameters.KEY_3_8));
                _testParameters.Add(new Tuple<string, string>("Key 4/9", TestParameters.KEY_4_9));
                _testParameters.Add(new Tuple<string, string>("Key 5/0", TestParameters.KEY_5_0));
                
                _activeTblLayoutPanel = tblSummaryPart1;
                GenerateTable(_testParameters.ToArray());
            }
        }


        private void wizardPageSummaryPart1_Enter(object sender, EventArgs e)
        {
            _activeTblLayoutPanel = tblSummaryPart1;
            AddRetestLabelToWizard(wizardPageSummaryPart1);
            Thread.Sleep(2000);
            TimeOutCallback(null);
        }
    }
}
