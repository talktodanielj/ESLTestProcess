namespace ESLTestProcess
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.stepWizardControl1 = new AeroWizard.StepWizardControl();
            this.wizardPageKeyPress = new AeroWizard.WizardPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn5_0 = new System.Windows.Forms.Button();
            this.btn4_9 = new System.Windows.Forms.Button();
            this.btn3_8 = new System.Windows.Forms.Button();
            this.btn2_7 = new System.Windows.Forms.Button();
            this.btn1_6 = new System.Windows.Forms.Button();
            this.btnEnt = new System.Windows.Forms.Button();
            this.wizardPageResultsStatus = new AeroWizard.WizardPage();
            this.themedLabel12 = new AeroWizard.ThemedLabel();
            this.tbllnitialStatus = new System.Windows.Forms.TableLayoutPanel();
            this.wizardPageSignIn = new AeroWizard.WizardPage();
            this.themedLabel7 = new AeroWizard.ThemedLabel();
            this.btnAddTechnician = new System.Windows.Forms.Button();
            this.themedLabel2 = new AeroWizard.ThemedLabel();
            this.cbTechnician = new System.Windows.Forms.ComboBox();
            this.themedLabel1 = new AeroWizard.ThemedLabel();
            this.wizardPageInsertPCB = new AeroWizard.WizardPage();
            this.themedLabel6 = new AeroWizard.ThemedLabel();
            this.themedLabel5 = new AeroWizard.ThemedLabel();
            this.txtManufactureSerial = new System.Windows.Forms.TextBox();
            this.themedLabel4 = new AeroWizard.ThemedLabel();
            this.themedLabel3 = new AeroWizard.ThemedLabel();
            this.wizardPageProgramPCB = new AeroWizard.WizardPage();
            this.tblPCBUnitId = new System.Windows.Forms.TableLayoutPanel();
            this.themedLabel11 = new AeroWizard.ThemedLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.themedLabel10 = new AeroWizard.ThemedLabel();
            this.themedLabel9 = new AeroWizard.ThemedLabel();
            this.themedLabel8 = new AeroWizard.ThemedLabel();
            this.wizardPageAccelerometerBase = new AeroWizard.WizardPage();
            this.tblAccelerometerBasline = new System.Windows.Forms.TableLayoutPanel();
            this.themedLabel13 = new AeroWizard.ThemedLabel();
            this.wizardPageAccelTestXY = new AeroWizard.WizardPage();
            this.themedLabel14 = new AeroWizard.ThemedLabel();
            this.tblAccelerometerXY = new System.Windows.Forms.TableLayoutPanel();
            this.wizardPageAccelTestYZ = new AeroWizard.WizardPage();
            this.tblAccelerometerYZ = new System.Windows.Forms.TableLayoutPanel();
            this.themedLabel15 = new AeroWizard.ThemedLabel();
            this.wizardPageTransceiver = new AeroWizard.WizardPage();
            this.themedLabel16 = new AeroWizard.ThemedLabel();
            this.tblTransceiverTest = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).BeginInit();
            this.wizardPageKeyPress.SuspendLayout();
            this.panel1.SuspendLayout();
            this.wizardPageResultsStatus.SuspendLayout();
            this.wizardPageSignIn.SuspendLayout();
            this.wizardPageInsertPCB.SuspendLayout();
            this.wizardPageProgramPCB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.wizardPageAccelerometerBase.SuspendLayout();
            this.wizardPageAccelTestXY.SuspendLayout();
            this.wizardPageAccelTestYZ.SuspendLayout();
            this.wizardPageTransceiver.SuspendLayout();
            this.SuspendLayout();
            // 
            // stepWizardControl1
            // 
            this.stepWizardControl1.Location = new System.Drawing.Point(0, 0);
            this.stepWizardControl1.Name = "stepWizardControl1";
            this.stepWizardControl1.Pages.Add(this.wizardPageSignIn);
            this.stepWizardControl1.Pages.Add(this.wizardPageInsertPCB);
            this.stepWizardControl1.Pages.Add(this.wizardPageProgramPCB);
            this.stepWizardControl1.Pages.Add(this.wizardPageResultsStatus);
            this.stepWizardControl1.Pages.Add(this.wizardPageKeyPress);
            this.stepWizardControl1.Pages.Add(this.wizardPageAccelerometerBase);
            this.stepWizardControl1.Pages.Add(this.wizardPageAccelTestXY);
            this.stepWizardControl1.Pages.Add(this.wizardPageAccelTestYZ);
            this.stepWizardControl1.Pages.Add(this.wizardPageTransceiver);
            this.stepWizardControl1.Size = new System.Drawing.Size(806, 568);
            this.stepWizardControl1.StepListFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.stepWizardControl1.StepListWidth = 250;
            this.stepWizardControl1.TabIndex = 0;
            this.stepWizardControl1.Title = "ESL test process for trap node PCB";
            // 
            // wizardPageKeyPress
            // 
            this.wizardPageKeyPress.Controls.Add(this.panel1);
            this.wizardPageKeyPress.Name = "wizardPageKeyPress";
            this.wizardPageKeyPress.NextPage = this.wizardPageResultsStatus;
            this.wizardPageKeyPress.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageKeyPress, "Key press test");
            this.wizardPageKeyPress.TabIndex = 10;
            this.wizardPageKeyPress.Text = "Key press test";
            this.wizardPageKeyPress.Enter += new System.EventHandler(this.wizardPageKeyPress_Enter);
            this.wizardPageKeyPress.Leave += new System.EventHandler(this.wizardPageKeyPress_Leave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LimeGreen;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btn5_0);
            this.panel1.Controls.Add(this.btn4_9);
            this.panel1.Controls.Add(this.btn3_8);
            this.panel1.Controls.Add(this.btn2_7);
            this.panel1.Controls.Add(this.btn1_6);
            this.panel1.Controls.Add(this.btnEnt);
            this.panel1.Location = new System.Drawing.Point(95, 84);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(307, 238);
            this.panel1.TabIndex = 6;
            // 
            // btn5_0
            // 
            this.btn5_0.BackColor = System.Drawing.Color.ForestGreen;
            this.btn5_0.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn5_0.ForeColor = System.Drawing.Color.White;
            this.btn5_0.Location = new System.Drawing.Point(200, 127);
            this.btn5_0.Name = "btn5_0";
            this.btn5_0.Size = new System.Drawing.Size(75, 71);
            this.btn5_0.TabIndex = 8;
            this.btn5_0.Text = "5/0";
            this.btn5_0.UseVisualStyleBackColor = false;
            // 
            // btn4_9
            // 
            this.btn4_9.BackColor = System.Drawing.Color.ForestGreen;
            this.btn4_9.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4_9.ForeColor = System.Drawing.Color.White;
            this.btn4_9.Location = new System.Drawing.Point(118, 127);
            this.btn4_9.Name = "btn4_9";
            this.btn4_9.Size = new System.Drawing.Size(75, 71);
            this.btn4_9.TabIndex = 7;
            this.btn4_9.Text = "4/9";
            this.btn4_9.UseVisualStyleBackColor = false;
            // 
            // btn3_8
            // 
            this.btn3_8.BackColor = System.Drawing.Color.ForestGreen;
            this.btn3_8.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3_8.ForeColor = System.Drawing.Color.White;
            this.btn3_8.Location = new System.Drawing.Point(35, 127);
            this.btn3_8.Name = "btn3_8";
            this.btn3_8.Size = new System.Drawing.Size(75, 71);
            this.btn3_8.TabIndex = 6;
            this.btn3_8.Text = "3/8";
            this.btn3_8.UseVisualStyleBackColor = false;
            // 
            // btn2_7
            // 
            this.btn2_7.BackColor = System.Drawing.Color.ForestGreen;
            this.btn2_7.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2_7.ForeColor = System.Drawing.Color.White;
            this.btn2_7.Location = new System.Drawing.Point(200, 49);
            this.btn2_7.Name = "btn2_7";
            this.btn2_7.Size = new System.Drawing.Size(75, 71);
            this.btn2_7.TabIndex = 5;
            this.btn2_7.Text = "2/7";
            this.btn2_7.UseVisualStyleBackColor = false;
            // 
            // btn1_6
            // 
            this.btn1_6.BackColor = System.Drawing.Color.ForestGreen;
            this.btn1_6.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1_6.ForeColor = System.Drawing.Color.White;
            this.btn1_6.Location = new System.Drawing.Point(118, 49);
            this.btn1_6.Name = "btn1_6";
            this.btn1_6.Size = new System.Drawing.Size(75, 71);
            this.btn1_6.TabIndex = 4;
            this.btn1_6.Text = "1/6";
            this.btn1_6.UseVisualStyleBackColor = false;
            // 
            // btnEnt
            // 
            this.btnEnt.BackColor = System.Drawing.Color.ForestGreen;
            this.btnEnt.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnt.ForeColor = System.Drawing.Color.White;
            this.btnEnt.Location = new System.Drawing.Point(35, 49);
            this.btnEnt.Name = "btnEnt";
            this.btnEnt.Size = new System.Drawing.Size(75, 71);
            this.btnEnt.TabIndex = 3;
            this.btnEnt.Text = "ENT";
            this.btnEnt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEnt.UseVisualStyleBackColor = false;
            // 
            // wizardPageResultsStatus
            // 
            this.wizardPageResultsStatus.Controls.Add(this.themedLabel12);
            this.wizardPageResultsStatus.Controls.Add(this.tbllnitialStatus);
            this.wizardPageResultsStatus.Name = "wizardPageResultsStatus";
            this.wizardPageResultsStatus.NextPage = this.wizardPageKeyPress;
            this.wizardPageResultsStatus.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageResultsStatus, "PCB initial status");
            this.wizardPageResultsStatus.TabIndex = 5;
            this.wizardPageResultsStatus.Text = "Initial Status";
            this.wizardPageResultsStatus.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageResultsStatus_Initialize);
            this.wizardPageResultsStatus.Enter += new System.EventHandler(this.wizardPageResultsStatus_Enter);
            this.wizardPageResultsStatus.Leave += new System.EventHandler(this.wizardPageResultsStatus_Leave);
            // 
            // themedLabel12
            // 
            this.themedLabel12.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel12.Location = new System.Drawing.Point(43, 41);
            this.themedLabel12.Name = "themedLabel12";
            this.themedLabel12.Size = new System.Drawing.Size(409, 30);
            this.themedLabel12.TabIndex = 1;
            this.themedLabel12.Text = "Detecting the status of the initial test parameters. ";
            // 
            // tbllnitialStatus
            // 
            this.tbllnitialStatus.ColumnCount = 3;
            this.tbllnitialStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbllnitialStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbllnitialStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tbllnitialStatus.Location = new System.Drawing.Point(43, 99);
            this.tbllnitialStatus.Name = "tbllnitialStatus";
            this.tbllnitialStatus.RowCount = 1;
            this.tbllnitialStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbllnitialStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tbllnitialStatus.Size = new System.Drawing.Size(443, 125);
            this.tbllnitialStatus.TabIndex = 0;
            // 
            // wizardPageSignIn
            // 
            this.wizardPageSignIn.AllowBack = false;
            this.wizardPageSignIn.AllowNext = false;
            this.wizardPageSignIn.Controls.Add(this.themedLabel7);
            this.wizardPageSignIn.Controls.Add(this.btnAddTechnician);
            this.wizardPageSignIn.Controls.Add(this.themedLabel2);
            this.wizardPageSignIn.Controls.Add(this.cbTechnician);
            this.wizardPageSignIn.Controls.Add(this.themedLabel1);
            this.wizardPageSignIn.Name = "wizardPageSignIn";
            this.wizardPageSignIn.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageSignIn, "Technician Sign In");
            this.wizardPageSignIn.TabIndex = 2;
            this.wizardPageSignIn.Text = "Begin testing...";
            this.wizardPageSignIn.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wizardPageSignIn_Commit);
            this.wizardPageSignIn.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageSignIn_Initialize);
            // 
            // themedLabel7
            // 
            this.themedLabel7.Location = new System.Drawing.Point(77, 85);
            this.themedLabel7.Name = "themedLabel7";
            this.themedLabel7.Size = new System.Drawing.Size(309, 36);
            this.themedLabel7.TabIndex = 5;
            this.themedLabel7.Text = "Please select your name from the cobo list below or click \"Add Technician\" to ins" +
    "ert your name in the list.";
            // 
            // btnAddTechnician
            // 
            this.btnAddTechnician.Location = new System.Drawing.Point(276, 203);
            this.btnAddTechnician.Name = "btnAddTechnician";
            this.btnAddTechnician.Size = new System.Drawing.Size(110, 23);
            this.btnAddTechnician.TabIndex = 4;
            this.btnAddTechnician.Text = "Add Technician";
            this.btnAddTechnician.UseVisualStyleBackColor = true;
            this.btnAddTechnician.Click += new System.EventHandler(this.btnAddTechnician_Click);
            // 
            // themedLabel2
            // 
            this.themedLabel2.Location = new System.Drawing.Point(74, 132);
            this.themedLabel2.Name = "themedLabel2";
            this.themedLabel2.Size = new System.Drawing.Size(100, 23);
            this.themedLabel2.TabIndex = 3;
            this.themedLabel2.Text = "Technician";
            // 
            // cbTechnician
            // 
            this.cbTechnician.FormattingEnabled = true;
            this.cbTechnician.Location = new System.Drawing.Point(74, 165);
            this.cbTechnician.Name = "cbTechnician";
            this.cbTechnician.Size = new System.Drawing.Size(312, 23);
            this.cbTechnician.TabIndex = 2;
            this.cbTechnician.SelectedIndexChanged += new System.EventHandler(this.cbTechnician_SelectedIndexChanged);
            // 
            // themedLabel1
            // 
            this.themedLabel1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel1.Location = new System.Drawing.Point(70, 47);
            this.themedLabel1.Name = "themedLabel1";
            this.themedLabel1.Size = new System.Drawing.Size(218, 23);
            this.themedLabel1.TabIndex = 0;
            this.themedLabel1.Text = "Sign in to begin testing";
            // 
            // wizardPageInsertPCB
            // 
            this.wizardPageInsertPCB.Controls.Add(this.themedLabel6);
            this.wizardPageInsertPCB.Controls.Add(this.themedLabel5);
            this.wizardPageInsertPCB.Controls.Add(this.txtManufactureSerial);
            this.wizardPageInsertPCB.Controls.Add(this.themedLabel4);
            this.wizardPageInsertPCB.Controls.Add(this.themedLabel3);
            this.wizardPageInsertPCB.Name = "wizardPageInsertPCB";
            this.wizardPageInsertPCB.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageInsertPCB, "Insert PCB for testing.");
            this.wizardPageInsertPCB.TabIndex = 3;
            this.wizardPageInsertPCB.Text = "Insert PCB";
            this.wizardPageInsertPCB.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wizardPageInsertPCB_Commit);
            this.wizardPageInsertPCB.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageInsertPCB_Initialize);
            // 
            // themedLabel6
            // 
            this.themedLabel6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel6.Location = new System.Drawing.Point(117, 44);
            this.themedLabel6.Name = "themedLabel6";
            this.themedLabel6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.themedLabel6.Size = new System.Drawing.Size(262, 23);
            this.themedLabel6.TabIndex = 4;
            this.themedLabel6.Text = "Get serial number and insert PCB";
            // 
            // themedLabel5
            // 
            this.themedLabel5.Location = new System.Drawing.Point(119, 154);
            this.themedLabel5.Name = "themedLabel5";
            this.themedLabel5.Size = new System.Drawing.Size(214, 23);
            this.themedLabel5.TabIndex = 3;
            this.themedLabel5.Text = "Manufacturers serial number";
            // 
            // txtManufactureSerial
            // 
            this.txtManufactureSerial.Location = new System.Drawing.Point(118, 181);
            this.txtManufactureSerial.Name = "txtManufactureSerial";
            this.txtManufactureSerial.Size = new System.Drawing.Size(228, 23);
            this.txtManufactureSerial.TabIndex = 2;
            this.txtManufactureSerial.TextChanged += new System.EventHandler(this.txtManufactureSerial_TextChanged);
            // 
            // themedLabel4
            // 
            this.themedLabel4.Location = new System.Drawing.Point(118, 102);
            this.themedLabel4.Name = "themedLabel4";
            this.themedLabel4.Size = new System.Drawing.Size(280, 38);
            this.themedLabel4.TabIndex = 1;
            this.themedLabel4.Text = "Enter the manufacturers number from the sticker on the PCB into the text box belo" +
    "w.";
            // 
            // themedLabel3
            // 
            this.themedLabel3.Location = new System.Drawing.Point(115, 216);
            this.themedLabel3.Name = "themedLabel3";
            this.themedLabel3.Size = new System.Drawing.Size(283, 48);
            this.themedLabel3.TabIndex = 0;
            this.themedLabel3.Text = "Then insert the PCB under test into the test jig and click next to continue.";
            // 
            // wizardPageProgramPCB
            // 
            this.wizardPageProgramPCB.Controls.Add(this.tblPCBUnitId);
            this.wizardPageProgramPCB.Controls.Add(this.themedLabel11);
            this.wizardPageProgramPCB.Controls.Add(this.pictureBox1);
            this.wizardPageProgramPCB.Controls.Add(this.themedLabel10);
            this.wizardPageProgramPCB.Controls.Add(this.themedLabel9);
            this.wizardPageProgramPCB.Controls.Add(this.themedLabel8);
            this.wizardPageProgramPCB.Name = "wizardPageProgramPCB";
            this.wizardPageProgramPCB.NextPage = this.wizardPageResultsStatus;
            this.wizardPageProgramPCB.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageProgramPCB, "Program the node PCB");
            this.wizardPageProgramPCB.TabIndex = 4;
            this.wizardPageProgramPCB.Text = "Program PCB";
            this.wizardPageProgramPCB.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wizardPageProgramPCB_Commit);
            this.wizardPageProgramPCB.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageProgramPCB_Initialize);
            this.wizardPageProgramPCB.Leave += new System.EventHandler(this.wizardPageProgramPCB_Leave);
            // 
            // tblPCBUnitId
            // 
            this.tblPCBUnitId.ColumnCount = 1;
            this.tblPCBUnitId.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.37407F));
            this.tblPCBUnitId.Location = new System.Drawing.Point(58, 300);
            this.tblPCBUnitId.Name = "tblPCBUnitId";
            this.tblPCBUnitId.RowCount = 1;
            this.tblPCBUnitId.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblPCBUnitId.Size = new System.Drawing.Size(401, 100);
            this.tblPCBUnitId.TabIndex = 5;
            // 
            // themedLabel11
            // 
            this.themedLabel11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel11.Location = new System.Drawing.Point(147, 260);
            this.themedLabel11.Name = "themedLabel11";
            this.themedLabel11.Size = new System.Drawing.Size(275, 23);
            this.themedLabel11.TabIndex = 4;
            this.themedLabel11.Text = "Waiting for a response from the PCB";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ESLTestProcess.Properties.Resources.spinner;
            this.pictureBox1.Location = new System.Drawing.Point(58, 223);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(65, 65);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // themedLabel10
            // 
            this.themedLabel10.Location = new System.Drawing.Point(54, 153);
            this.themedLabel10.Name = "themedLabel10";
            this.themedLabel10.Size = new System.Drawing.Size(356, 70);
            this.themedLabel10.TabIndex = 2;
            this.themedLabel10.Text = "The next step is to receive the internal serial number of the PCB.  If a serial n" +
    "umber is already present it signals that the current process is a retest.";
            // 
            // themedLabel9
            // 
            this.themedLabel9.Location = new System.Drawing.Point(53, 82);
            this.themedLabel9.Name = "themedLabel9";
            this.themedLabel9.Size = new System.Drawing.Size(359, 67);
            this.themedLabel9.TabIndex = 1;
            this.themedLabel9.Text = "Program the trap node PCB with the Microchip PickIt2 programmer.  Once the PCB is" +
    " repowered it will begin the test process.";
            // 
            // themedLabel8
            // 
            this.themedLabel8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel8.Location = new System.Drawing.Point(54, 32);
            this.themedLabel8.Name = "themedLabel8";
            this.themedLabel8.Size = new System.Drawing.Size(259, 23);
            this.themedLabel8.TabIndex = 0;
            this.themedLabel8.Text = "Program the trap node PCB";
            // 
            // wizardPageAccelerometerBase
            // 
            this.wizardPageAccelerometerBase.Controls.Add(this.tblAccelerometerBasline);
            this.wizardPageAccelerometerBase.Controls.Add(this.themedLabel13);
            this.wizardPageAccelerometerBase.Name = "wizardPageAccelerometerBase";
            this.wizardPageAccelerometerBase.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageAccelerometerBase, "Accelerometer baseline");
            this.wizardPageAccelerometerBase.TabIndex = 6;
            this.wizardPageAccelerometerBase.Text = "Accelerometer Baseline Measurements";
            this.wizardPageAccelerometerBase.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageAccelerometerBase_Initialize);
            this.wizardPageAccelerometerBase.Leave += new System.EventHandler(this.wizardPageAccelerometerBase_Leave);
            // 
            // tblAccelerometerBasline
            // 
            this.tblAccelerometerBasline.ColumnCount = 1;
            this.tblAccelerometerBasline.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAccelerometerBasline.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblAccelerometerBasline.Location = new System.Drawing.Point(37, 74);
            this.tblAccelerometerBasline.Name = "tblAccelerometerBasline";
            this.tblAccelerometerBasline.RowCount = 1;
            this.tblAccelerometerBasline.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAccelerometerBasline.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 117F));
            this.tblAccelerometerBasline.Size = new System.Drawing.Size(446, 117);
            this.tblAccelerometerBasline.TabIndex = 1;
            // 
            // themedLabel13
            // 
            this.themedLabel13.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel13.Location = new System.Drawing.Point(33, 25);
            this.themedLabel13.Name = "themedLabel13";
            this.themedLabel13.Size = new System.Drawing.Size(326, 27);
            this.themedLabel13.TabIndex = 0;
            this.themedLabel13.Text = "Requesting basline accelerometer values";
            // 
            // wizardPageAccelTestXY
            // 
            this.wizardPageAccelTestXY.Controls.Add(this.themedLabel14);
            this.wizardPageAccelTestXY.Controls.Add(this.tblAccelerometerXY);
            this.wizardPageAccelTestXY.Name = "wizardPageAccelTestXY";
            this.wizardPageAccelTestXY.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageAccelTestXY, "Accelerometer XY axis");
            this.wizardPageAccelTestXY.TabIndex = 7;
            this.wizardPageAccelTestXY.Text = "Accelerometer XY axis";
            this.wizardPageAccelTestXY.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageAccelTestXY_Initialize);
            this.wizardPageAccelTestXY.Leave += new System.EventHandler(this.wizardPageAccelTestXY_Leave);
            // 
            // themedLabel14
            // 
            this.themedLabel14.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel14.Location = new System.Drawing.Point(26, 23);
            this.themedLabel14.Name = "themedLabel14";
            this.themedLabel14.Size = new System.Drawing.Size(410, 23);
            this.themedLabel14.TabIndex = 1;
            this.themedLabel14.Text = "Hold the PCB under test vertically on the longest edge ";
            // 
            // tblAccelerometerXY
            // 
            this.tblAccelerometerXY.ColumnCount = 1;
            this.tblAccelerometerXY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAccelerometerXY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblAccelerometerXY.Location = new System.Drawing.Point(26, 66);
            this.tblAccelerometerXY.Name = "tblAccelerometerXY";
            this.tblAccelerometerXY.RowCount = 1;
            this.tblAccelerometerXY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAccelerometerXY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tblAccelerometerXY.Size = new System.Drawing.Size(458, 123);
            this.tblAccelerometerXY.TabIndex = 0;
            // 
            // wizardPageAccelTestYZ
            // 
            this.wizardPageAccelTestYZ.Controls.Add(this.tblAccelerometerYZ);
            this.wizardPageAccelTestYZ.Controls.Add(this.themedLabel15);
            this.wizardPageAccelTestYZ.Name = "wizardPageAccelTestYZ";
            this.wizardPageAccelTestYZ.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageAccelTestYZ, "Acceleromter YZ axis");
            this.wizardPageAccelTestYZ.TabIndex = 8;
            this.wizardPageAccelTestYZ.Text = "Acceleromter YZ axis";
            this.wizardPageAccelTestYZ.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageAccelTestYZ_Initialize);
            this.wizardPageAccelTestYZ.Leave += new System.EventHandler(this.wizardPageAccelTestYZ_Leave);
            // 
            // tblAccelerometerYZ
            // 
            this.tblAccelerometerYZ.ColumnCount = 1;
            this.tblAccelerometerYZ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAccelerometerYZ.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblAccelerometerYZ.Location = new System.Drawing.Point(26, 55);
            this.tblAccelerometerYZ.Name = "tblAccelerometerYZ";
            this.tblAccelerometerYZ.RowCount = 1;
            this.tblAccelerometerYZ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblAccelerometerYZ.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tblAccelerometerYZ.Size = new System.Drawing.Size(465, 146);
            this.tblAccelerometerYZ.TabIndex = 1;
            // 
            // themedLabel15
            // 
            this.themedLabel15.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel15.Location = new System.Drawing.Point(22, 19);
            this.themedLabel15.Name = "themedLabel15";
            this.themedLabel15.Size = new System.Drawing.Size(417, 23);
            this.themedLabel15.TabIndex = 0;
            this.themedLabel15.Text = "Hold the PCB under test vertically on its shortest edge";
            // 
            // wizardPageTransceiver
            // 
            this.wizardPageTransceiver.Controls.Add(this.themedLabel16);
            this.wizardPageTransceiver.Controls.Add(this.tblTransceiverTest);
            this.wizardPageTransceiver.Name = "wizardPageTransceiver";
            this.wizardPageTransceiver.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageTransceiver, "Tranceiver test");
            this.wizardPageTransceiver.TabIndex = 9;
            this.wizardPageTransceiver.Text = "Tranceiver Test";
            this.wizardPageTransceiver.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageTransceiver_Initialize);
            // 
            // themedLabel16
            // 
            this.themedLabel16.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel16.Location = new System.Drawing.Point(25, 23);
            this.themedLabel16.Name = "themedLabel16";
            this.themedLabel16.Size = new System.Drawing.Size(450, 23);
            this.themedLabel16.TabIndex = 1;
            this.themedLabel16.Text = "Sending data through the transceiver and waiting for a reply";
            // 
            // tblTransceiverTest
            // 
            this.tblTransceiverTest.ColumnCount = 1;
            this.tblTransceiverTest.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTransceiverTest.Location = new System.Drawing.Point(25, 87);
            this.tblTransceiverTest.Name = "tblTransceiverTest";
            this.tblTransceiverTest.RowCount = 1;
            this.tblTransceiverTest.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblTransceiverTest.Size = new System.Drawing.Size(463, 159);
            this.tblTransceiverTest.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 568);
            this.Controls.Add(this.stepWizardControl1);
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).EndInit();
            this.wizardPageKeyPress.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.wizardPageResultsStatus.ResumeLayout(false);
            this.wizardPageSignIn.ResumeLayout(false);
            this.wizardPageInsertPCB.ResumeLayout(false);
            this.wizardPageInsertPCB.PerformLayout();
            this.wizardPageProgramPCB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.wizardPageAccelerometerBase.ResumeLayout(false);
            this.wizardPageAccelTestXY.ResumeLayout(false);
            this.wizardPageAccelTestYZ.ResumeLayout(false);
            this.wizardPageTransceiver.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.StepWizardControl stepWizardControl1;
        private AeroWizard.WizardPage wizardPageSignIn;
        private AeroWizard.ThemedLabel themedLabel1;
        private System.Windows.Forms.Button btnAddTechnician;
        private AeroWizard.ThemedLabel themedLabel2;
        private System.Windows.Forms.ComboBox cbTechnician;
        private AeroWizard.WizardPage wizardPageInsertPCB;
        private AeroWizard.ThemedLabel themedLabel4;
        private AeroWizard.ThemedLabel themedLabel3;
        private AeroWizard.ThemedLabel themedLabel5;
        private System.Windows.Forms.TextBox txtManufactureSerial;
        private AeroWizard.ThemedLabel themedLabel6;
        private AeroWizard.ThemedLabel themedLabel7;
        private AeroWizard.WizardPage wizardPageProgramPCB;
        private AeroWizard.ThemedLabel themedLabel10;
        private AeroWizard.ThemedLabel themedLabel9;
        private AeroWizard.ThemedLabel themedLabel8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private AeroWizard.ThemedLabel themedLabel11;
        private AeroWizard.WizardPage wizardPageResultsStatus;
        private System.Windows.Forms.TableLayoutPanel tbllnitialStatus;
        private AeroWizard.ThemedLabel themedLabel12;
        private AeroWizard.WizardPage wizardPageAccelerometerBase;
        private AeroWizard.ThemedLabel themedLabel13;
        private AeroWizard.WizardPage wizardPageAccelTestXY;
        private AeroWizard.WizardPage wizardPageAccelTestYZ;
        private System.Windows.Forms.TableLayoutPanel tblAccelerometerBasline;
        private AeroWizard.ThemedLabel themedLabel14;
        private System.Windows.Forms.TableLayoutPanel tblAccelerometerXY;
        private System.Windows.Forms.TableLayoutPanel tblAccelerometerYZ;
        private AeroWizard.ThemedLabel themedLabel15;
        private AeroWizard.WizardPage wizardPageTransceiver;
        private AeroWizard.ThemedLabel themedLabel16;
        private System.Windows.Forms.TableLayoutPanel tblTransceiverTest;
        private System.Windows.Forms.TableLayoutPanel tblPCBUnitId;
        private AeroWizard.WizardPage wizardPageKeyPress;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn5_0;
        private System.Windows.Forms.Button btn4_9;
        private System.Windows.Forms.Button btn3_8;
        private System.Windows.Forms.Button btn2_7;
        private System.Windows.Forms.Button btn1_6;
        private System.Windows.Forms.Button btnEnt;


    }
}

