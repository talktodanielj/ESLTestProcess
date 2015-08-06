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
            this.wizardPageSignIn = new AeroWizard.WizardPage();
            this.themedLabel7 = new AeroWizard.ThemedLabel();
            this.btnAddTechnician = new System.Windows.Forms.Button();
            this.themedLabel2 = new AeroWizard.ThemedLabel();
            this.cbTechnician = new System.Windows.Forms.ComboBox();
            this.themedLabel1 = new AeroWizard.ThemedLabel();
            this.wizardPageInsertPCB = new AeroWizard.WizardPage();
            this.themedLabel6 = new AeroWizard.ThemedLabel();
            this.themedLabel5 = new AeroWizard.ThemedLabel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.themedLabel4 = new AeroWizard.ThemedLabel();
            this.themedLabel3 = new AeroWizard.ThemedLabel();
            this.wizardPageProgramPCB = new AeroWizard.WizardPage();
            this.themedLabel11 = new AeroWizard.ThemedLabel();
            this.themedLabel10 = new AeroWizard.ThemedLabel();
            this.themedLabel9 = new AeroWizard.ThemedLabel();
            this.themedLabel8 = new AeroWizard.ThemedLabel();
            this.wizardPageResultsStatus = new AeroWizard.WizardPage();
            this.themedLabel12 = new AeroWizard.ThemedLabel();
            this.tbllnitialStatus = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).BeginInit();
            this.wizardPageSignIn.SuspendLayout();
            this.wizardPageInsertPCB.SuspendLayout();
            this.wizardPageProgramPCB.SuspendLayout();
            this.wizardPageResultsStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.stepWizardControl1.Size = new System.Drawing.Size(806, 568);
            this.stepWizardControl1.StepListFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.stepWizardControl1.StepListWidth = 250;
            this.stepWizardControl1.TabIndex = 0;
            this.stepWizardControl1.Title = "ESL test process for trap node PCB";
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
            this.wizardPageInsertPCB.Controls.Add(this.textBox2);
            this.wizardPageInsertPCB.Controls.Add(this.themedLabel4);
            this.wizardPageInsertPCB.Controls.Add(this.themedLabel3);
            this.wizardPageInsertPCB.Name = "wizardPageInsertPCB";
            this.wizardPageInsertPCB.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageInsertPCB, "Insert PCB for testing.");
            this.wizardPageInsertPCB.TabIndex = 3;
            this.wizardPageInsertPCB.Text = "Insert PCB";
            this.wizardPageInsertPCB.Commit += new System.EventHandler<AeroWizard.WizardPageConfirmEventArgs>(this.wizardPageInsertPCB_Commit);
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
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(118, 181);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(228, 23);
            this.textBox2.TabIndex = 2;
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
            this.wizardPageProgramPCB.Controls.Add(this.themedLabel11);
            this.wizardPageProgramPCB.Controls.Add(this.pictureBox1);
            this.wizardPageProgramPCB.Controls.Add(this.themedLabel10);
            this.wizardPageProgramPCB.Controls.Add(this.themedLabel9);
            this.wizardPageProgramPCB.Controls.Add(this.themedLabel8);
            this.wizardPageProgramPCB.Name = "wizardPageProgramPCB";
            this.wizardPageProgramPCB.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageProgramPCB, "Program the node PCB");
            this.wizardPageProgramPCB.TabIndex = 4;
            this.wizardPageProgramPCB.Text = "Program PCB";
            // 
            // themedLabel11
            // 
            this.themedLabel11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themedLabel11.Location = new System.Drawing.Point(147, 267);
            this.themedLabel11.Name = "themedLabel11";
            this.themedLabel11.Size = new System.Drawing.Size(275, 23);
            this.themedLabel11.TabIndex = 4;
            this.themedLabel11.Text = "Waiting for a response from the PCB";
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
            // wizardPageResultsStatus
            // 
            this.wizardPageResultsStatus.Controls.Add(this.themedLabel12);
            this.wizardPageResultsStatus.Controls.Add(this.tbllnitialStatus);
            this.wizardPageResultsStatus.Name = "wizardPageResultsStatus";
            this.wizardPageResultsStatus.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageResultsStatus, "PCB initial status");
            this.wizardPageResultsStatus.TabIndex = 5;
            this.wizardPageResultsStatus.Text = "Initial Status";
            this.wizardPageResultsStatus.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.wizardPageResultsStatus_Initialize);
            this.wizardPageResultsStatus.Leave += new System.EventHandler(this.wizardPageResultsStatus_Leave);
            // 
            // themedLabel12
            // 
            this.themedLabel12.Location = new System.Drawing.Point(43, 41);
            this.themedLabel12.Name = "themedLabel12";
            this.themedLabel12.Size = new System.Drawing.Size(311, 42);
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
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ESLTestProcess.Properties.Resources.spinner;
            this.pictureBox1.Location = new System.Drawing.Point(58, 242);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(65, 65);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
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
            this.wizardPageSignIn.ResumeLayout(false);
            this.wizardPageInsertPCB.ResumeLayout(false);
            this.wizardPageInsertPCB.PerformLayout();
            this.wizardPageProgramPCB.ResumeLayout(false);
            this.wizardPageResultsStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.TextBox textBox2;
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


    }
}

