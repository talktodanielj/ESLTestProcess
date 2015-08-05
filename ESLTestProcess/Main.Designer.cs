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
            this.btnAddTechnician = new System.Windows.Forms.Button();
            this.themedLabel2 = new AeroWizard.ThemedLabel();
            this.cbTechnician = new System.Windows.Forms.ComboBox();
            this.themedLabel1 = new AeroWizard.ThemedLabel();
            this.wizardPageInsertPCB = new AeroWizard.WizardPage();
            this.themedLabel3 = new AeroWizard.ThemedLabel();
            this.themedLabel4 = new AeroWizard.ThemedLabel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.themedLabel5 = new AeroWizard.ThemedLabel();
            this.themedLabel6 = new AeroWizard.ThemedLabel();
            this.themedLabel7 = new AeroWizard.ThemedLabel();
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).BeginInit();
            this.wizardPageSignIn.SuspendLayout();
            this.wizardPageInsertPCB.SuspendLayout();
            this.SuspendLayout();
            // 
            // stepWizardControl1
            // 
            this.stepWizardControl1.Location = new System.Drawing.Point(0, 0);
            this.stepWizardControl1.Name = "stepWizardControl1";
            this.stepWizardControl1.Pages.Add(this.wizardPageSignIn);
            this.stepWizardControl1.Pages.Add(this.wizardPageInsertPCB);
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
            // comboBox1
            // 
            this.cbTechnician.FormattingEnabled = true;
            this.cbTechnician.Location = new System.Drawing.Point(74, 165);
            this.cbTechnician.Name = "comboBox1";
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
            // themedLabel3
            // 
            this.themedLabel3.Location = new System.Drawing.Point(115, 216);
            this.themedLabel3.Name = "themedLabel3";
            this.themedLabel3.Size = new System.Drawing.Size(283, 48);
            this.themedLabel3.TabIndex = 0;
            this.themedLabel3.Text = "Then insert the PCB under test into the test jig and click next to continue.";
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
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(118, 181);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(228, 23);
            this.textBox2.TabIndex = 2;
            // 
            // themedLabel5
            // 
            this.themedLabel5.Location = new System.Drawing.Point(119, 154);
            this.themedLabel5.Name = "themedLabel5";
            this.themedLabel5.Size = new System.Drawing.Size(214, 23);
            this.themedLabel5.TabIndex = 3;
            this.themedLabel5.Text = "Manufacturers serial number";
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
            // themedLabel7
            // 
            this.themedLabel7.Location = new System.Drawing.Point(77, 85);
            this.themedLabel7.Name = "themedLabel7";
            this.themedLabel7.Size = new System.Drawing.Size(309, 36);
            this.themedLabel7.TabIndex = 5;
            this.themedLabel7.Text = "Please select your name from the cobo list below or click \"Add Technician\" to ins" +
    "ert your name in the list.";
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


    }
}

