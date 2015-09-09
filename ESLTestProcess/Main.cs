using ESLTestProcess.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using System.Threading;

namespace ESLTestProcess
{
    public partial class Main : Form
    {

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Main()
        {
            InitializeComponent();
            _timeOutTimer = new System.Threading.Timer(TimeOutCallback);
            _flashColourTimer = new System.Threading.Timer(FlashColourCallback);
            _flashLedBtnTimer = new System.Threading.Timer(FlashLEDBtnCallback);

            if (CommunicationManager.Instance.OpenConnection())
            {
                CommunicationManager.Instance.SerialPort.DataReceived += SerialPort_DataReceived;
            }
        }

        void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string data = CommunicationManager.Instance.SerialPort.ReadExisting();

            if (data != null)
            {
                var responseData = ASCIIEncoding.ASCII.GetBytes(data.Trim());
                Console.WriteLine(BitConverter.ToString(responseData));
                _byteStreamHandler.AddToBytesQueue(responseData);
            }
        }

        private AddTechnician addTechnicianWindow = new AddTechnician();
        private System.Threading.Timer _timeOutTimer;
        private System.Threading.Timer _flashColourTimer;
        private System.Threading.Timer _flashLedBtnTimer;

        private TableLayoutPanel _activeTblLayoutPanel;
        private ByteStreamHandler _byteStreamHandler = new ByteStreamHandler();

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

        private bool _testExpired = false;

        private void TestResponseHandler(object sender, TestResponseEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<object, TestResponseEventArgs>(TestResponseHandler), new object[] { null, e });
                return;
            }

            if (_activeTblLayoutPanel != null)
            {
                var iconControl = (PictureBox)_activeTblLayoutPanel.Controls.Find(TestParameters.GetIconName(e.Parameter), true).FirstOrDefault();

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

                var valueLabel = (Label)_activeTblLayoutPanel.Controls.Find(e.Parameter, true).FirstOrDefault();
                if (valueLabel != null)
                {
                    valueLabel.Text = e.Value;
                }
            }
        }

        private void TimeOutCallback(Object state)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            // Process timed out
            // Update the display to the current state

            _testExpired = true;

            bool allPassed = true;

            if (testRun != null)
            {
                // Only check the parameters currently under test
                foreach (var parameter in _testParameters)
                {
                    var testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == parameter.Item2);

                    if (testResponse != null)
                    {
                        if ((TestStatus)testResponse.response_outcome != TestStatus.Pass)
                            allPassed = false;

                        TestResponseHandler(null, new TestResponseEventArgs
                        {
                            Parameter = testResponse.response_parameter,
                            Status = (TestStatus)testResponse.response_outcome,
                            Value = testResponse.response_value,
                            RawValue = testResponse.response_raw
                        });
                    }
                }
            }

            if (allPassed)
            {
                this.BeginInvoke(new MethodInvoker(delegate
                    {
                        stepWizardControl1.NextPage();
                    }));
            }
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
                var technicianName = addTechnicianWindow.TechnicianName.Trim();
                var existingTechnicians = DataManager.Instance.GetTechnicianNames();

                // Unique names only
                if (existingTechnicians.FirstOrDefault(s => s == technicianName) == null)
                {
                    DataManager.Instance.AddTechnician(addTechnicianWindow.TechnicianName.Trim());
                    cbTechnician.Text = "";
                    cbTechnician.SelectedIndex = -1;
                    cbTechnician.Items.Clear();
                    cbTechnician.Items.AddRange(DataManager.Instance.GetTechnicianNames());
                }
            }
        }

        private void wizardPageInsertPCB_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (txtManufactureSerial.Text.Trim().Length > 0)
                wizardPageInsertPCB.AllowNext = true;
            else
                wizardPageInsertPCB.AllowNext = false;
        }

        private void wizardPageInsertPCB_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            ProcessControl.Instance.InitialiaseTestRun(txtManufactureSerial.Text.Trim());
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

        private List<Tuple<string, string>> _testParameters = new List<Tuple<string, string>>();

        private void wizardPageAccelerometerBase_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerBasline.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestParameters.ACCELEROMETER_X_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestParameters.ACCELEROMETER_Y_BASE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestParameters.ACCELEROMETER_Z_BASE));

                _activeTblLayoutPanel = tblAccelerometerBasline;
                GenerateTable(_testParameters.ToArray());
            }

            _timeOutTimer.Change(8000, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;

            // Start the test process
            _testExpired = false;
            ProcessControl.Instance.TestBaseAccelerometerValues();
        }

        private void wizardPageAccelerometerBase_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
        }

        private void wizardPageAccelTestXY_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerXY.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestParameters.ACCELEROMETER_X_LONG_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestParameters.ACCELEROMETER_Y_LONG_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestParameters.ACCELEROMETER_Z_LONG_EDGE));

                _activeTblLayoutPanel = tblAccelerometerXY;
                GenerateTable(_testParameters.ToArray());
            }

            _timeOutTimer.Change(8000, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;

            // Start the test process
            _testExpired = false;
            ProcessControl.Instance.TestXYAccelerometerValues();
        }

        private void wizardPageAccelTestXY_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
        }

        private void wizardPageAccelTestYZ_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblAccelerometerYZ.RowCount == 1)
            {
                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("Accelerometer X", TestParameters.ACCELEROMETER_X_SHORT_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Y", TestParameters.ACCELEROMETER_Y_SHORT_EDGE));
                _testParameters.Add(new Tuple<string, string>("Accelerometer Z", TestParameters.ACCELEROMETER_Z_SHORT_EDGE));

                _activeTblLayoutPanel = tblAccelerometerYZ;
                GenerateTable(_testParameters.ToArray());
            }

            _timeOutTimer.Change(8000, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;

            // Start the test process
            _testExpired = false;
            ProcessControl.Instance.TestYZAccelerometerValues();
        }

        private void wizardPageAccelTestYZ_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
        }
        
        private void wizardPageProgramPCB_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tblPCBUnitId.RowCount == 1)
            {
                _activeTblLayoutPanel = tblPCBUnitId;

                _testParameters.Clear();
                _testParameters.Add(new Tuple<string, string>("PCB Id", TestParameters.NODE_ID));
                GenerateTable(_testParameters.ToArray());
            }


            ProcessControl.Instance.TestResponseHandler += TestResponseHandler;
            _timeOutTimer.Change(8000, Timeout.Infinite);
            // Start the test process
            _testExpired = false;
            //ProcessControl.Instance.PrepareForTestRun();

        }

        private void wizardPageProgramPCB_Leave(object sender, EventArgs e)
        {
            _timeOutTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessControl.Instance.TestResponseHandler -= TestResponseHandler;
        }

        private void txtManufactureSerial_TextChanged(object sender, EventArgs e)
        {
            if (txtManufactureSerial.Text.Trim().Length > 0)
                wizardPageInsertPCB.AllowNext = true;
            else
                wizardPageInsertPCB.AllowNext = false;
        }

        private void wizardPageProgramPCB_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

        private void wizardPageSignIn_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            ProcessControl.Instance.StartTestSession(cbTechnician.Text);
        }

        private void wizardPageResultsStatus_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

    }
}
