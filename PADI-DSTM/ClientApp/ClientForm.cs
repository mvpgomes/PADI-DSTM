﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientApp
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();
        }

        private void connectBut_Click(object sender, EventArgs e)
        {
            //just for test
            logText.Text += "tcp://localhost:" + portNumb.Text + "/" + NickBox.Text;
            logText.Text += "\r\n";
        }

    }
}
