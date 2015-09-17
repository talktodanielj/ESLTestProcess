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
using System.Configuration;

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
            else
            {
                MessageBox.Show(string.Format("Failed to open COM port {0}", ConfigurationManager.AppSettings["serial_port"]), "Test Jig Connection Error", MessageBoxButtons.OK);
            }

            this.KeyPreview = true;
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
                , new PictureBox() { Image = ESLTestProcess.Properties.Resources.test_spinner, Anchor = AnchorStyles.None, Size = new Size(24, 24), SizeMode = PictureBoxSizeMode.StretchImage, Name = TestViewParameters.GetIconName(parameter.Item2) });
            }

            _activeTblLayoutPanel.AutoSize = true;
            _activeTblLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _activeTblLayoutPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            _activeTblLayoutPanel.ResumeLayout();
            _activeTblLayoutPanel.PerformLayout();


            int postion = (this.stepWizardControl1.SelectedPage.Width - _activeTblLayoutPanel.Width) / 2;
            _activeTblLayoutPanel.Location = new Point(postion, _activeTblLayoutPanel.Location.Y);
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
                var iconControl = (PictureBox)_activeTblLayoutPanel.Controls.Find(TestViewParameters.GetIconName(e.Parameter), true).FirstOrDefault();

                if (iconControl == null)
                {
                    if (e.Parameter == TestViewParameters.LED_GREEN_FLASH)
                    {
                        iconControl = pictureBoxLED1;
                    }

                    if (e.Parameter == TestViewParameters.LED_RED_FLASH)
                    {
                        iconControl = pictureBoxLED2;
                    }
                }

                if (iconControl != null)
                {
                    switch (e.Status)
                    {
                        case TestStatus.Unknown:
                            if (_testExpired)
                            {
                                iconControl.Image = ESLTestProcess.Properties.Resources.question;
                                iconControl.Image.Tag = "question";
                            }
                            else
                            {
                                // If the spinner is currently active don't change the image
                                // or else the animation gets reset
                                if (iconControl.Image.Tag != "test_spinner")
                                    iconControl.Image = ESLTestProcess.Properties.Resources.test_spinner;
                                iconControl.Image.Tag = "test_spinner";
                            }
                            break;
                        case TestStatus.Fail:
                            iconControl.Image = ESLTestProcess.Properties.Resources.cross;
                            iconControl.Image.Tag = "cross";
                            break;
                        case TestStatus.Warning:
                            iconControl.Image = ESLTestProcess.Properties.Resources.alert;
                            iconControl.Image.Tag = "alert";
                            break;
                        case TestStatus.Pass:
                            iconControl.Image = ESLTestProcess.Properties.Resources.tick;
                            iconControl.Image.Tag = "tick";
                            break;
                        default:
                            iconControl.Image = ESLTestProcess.Properties.Resources.question;
                            iconControl.Image.Tag = "question";
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
                            //RawValue = testResponse.response_raw
                        });
                    }
                }
            }

            this.BeginInvoke(new MethodInvoker(delegate
            {
                stepWizardControl1.SelectedPage.AllowNext = true;
            }));

            if (allPassed)
            {
                this.BeginInvoke(new MethodInvoker(delegate
                    {
                        stepWizardControl1.NextPage();
                    }));
            }
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



        private void txtManufactureSerial_TextChanged(object sender, EventArgs e)
        {
            if (txtManufactureSerial.Text.Trim().Length > 0)
                wizardPageInsertPCB.AllowNext = true;
            else
                wizardPageInsertPCB.AllowNext = false;
        }


        private void wizardPageSignIn_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {
            ProcessControl.Instance.StartTestSession(cbTechnician.Text);
        }

        private void wizardPageResultsStatus_Commit(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

        private void Main_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_activeTblLayoutPanel == tblAccelerometerBaseline)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Space) && !_accelerometerBaseTestRunning)
                {
                    _accelerometerBaseTestRunning = true;
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                }
            }

            if (_activeTblLayoutPanel == tblAccelerometerStep1)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Space) && !_accelerometerTestStep1Running)
                {
                    _accelerometerTestStep1Running = true;
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                }
            }

            if (_activeTblLayoutPanel == tblAccelerometerStep2)
            {
                if (e.KeyChar == Convert.ToChar(Keys.Space) && !_accelerometerTestStep2Running)
                {
                    _accelerometerTestStep2Running = true;
                    CommunicationManager.Instance.SendCommand(TestParameters.REQUEST_BEGIN_TEST);
                }
            }
        }

        private void ResetTestParameter(string testParameter)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            if (testRun != null)
            {
                var testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == testParameter);
                if (testResponse != null)
                {
                    testResponse.response_outcome = (Int16)TestStatus.Unknown;
                    testResponse.response_raw = "";
                    testResponse.response_value = "Unknown";

                    TestResponseHandler(null, new TestResponseEventArgs
                    {
                        Parameter = testResponse.response_parameter,
                        Status = (TestStatus)testResponse.response_outcome,
                        Value = testResponse.response_value,
                        //RawValue = testResponse.response_raw
                    });
                }
            }
        }

        private void SetTestResponse(string dataString, string responseKey, byte[] rawData, TestStatus testStatus)
        {
            var testRun = ProcessControl.Instance.GetCurrentTestRun();
            response testResponse = null;

            testResponse = testRun.responses.FirstOrDefault(r => r.response_parameter == responseKey);
            testResponse.response_raw = BitConverter.ToString(rawData);
            testResponse.response_value = dataString;
            testResponse.response_outcome = (Int16)testStatus;

            TestResponseHandler(null, new TestResponseEventArgs
            {
                Parameter = testResponse.response_parameter,
                Status = (TestStatus)testResponse.response_outcome,
                Value = testResponse.response_value,
                //RawValue = testResponse.response_raw
            });
        }

        private void btnSaveTest_Click(object sender, EventArgs e)
        {
            ProcessControl.Instance.SaveTestSession();
        }


        Label _retestLabel = null;

        private void AddRetestLabelToWizard(AeroWizard.WizardPage wizardPage)
        {
            if (_retestLabel == null)
            {
                _retestLabel = new Label();
                _retestLabel.Name = "lblRetest";
                _retestLabel.BackColor = Color.LightGoldenrodYellow;
                _retestLabel.ForeColor = Color.Red;
                _retestLabel.Text = "RETEST";
                _retestLabel.Font = new Font(_retestLabel.Font, FontStyle.Bold);
                _retestLabel.TextAlign = ContentAlignment.MiddleCenter;
                _retestLabel.Location = new Point(wizardPage.Width - 100, _retestLabel.Location.Y);
            }

            if (ProcessControl.Instance.IsRetest)
            {
                if (!wizardPage.Contains(_retestLabel))
                    wizardPage.Controls.Add(_retestLabel);
                _retestLabel.Visible = true;
            }
            else
            {
                _retestLabel.Visible = false;
            }
        }

        private void RemoveRetestLabelFromWizard(AeroWizard.WizardPage wizardPage)
        {
            if (!wizardPage.Contains(_retestLabel))
                wizardPage.Controls.Remove(_retestLabel);
        }

        private void stepWizardControl1_Cancelling(object sender, CancelEventArgs e)
        {
            Application.Exit();
        }

        private void stepWizardControl1_Finished(object sender, EventArgs e)
        {
            stepWizardControl1.NextPage(wizardPageInsertPCB);
        }

        bool cameFromFinishCommand = false;

        private void wizardPageInsertPCB_Enter(object sender, EventArgs e)
        {
            if (cameFromFinishCommand)
            {
                cameFromFinishCommand = false;
                txtManufactureSerial.Text = "";
            }
        }

        private void exportResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.CreatePrompt = false;
            saveDialog.CheckFileExists = false;
            saveDialog.CheckPathExists = true;
            saveDialog.DefaultExt = "csv";
            saveDialog.OverwritePrompt = true;
            saveDialog.Title = "Export test results as CSV file";

            var diaogResult = saveDialog.ShowDialog();

            if (diaogResult == System.Windows.Forms.DialogResult.OK)
            {
                DataManager.Instance.ExportTestData(saveDialog.FileName);
            }
        }

        private void wizardPageSignIn_Rollback(object sender, AeroWizard.WizardPageConfirmEventArgs e)
        {

        }

        private void btnProgramNode_Click(object sender, EventArgs e)
        {
            var currentRun = ProcessControl.Instance.GetCurrentTestRun().run_complete = true;
            ProcessControl.Instance.SaveTestSession();
           
        }






















    }
}
