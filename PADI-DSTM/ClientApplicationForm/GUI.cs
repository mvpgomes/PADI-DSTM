using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonTypes;
using PADI_DSTM;

namespace ClientApplicationForm
{
    public partial class GUI : Form
    {
        private bool isOnTrans;
        public GUI()
        {
            InitializeComponent();
            this.isOnTrans = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CreatePadInt_Click(object sender, EventArgs e)
        {
            string PadIntID = uidTxt.Text;

            if (!this.isOnTrans)
            {
                updateLog("Please begin a transaction.");
                return;
            }

            if (PadIntID.Length == 0)
            {
                updateLog("Please insert a valid ID.");
                return;
            }

            PadInt res = PadiDstm.CreatePadInt(Convert.ToInt32(PadIntID));

            if (res == null)
            {
                updateLog("The PadInt with id " + PadIntID + " already exists.");
            }
            else
            {
                updatePadintStack(res.ToString());
            }
        }

        private void AccessPadInt_Click(object sender, EventArgs e)
        {
            string PadIntID = uidTxt.Text;

            if (!this.isOnTrans)
            {
                updateLog("Please begin a transaction.");
                return;
            }

            if (PadIntID.Length == 0)
            {
                updateLog("Please insert a valid ID.");
                return;
            }

            PadInt res = PadiDstm.AccessPadInt(Convert.ToInt32(PadIntID));

            if (res == null)
            {
                updateLog("The PadInt with id " + PadIntID + " does not exist.");
            }
        }

        private void Fail_Click(object sender, EventArgs e)
        {
            string URL = URLTextBox.Text;

            if (URL.Length == 0)
            {
                updateLog("Please insert a valid URL.");
                return;
            }

            bool res = PadiDstm.Fail(URL);
        }

        private void Freeze_Click(object sender, EventArgs e)
        {
            string URL = URLTextBox.Text;

            if (URL.Length == 0)
            {
                updateLog("Please insert a valid URL.");
                return;
            }


            bool res = PadiDstm.Freeze(URL);

            if (res == false)
            {
                updateLog("The reuqest could not be completed.");
            }
        }

        private void Recover_Click(object sender, EventArgs e)
        {
            string URL = URLTextBox.Text;

            if (URL.Length == 0)
            {
                updateLog("Please insert a valid URL.");
                return;
            }

            bool res = PadiDstm.Recover(URL);

            if (res == false)
            {
                updateLog("The reuqest could not be completed.");
            }
        }

        private void Status_Click(object sender, EventArgs e)
        {
            bool res = PadiDstm.Status();

            if (res == false)
            {
                updateLog("The reuqest could not be completed.");
            }
        }

        private void TxAbort_Click(object sender, EventArgs e)
        {
            if (this.isOnTrans)
            {
                PadiDstm.TxCommit();
                this.isOnTrans = false;
            }
            else
            {
                updateLog("Please Begin a transaction.");
            }
        }

        private void TxBegin_Click(object sender, EventArgs e)
        {
            if(this.isOnTrans) 
            {
                updateLog("Error. No support for nested transaction.");
                return;
            }
            this.isOnTrans = true;

            PadiDstm.TxBegin();
        }

        private void TxCommit_Click(object sender, EventArgs e)
        {
            if (this.isOnTrans)
            {
                PadiDstm.TxCommit();
                this.isOnTrans = false;
            }
            else
            {
                updateLog("Please Begin a transaction.");
            }

        }

        private void updateLog(string message)
        {
            Log.Text += message + "\r\n";
        }

        private void updatePadintStack(string message)
        {
            padintStack.Text += message + "\r\n";
        }

        private void InitButton_Click(object sender, EventArgs e)
        {
            string masterIP = masterIPtxt.Text;
            string masterPort = masterPortTxt.Text;

            PadiDstm.Init();
        }
    }
}
