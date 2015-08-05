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
            this.themedLabel1 = new AeroWizard.ThemedLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.themedLabel2 = new AeroWizard.ThemedLabel();
            this.btnAddTechnician = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).BeginInit();
            this.wizardPageSignIn.SuspendLayout();
            this.SuspendLayout();
            // 
            // stepWizardControl1
            // 
            this.stepWizardControl1.Location = new System.Drawing.Point(0, 0);
            this.stepWizardControl1.Name = "stepWizardControl1";
            this.stepWizardControl1.Pages.Add(this.wizardPageSignIn);
            this.stepWizardControl1.Size = new System.Drawing.Size(806, 568);
            this.stepWizardControl1.StepListFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.stepWizardControl1.StepListWidth = 250;
            this.stepWizardControl1.TabIndex = 0;
            this.stepWizardControl1.Title = "ESL test process for trap node PCB";
            // 
            // wizardPageSignIn
            // 
            this.wizardPageSignIn.Controls.Add(this.btnAddTechnician);
            this.wizardPageSignIn.Controls.Add(this.themedLabel2);
            this.wizardPageSignIn.Controls.Add(this.comboBox1);
            this.wizardPageSignIn.Controls.Add(this.textBox1);
            this.wizardPageSignIn.Controls.Add(this.themedLabel1);
            this.wizardPageSignIn.Name = "wizardPageSignIn";
            this.wizardPageSignIn.Size = new System.Drawing.Size(508, 414);
            this.stepWizardControl1.SetStepText(this.wizardPageSignIn, "Technician Sign In");
            this.wizardPageSignIn.TabIndex = 2;
            this.wizardPageSignIn.Text = "Begin test";
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
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(72, 84);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(314, 41);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Please select your name from the cobo list below or click \"Add Technician\" to ins" +
    "ert your name in the list.";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(74, 165);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(312, 23);
            this.comboBox1.TabIndex = 2;
            // 
            // themedLabel2
            // 
            this.themedLabel2.Location = new System.Drawing.Point(74, 132);
            this.themedLabel2.Name = "themedLabel2";
            this.themedLabel2.Size = new System.Drawing.Size(100, 23);
            this.themedLabel2.TabIndex = 3;
            this.themedLabel2.Text = "Technician";
            // 
            // btnAddTechnician
            // 
            this.btnAddTechnician.Location = new System.Drawing.Point(276, 203);
            this.btnAddTechnician.Name = "btnAddTechnician";
            this.btnAddTechnician.Size = new System.Drawing.Size(110, 23);
            this.btnAddTechnician.TabIndex = 4;
            this.btnAddTechnician.Text = "Add Technician";
            this.btnAddTechnician.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 568);
            this.Controls.Add(this.stepWizardControl1);
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.stepWizardControl1)).EndInit();
            this.wizardPageSignIn.ResumeLayout(false);
            this.wizardPageSignIn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.StepWizardControl stepWizardControl1;
        private AeroWizard.WizardPage wizardPageSignIn;
        private AeroWizard.ThemedLabel themedLabel1;
        private System.Windows.Forms.Button btnAddTechnician;
        private AeroWizard.ThemedLabel themedLabel2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;


    }
}

