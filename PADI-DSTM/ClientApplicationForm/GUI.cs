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
using CustomExceptions;

namespace ClientApplicationForm
{
    public partial class GUI : Form
    {
        private Dictionary<int, PadInt> cachedObjects;
        private bool isOnTrans;

        public GUI()
        {
            InitializeComponent();
            this.cachedObjects = new Dictionary<int, PadInt>();
            this.isOnTrans = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        { }

        private void updateLog(string message)
        {
            Log.Text += message + "\r\n";
        }

        private void updatePadintStack(string message)
        {
            padintStack.Text += message + "\r\n";
        }

        private void CreatePadInt_Click(object sender, EventArgs e)
        {
            string PadIntID = uidTxt.Text;

            if (!this.isOnTrans)
            {
                updateLog("Please begin a transaction first.");
                updatePadintStack("");
                return;
            }

            if (PadIntID.Length == 0)
            {
                updateLog("Please insert a valid ID.");
                updatePadintStack("");
                return;
            }

            PadInt res = PadiDstm.CreatePadInt(Convert.ToInt32(PadIntID));

            if (res == null)
            {
                updateLog("The PadInt with id " + PadIntID + " already exists.");
                updatePadintStack("");
            }
            else
            {
                updateLog("PadInt created.");
                updatePadintStack(res.ToString());
                cachedObjects.Add(res.getUID(), res);
            }
        }

        private void AccessPadInt_Click(object sender, EventArgs e)
        {
            if (!this.isOnTrans)
            {
                updateLog("Please begin a transaction first.");
                updatePadintStack("");
                return;
            }
            string PadIntID = uidTxt.Text;
            if (PadIntID.Length == 0)
            {
                updateLog("Please insert a valid ID.");
                updatePadintStack("");
                return;
            }
            PadInt res = null;
            int uid = Convert.ToInt32(PadIntID);

            if (!cachedObjects.ContainsKey(uid))
            {
                res = PadiDstm.AccessPadInt(uid);
                cachedObjects.Add(uid, res);
            }
            else
            {
                res = cachedObjects[Convert.ToInt32(PadIntID)];
            }

            if (res == null)
            {
                updateLog("The PadInt with id " + PadIntID + " does not exist.");
                updatePadintStack("");
            }

            updateLog("PadInt Accessed.");
            updatePadintStack(res.ToString());
        }

        private void Fail_Click(object sender, EventArgs e)
        {
            string URL = URLTextBox.Text;

            if (URL.Length == 0)
            {
                updateLog("Please insert a valid URL.");
                updatePadintStack("");
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
                updatePadintStack("");
                return;
            }


            bool res = PadiDstm.Freeze(URL);

            if (res == false)
            {
                updateLog("The request could not be completed.");
                updatePadintStack("");
            }
        }

        private void Recover_Click(object sender, EventArgs e)
        {
            string URL = URLTextBox.Text;

            if (URL.Length == 0)
            {
                updateLog("Please insert a valid URL.");
                updatePadintStack("");
                return;
            }

            bool res = PadiDstm.Recover(URL);

            if (res == false)
            {
                updateLog("The request could not be completed.");
                updatePadintStack("");
            }
        }

        private void Status_Click(object sender, EventArgs e)
        {
            bool res = PadiDstm.Status();

            if (res == false)
            {
                updateLog("The request could not be completed.");
                updatePadintStack("");
            }
        }

        private void TxAbort_Click(object sender, EventArgs e)
        {
            if (this.isOnTrans)
            {
                bool response = PadiDstm.TxAbort();
                if (response)
                {
                    this.isOnTrans = false;
                    this.cachedObjects.Clear();
                    updateLog("Transaction Aborted.");
                    updatePadintStack("");
                }
                else
                {
                    updateLog("Transaction Abort failed.");
                    updatePadintStack("");
                }
            }
            else
            {
                updateLog("Please Begin a transaction first.");
                updatePadintStack("");
            }
        }

        private void TxBegin_Click(object sender, EventArgs e)
        {
            try
            {
                PadiDstm.TxBegin();
                this.isOnTrans = true;
                updateLog("Transaction Started.");
                updatePadintStack("");
            }
            catch (TxException ex) { updateLog(ex.Message); }
        }

        private void TxCommit_Click(object sender, EventArgs e)
        {
            if (this.isOnTrans)
            {
                try
                {
                    bool res = PadiDstm.TxCommit();
                    this.isOnTrans = false;
                    this.cachedObjects.Clear();

                    if (res) { 
                    updateLog("Transaction Committed."); 
                    updatePadintStack(""); 
                    }
                }
                catch (TxException ex) { updateLog(ex.Message); }
            }
            else
            {
                updateLog("Please Begin a transaction first.");
                updatePadintStack("");
            }
        }

        private void InitButton_Click(object sender, EventArgs e)
        {
            string masterIP = masterIPtxt.Text;
            string masterPort = masterPortTxt.Text;

            PadiDstm.Init();
        }

        private void WritePadInt(object sender, EventArgs e)
        {
            if (!this.isOnTrans)
            {
                updateLog("Please begin a transaction first.");
                updatePadintStack("");
                return;
            }
            if (uidTxt.Text.Length == 0 || valueTxt.Text.Length == 0)
            {
                updateLog("Please insert valid values.");
                updatePadintStack("");
                return;
            }

            int uid = Convert.ToInt32(uidTxt.Text);
            int value = Convert.ToInt32(valueTxt.Text);

            try
            {
                PadInt obj = cachedObjects[uid];
                obj.Write(value);
                updateLog("Write operation.");
                updatePadintStack(obj.ToString());
            }
            catch (Exception) {
            updateLog("The PadInt with id " + uid + " does not exist.");
            updatePadintStack("");
            }
        }

        private void ReadPadInt(object sender, EventArgs e)
        {
            if (!this.isOnTrans)
            {
                updateLog("Please begin a transaction first.");
                updatePadintStack("");
                return;
            }
            if (uidTxt.Text.Length == 0)
            {
                updateLog("Please insert valid ID.");
                updatePadintStack("");
                return;
            }

            int uid = Convert.ToInt32(uidTxt.Text);

            try
            {
                PadInt obj = cachedObjects[uid];
                updateLog("Read operation.");
                updatePadintStack(obj.Read().ToString());
            }
            catch (Exception) { 
            updateLog("The PadInt with id " + uid + " does not exist.");
            updatePadintStack("");
            }
        }
    }
}
