﻿using System;
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
    public partial class AddTechnician : Form
    {
        public AddTechnician()
        {
            InitializeComponent();
        }

        public string TechnicianName { get { return txtTechnicianName.Text; } set { txtTechnicianName.Text = value; } }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
