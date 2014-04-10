using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PADI_DSTM;

namespace ClientApplicationForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CreatePadInt_Click(object sender, EventArgs e)
        {
            int PadIntID = Convert.ToInt32(CreateTextBox.Text);
            PadiDstm.CreatePadInt(PadIntID);
        }

        private void AccessPadInt_Click(object sender, EventArgs e)
        {
            int PadIntID = Convert.ToInt32(AccessTextBox.Text);
            PadiDstm.AccessPadInt(PadIntID);
        }

        private void Fail_Click(object sender, EventArgs e)
        {
            string URL = URLTextBox.Text;
            PadiDstm.Fail(URL);
        }

        private void Freeze_Click(object sender, EventArgs e)
        {
            string URL = URLTextBox.Text;
            PadiDstm.Freeze(URL);
        }

        private void Recover_Click(object sender, EventArgs e)
        {
            string URL = URLTextBox.Text;
            PadiDstm.Recover(URL);
        }

        private void Status_Click(object sender, EventArgs e)
        {
            PadiDstm.Status();
        }

        private void TxAbort_Click(object sender, EventArgs e)
        {

        }

        private void TxBegin_Click(object sender, EventArgs e)
        {

        }

        private void TxCommit_Click(object sender, EventArgs e)
        {

        }
    }
}
