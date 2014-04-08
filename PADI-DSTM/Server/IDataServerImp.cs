using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using CommonTypes;

namespace DataServer
{
    class IDataServerImp : MarshalByRefObject, IDataServer
    {
        private readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";

        private Dictionary<int, PadInt> padIntDB;

          public IDataServerImp()
        {
           this.padIntDB = new Dictionary<int, PadInt>();
        }

        /**
         * Method that allows a user to create a new PadInt
         * object in the DataServer;
         **/
        public PadInt createObject(int uid)
        {
            if (!PermissionToCreate(uid))
            {
                PadInt padIntObject = new PadInt(uid);
                this.padIntDB.Add(uid, padIntObject);
                NotifyMasterServer(DataServer.DATA_SERVER_ADDRESS, uid);
                Console.WriteLine("Object with id " + uid + " created with sucess"); ;
                return padIntObject;
            }
            else
            {
                Console.WriteLine("ERROR: The object with id " + uid + " already exists.");
                return null;
            }
        }


        public PadInt accessObject(int uid)
        {
            return this.padIntDB[uid];
        }
    
        /**
         * Method that verifies if the a PadInt object with identifier 
         * uid can be created at the DataServer.
         **/
        public bool PermissionToCreate(int uid)
        {
            bool answerRequest = false;
            IMasterServer remoteObject;
            remoteObject = (IMasterServer)Activator.GetObject(
                typeof(IMasterServer),
                MASTER_SERVER_ADDRESS);
            answerRequest = remoteObject.ObjectExists(uid);
            return answerRequest;
        }

        /**
         * Method that notify the MasterServer when a PadInt object is created
         * sucessfully.
         **/
        public void NotifyMasterServer(string url, int uid)
        {
            IMasterServer remoteObject;
            remoteObject = (IMasterServer)Activator.GetObject(
                typeof(IMasterServer),
                MASTER_SERVER_ADDRESS);
            try
            {
                remoteObject.ObjCreatedSuccess(url, uid);
            }
            catch (Exception e)
            {
                Console.WriteLine("The remote call throw the exception : " + e);
            }
        }

        /**
         * Method that shows the state of the DataServer.
         * The DataServer state is described by its id and the id's of the PadInt's that 
         * are stored in. 
         **/
        public bool DumpState()
        {
            Console.WriteLine("Data Server ID : " + DataServer.DATA_SERVER_ID);
            Console.WriteLine("Stored PadInt's in this Server :");  
            foreach (int id in padIntDB.Keys)
            {
                Console.WriteLine("ID : " + id);
            }
            return true;
        }

        /**
         * Method that makes the Data Server disconnect from the current channel and
         * after that register itself in a secret channel to accept recover calls from
         * the PADI-DSTM lib. This method returns the port from the secret channel.
         **/
        public string Disconnect()
        {
            TcpChannel newChannel = new TcpChannel(9000);
            ChannelServices.UnregisterChannel(DataServer.channel);  
            ChannelServices.RegisterChannel(newChannel, false);
            return "tcp://" + System.Environment.MachineName + ":" + "9000" + "/DataServer"; ;
        }
    }
}
