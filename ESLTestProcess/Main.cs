using ESLTesProcess.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ESLTestProcess
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private AddTechnician addTechnicianWindow = new AddTechnician();


        private void stepWizardControl1_SelectedPageChanged(object sender, EventArgs e)
        {

        }

        private void btnAddTechnician_Click(object sender, EventArgs e)
        {
            var dialogResult = addTechnicianWindow.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                ProcessControl.Instance.AddTechnician(addTechnicianWindow.TechnicianName);
                cbTechnician.Text = "";
                cbTechnician.SelectedIndex = -1;
                cbTechnician.Items.Clear();
                cbTechnician.Items.AddRange(ProcessControl.Instance.GetTechnicianNames());
            }
        }

        private void wizardPageInsertPCB_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

        private void cbTechnician_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTechnician.SelectedIndex > -1)
                wizardPageSignIn.AllowNext = true;
        }

        private void wizardPageSignIn_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            cbTechnician.Items.Clear();
            cbTechnician.Items.AddRange(ProcessControl.Instance.GetTechnicianNames());
        }

        private void wizardPageResultsStatus_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            AddRow(
                  new Label() { Text = "Test:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(tbllnitialStatus.Font, FontStyle.Bold) }
                , new Label() { Text = "Value:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(tbllnitialStatus.Font, FontStyle.Bold) }
                , new Label() { Text = "Status:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(tbllnitialStatus.Font, FontStyle.Bold) });

        }


        private void AddRow(Control label, Control value, Control status)
        {
            int rowIndex = AddTableRow();
            tbllnitialStatus.Controls.Add(label, 0, rowIndex);
            tbllnitialStatus.Controls.Add(value, 1, rowIndex);
            tbllnitialStatus.Controls.Add(status, 2, rowIndex);
        }

        private int AddTableRow()
        {
            int index = tbllnitialStatus.RowCount++;
            RowStyle style = new RowStyle(SizeType.AutoSize);
            tbllnitialStatus.RowStyles.Add(style);
            return index;
        }

    }
}
