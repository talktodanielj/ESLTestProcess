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
        private void wizardPageProgramForRelease_Enter(object sender, EventArgs e)
        {
            AddRetestLabelToWizard(wizardPageProgramForRelease);
            _rollBackFromProgramPage = false;

            var testRun = ProcessControl.Instance.GetCurrentTestRun();

            lblProgramNodeId.Text =  string.Format("Node ID: {0}", testRun.pcb_unit.pcb_unit_id.ToString());
            lblSerial.Text = string.Format("Serial #: {0}", testRun.pcb_unit.pcb_unit_serial_sticker_manufacture);
            lblProgramHubId.Text = string.Format("Hub ID: {0}", ConfigurationManager.AppSettings["release_hub_id"]);

            cameFromFinishCommand = true;
        }

        private void wizardPageProgramForRelease_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
        }

        private void wizardPageProgramForRelease_Leave(object sender, EventArgs e)
        {
            RemoveRetestLabelFromWizard(wizardPageProgramForRelease);
        }

        private bool _rollBackFromProgramPage = false;

        private void wizardPageProgramForRelease_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            _rollBackFromProgramPage = true;
            wizardPageProgramForRelease_Leave(sender, e);
        }
    }
}
