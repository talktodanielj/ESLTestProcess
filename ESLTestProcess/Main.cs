﻿using ESLTestProcess.Data;
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
        private System.Threading.Timer _demoTimer;


        private void stepWizardControl1_SelectedPageChanged(object sender, EventArgs e)
        {

        }

        private void btnAddTechnician_Click(object sender, EventArgs e)
        {
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

        private void DemoTimerCallback(Object state)
        {
            var icon = (PictureBox)tbllnitialStatus.Controls.Find(TestParameters.GetIconName(TestParameters.EPROM_ID), true).FirstOrDefault();

            if (icon != null)
                icon.Image = ESLTestProcess.Properties.Resources.tick;

            icon = (PictureBox)tbllnitialStatus.Controls.Find(TestParameters.GetIconName(TestParameters.ACCELEROMETER_ID), true).FirstOrDefault();

            if (icon != null)
                icon.Image = ESLTestProcess.Properties.Resources.alert;


            icon = (PictureBox)tbllnitialStatus.Controls.Find(TestParameters.GetIconName(TestParameters.BATTERY_VOLTAGE), true).FirstOrDefault();

            if (icon != null)
                icon.Image = ESLTestProcess.Properties.Resources.cross;


        }

        private void wizardPageResultsStatus_Initialize(object sender, AeroWizard.WizardPageInitEventArgs e)
        {
            if (tbllnitialStatus.RowCount == 1)
            {
                GenerateTable();
                _demoTimer = new System.Threading.Timer(DemoTimerCallback, this, 3000, 0);
            }
        }

        private void GenerateTable()
        {
            tbllnitialStatus.SuspendLayout();

            //Clear out the existing controls, we are generating a new table layout
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
