using ESLTestProcess.Data;
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
        private System.Threading.Timer _timeOutTimer;


        private void stepWizardControl1_SelectedPageChanged(object sender, EventArgs e)
        {

        }

        private void btnAddTechnician_Click(object sender, EventArgs e)
        {
            addTechnicianWindow.TechnicianName = "";
            var dialogResult = addTechnicianWindow.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                DataManager.Instance.AddTechnician(addTechnicianWindow.TechnicianName);
                cbTechnician.Text = "";
                cbTechnician.SelectedIndex = -1;
                cbTechnician.Items.Clear();
                cbTechnician.Items.AddRange(DataManager.Instance.GetTechnicianNames());
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
            cbTechnician.Items.AddRange(DataManager.Instance.GetTechnicianNames());
        }

        private void TimerOutCallback(Object state)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            // Process timed out
            // Update the display to the current state

            _testExpired = true;
            if (testRun != null)
            {
                foreach (response response in testRun.responses)
                {
                    Instance_TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = response.response_parameter,
                        Status = (TestStatus)response.response_outcome,
                        Value = response.response_value,
                        RawValue = response.response_raw
                    });
                }
            }
        }

        private void wizardPageResultsStatus_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tbllnitialStatus.RowCount == 1)
            {
                GenerateTable();
            }

            _timeOutTimer = new System.Threading.Timer(TimerOutCallback, this, 10000, 0);   
            ProcessControl.Instance.TestResponseHandler += Instance_TestResponseHandler;

            // Start the test process
            _testExpired = false;
            ProcessControl.Instance.TestGetIntialStatus();
        }

        private void wizardPageResultsStatus_Leave(object sender, EventArgs e)
        {
            ProcessControl.Instance.TestResponseHandler -= Instance_TestResponseHandler;
        }

        private bool _testExpired = false;

        private void Instance_TestResponseHandler(object sender, TestResponseEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, TestResponseEventArgs>(Instance_TestResponseHandler), new object[] { null, e });
                return;
            }
            

            var iconControl = (PictureBox)tbllnitialStatus.Controls.Find(TestParameters.GetIconName(e.Parameter), true).FirstOrDefault();

            if (iconControl != null)
            {
                switch (e.Status)
                {
                    case TestStatus.Unknown:
                        if(_testExpired)
                            iconControl.Image = ESLTestProcess.Properties.Resources.question;
                        else
                            iconControl.Image = ESLTestProcess.Properties.Resources.test_spinner;
                        break;
                    case TestStatus.Fail:
                        iconControl.Image = ESLTestProcess.Properties.Resources.cross;
                        break;
                    case TestStatus.Warning:
                        iconControl.Image = ESLTestProcess.Properties.Resources.alert;
                        break;
                    case TestStatus.Pass:
                        iconControl.Image = ESLTestProcess.Properties.Resources.tick;
                        break;
                    default:
                        iconControl.Image = ESLTestProcess.Properties.Resources.question;
                        break;
                }
            }

            var valueLabel = (Label)tbllnitialStatus.Controls.Find(e.Parameter, true).FirstOrDefault();
            if (valueLabel != null)
            {
                valueLabel.Text = e.Value;
            }

        }

        private void GenerateTable()
        {
            tbllnitialStatus.SuspendLayout();

            // Clear out the existing controls, we are generating a new table layout
            tbllnitialStatus.Controls.Clear();

            tbllnitialStatus.ColumnStyles.Clear();
            tbllnitialStatus.RowStyles.Clear();
            tbllnitialStatus.ColumnCount = 3;

            // Add the columns
            tbllnitialStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
            tbllnitialStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
            tbllnitialStatus.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));

            // Add the rows
            AddRow(
                  new Label() { Text = "Test:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(tbllnitialStatus.Font, FontStyle.Bold) }
                , new Label() { Text = "Value:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(tbllnitialStatus.Font, FontStyle.Bold) }
                , new Label() { Text = "Status:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(tbllnitialStatus.Font, FontStyle.Bold) });

            AddRow(
                  new Label() { Text = "EPROM Id", Anchor = AnchorStyles.Left, AutoSize = true }
                , new Label() { Text = "Unknown", Anchor = AnchorStyles.Left, AutoSize = true, Name = TestParameters.EPROM_ID }
                , new PictureBox() { Image = ESLTestProcess.Properties.Resources.test_spinner, Anchor = AnchorStyles.None, Size = new Size(24, 24), SizeMode = PictureBoxSizeMode.StretchImage, Name = TestParameters.GetIconName(TestParameters.EPROM_ID) });
            AddRow(
                  new Label() { Text = "Accelerometer ID", Anchor = AnchorStyles.Left, AutoSize = true }
                , new Label() { Text = "Unknown", Anchor = AnchorStyles.Left, AutoSize = true, Name = TestParameters.ACCELEROMETER_ID }
                , new PictureBox() { Image = ESLTestProcess.Properties.Resources.test_spinner, Anchor = AnchorStyles.None, Size = new Size(24, 24), SizeMode = PictureBoxSizeMode.StretchImage, Name = TestParameters.GetIconName(TestParameters.ACCELEROMETER_ID) });
            AddRow(
                  new Label() { Text = "PIC24 Id", Anchor = AnchorStyles.Left, AutoSize = true }
                , new Label() { Text = "Unknown", Anchor = AnchorStyles.Left, AutoSize = true, Name = TestParameters.PIC24_ID }
                , new PictureBox() { Image = ESLTestProcess.Properties.Resources.test_spinner, Anchor = AnchorStyles.None, Size = new Size(24, 24), SizeMode = PictureBoxSizeMode.StretchImage, Name = TestParameters.GetIconName(TestParameters.PIC24_ID) });
            AddRow(
                  new Label() { Text = "Transceveier ID", Anchor = AnchorStyles.Left, AutoSize = true }
                , new Label() { Text = "Unknown", Anchor = AnchorStyles.Left, AutoSize = true, Name = TestParameters.TRANSCEVEIER_ID }
                , new PictureBox() { Image = ESLTestProcess.Properties.Resources.test_spinner, Anchor = AnchorStyles.None, Size = new Size(24, 24), SizeMode = PictureBoxSizeMode.StretchImage, Name = TestParameters.GetIconName(TestParameters.TRANSCEVEIER_ID) });
            AddRow(
                  new Label() { Text = "Battery Volatge", Anchor = AnchorStyles.Left, AutoSize = true }
                , new Label() { Text = "Unknown", Anchor = AnchorStyles.Left, AutoSize = true, Name= TestParameters.BATTERY_VOLTAGE }
                , new PictureBox() { Image = ESLTestProcess.Properties.Resources.test_spinner, Anchor = AnchorStyles.None, Size = new Size(24, 24), SizeMode = PictureBoxSizeMode.StretchImage, Name = TestParameters.GetIconName(TestParameters.BATTERY_VOLTAGE)});
            AddRow(
                 new Label() { Text = "Temperature", Anchor = AnchorStyles.Left, AutoSize = true }
                , new Label() { Text = "Unknown", Anchor = AnchorStyles.Left, AutoSize = true, Name = TestParameters.TEMPERATURE_READING }
                , new PictureBox() { Image = ESLTestProcess.Properties.Resources.test_spinner, Anchor = AnchorStyles.None, Size = new Size(24, 24), SizeMode = PictureBoxSizeMode.StretchImage, Name = TestParameters.GetIconName(TestParameters.TEMPERATURE_READING) });

            tbllnitialStatus.AutoSize = true;
            tbllnitialStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tbllnitialStatus.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tbllnitialStatus.ResumeLayout();
            tbllnitialStatus.PerformLayout();

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
