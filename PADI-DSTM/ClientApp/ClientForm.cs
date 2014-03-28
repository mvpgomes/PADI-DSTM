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
using PadiDstmLibrary;

namespace ClientApp
{
    public partial class ClientForm : Form
    {
        public static IDataServer remoteServer;
        private readonly string DATA_SERVER_ADDRESS = "tcp://localhost:8081/DataServer";

        public ClientForm()
        {
            InitializeComponent();
        }

        private void connectBut_Click(object sender, EventArgs e)
        {
            PadInt obj = null;
            remoteServer = (IDataServer)Activator.GetObject(
                typeof(IDataServer),
                DATA_SERVER_ADDRESS);
            try
            {
                obj = remoteServer.createPadInt(Convert.ToInt32(uidText.Text));

                if (obj.Equals(null))
                {
                    textLog.Text += "ERROR: The object with id " + uidText.Text + " already exists.";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("The remote call throw the exception : " + ex);
            }

        }

    }
}
