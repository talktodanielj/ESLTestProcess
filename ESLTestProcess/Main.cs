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
        private TableLayoutPanel _activeTblLayoutPanel;

        private void GenerateTable(Tuple<string, string>[] parameters)
        {
            _activeTblLayoutPanel.SuspendLayout();

            // Clear out the existing controls, we are generating a new table layout
            _activeTblLayoutPanel.Controls.Clear();

            _activeTblLayoutPanel.ColumnStyles.Clear();
            _activeTblLayoutPanel.RowStyles.Clear();
            _activeTblLayoutPanel.ColumnCount = 3;

            // Add the columns
            _activeTblLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
            _activeTblLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
            _activeTblLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));

            // Add the rows
            AddRow(
                  new Label() { Text = "Test:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(_activeTblLayoutPanel.Font, FontStyle.Bold) }
                , new Label() { Text = "Value:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(_activeTblLayoutPanel.Font, FontStyle.Bold) }
                , new Label() { Text = "Status:", Anchor = AnchorStyles.Left, AutoSize = true, Font = new Font(_activeTblLayoutPanel.Font, FontStyle.Bold) });

            foreach (Tuple<string, string> parameter in parameters)
            {
                AddRow(
                  new Label() { Text = parameter.Item1, Anchor = AnchorStyles.Left, AutoSize = true }
                , new Label() { Text = "Unknown", Anchor = AnchorStyles.Left, AutoSize = true, Name = parameter.Item2 }
                , new PictureBox() { Image = ESLTestProcess.Properties.Resources.test_spinner, Anchor = AnchorStyles.None, Size = new Size(24, 24), SizeMode = PictureBoxSizeMode.StretchImage, Name = TestParameters.GetIconName(parameter.Item2) });
            }

            _activeTblLayoutPanel.AutoSize = true;
            _activeTblLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _activeTblLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            _activeTblLayoutPanel.ResumeLayout();
            _activeTblLayoutPanel.PerformLayout();

        }

        private void AddRow(Control label, Control value, Control status)
        {
            int rowIndex = AddTableRow();
            _activeTblLayoutPanel.Controls.Add(label, 0, rowIndex);
            _activeTblLayoutPanel.Controls.Add(value, 1, rowIndex);
            _activeTblLayoutPanel.Controls.Add(status, 2, rowIndex);
        }

        private int AddTableRow()
        {
            int index = _activeTblLayoutPanel.RowCount++;
            RowStyle style = new RowStyle(SizeType.AutoSize);
            _activeTblLayoutPanel.RowStyles.Add(style);
            return index;
        }


        
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

            bool allPassed = false;

            if (testRun != null)
            {
                foreach (response response in testRun.responses)
                {
                    if ((TestStatus)response.response_outcome != TestStatus.Pass)
                        allPassed = false;

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = response.response_parameter,
                        Status = (TestStatus)response.response_outcome,
                        Value = response.response_value,
                        RawValue = response.response_raw
                    });
                }
            }

            if (allPassed)
                stepWizardControl1.NextPage();
        }

        private void wizardPageResultsStatus_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tbllnitialStatus.RowCount == 1)
            {
                List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();
                parameters.Add(new Tuple<string, string>("EPROM Id", TestParameters.EPROM_ID));
                parameters.Add(new Tuple<string, string>("Accelerometer ID", TestParameters.ACCELEROMETER_ID));
                parameters.Add(new Tuple<string, string>("PIC24 Id", TestParameters.PIC24_ID));
                parameters.Add(new Tuple<string, string>("Transceveier ID", TestParameters.TRANSCEVEIER_ID));
                parameters.Add(new Tuple<string, string>("Battery Volatge", TestParameters.BATTERY_VOLTAGE));
                parameters.Add(new Tuple<string, string>("Temperature", TestParameters.TEMPERATURE_READING));

                _activeTblLayoutPanel = tbllnitialStatus;
                GenerateTable(parameters.ToArray());
            }

            _timeOutTimer = new System.Threading.Timer(TimerOutCallback, this, 10000, 0);
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;

            // Start the test process
            _testExpired = false;
            ProcessControl.Instance.TestGetIntialStatus();
        }

        private void wizardPageResultsStatus_Leave(object sender, EventArgs e)
        {
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
        }

        private bool _testExpired = false;

        private void TestResponseHandler(object sender, TestResponseEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, TestResponseEventArgs>(TestResponseHandler), new object[] { null, e });
                return;
            }

           var iconControl = (PictureBox)tbllnitialStatus.Controls.Find(TestParameters.GetIconName(e.Parameter), true).FirstOrDefault();

            if (iconControl != null)
            {
                switch (e.Status)
                {
                    case TestStatus.Unknown:
                        if (_testExpired)
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

        private void wizardPageAccelerometerBase_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerBasline.RowCount == 1)
            {
                List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();
                parameters.Add(new Tuple<string, string>("Accelerometer X", TestParameters.ACCELEROMETER_X_BASE));
                parameters.Add(new Tuple<string, string>("Accelerometer Y", TestParameters.ACCELEROMETER_Y_BASE));
                parameters.Add(new Tuple<string, string>("Accelerometer Z", TestParameters.ACCELEROMETER_Z_BASE));

                _activeTblLayoutPanel = tblAccelerometerBasline;
                GenerateTable(parameters.ToArray());
            }
        }

        private void wizardPageAccelTestXY_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerXY.RowCount == 1)
            {
                List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();
                parameters.Add(new Tuple<string, string>("Accelerometer X", TestParameters.ACCELEROMETER_X_LONG_EDGE));
                parameters.Add(new Tuple<string, string>("Accelerometer Y", TestParameters.ACCELEROMETER_Y_LONG_EDGE));
                parameters.Add(new Tuple<string, string>("Accelerometer Z", TestParameters.ACCELEROMETER_Z_LONG_EDGE));

                _activeTblLayoutPanel = tblAccelerometerXY;
                GenerateTable(parameters.ToArray());
            }
        }

        private void wizardPageAccelTestYZ_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerYZ.RowCount == 1)
            {
                List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();
                parameters.Add(new Tuple<string, string>("Accelerometer X", TestParameters.ACCELEROMETER_X_SHORT_EDGE));
                parameters.Add(new Tuple<string, string>("Accelerometer Y", TestParameters.ACCELEROMETER_Y_SHORT_EDGE));
                parameters.Add(new Tuple<string, string>("Accelerometer Z", TestParameters.ACCELEROMETER_Z_SHORT_EDGE));

                _activeTblLayoutPanel = tblAccelerometerYZ;
                GenerateTable(parameters.ToArray());
            }
        }

        private void wizardPageTransceveier_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblTransceveierTest.RowCount == 1)
            {
                List<Tuple<string, string>> parameters = new List<Tuple<string, string>>();
                parameters.Add(new Tuple<string, string>("Message Sent", TestParameters.TRANS_MSG_TX));
                parameters.Add(new Tuple<string, string>("Receveied Response", TestParameters.TRANS_MSG_RX));
                parameters.Add(new Tuple<string, string>("RSSI value", TestParameters.TRANS_RSSI));

                _activeTblLayoutPanel = tblTransceveierTest;
                GenerateTable(parameters.ToArray());
            }
        }

    }
}
