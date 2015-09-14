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
            lblProgramNodeId.Text = string.Format("Node ID: {0}", ConfigurationManager.AppSettings["test_node_id"]);
            lblProgramNodeId.Text = string.Format("Hub ID: {0}", ConfigurationManager.AppSettings["test_hub_id"]);
        }

        private void wizardPageProgramForRelease_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {

        }

    }
}
