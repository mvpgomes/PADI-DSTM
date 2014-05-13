using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using CommonTypes;
using DataServer;
using System.Diagnostics;
using System.IO;

namespace MasterServer
{
    /// <summary>
    /// TidGenerator
    /// </summary>
    class TidGenerator
    {
        static private int count = 0;

        public static TID GenerateTID()
        {
            return new TID(TidGenerator.count++);
        }
    }

    /// <summary>
    /// Specification in CommonTypes
    /// </summary>
    class IMasterServerImp : MarshalByRefObject, IMasterServer
    {
        private static string ServerPath = Directory.GetCurrentDirectory() + "\\Server.exe";
        private static string BACKUP_ROLE = "Backup";
        private static readonly int PORT_INCREMENT = 1000;

        private int dataServerId;
        private static IMasterServerImp instance;

        private Dictionary<int, string> primaryServerAddress;
        private Dictionary<int, string> backupServerAddress;
        private Dictionary<int, string> objectLocation;
        private Dictionary<int, int> storageVector;

        private TransactionManager transactionManager;

        private IMasterServerImp()
        {
            this.primaryServerAddress = new Dictionary<int, string>();
            this.backupServerAddress = new Dictionary<int, string>();
            this.objectLocation = new Dictionary<int, string>();
            this.storageVector = new Dictionary<int, int>();
            this.dataServerId = 0;
            this.transactionManager = new TransactionManager();
        }

        /**
         * Method that returns the single instance of the Master Server.
         **/
        public static IMasterServerImp Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IMasterServerImp();
                }
                return instance;
            }
        }

        /**
         * Method that register a Data Server and store its 
         * URL in the well know servers structure.
         **/
        public int RegisterDataServer(string url)
        {
            int serverId = this.dataServerId;
            this.primaryServerAddress.Add(serverId, url);
            this.storageVector.Add(serverId, 0);
            this.dataServerId++;
            Console.WriteLine("The server hosted at " + url + " was registred with sucess !");
            return serverId;
        }


        /**
         * Method that returns the address of the data server that has less
         * PadInt's objects stored.
         **/
        public string GetDataServerAddress()
        {
            int minValue = 2048;
            int serverID = 0;
            foreach (KeyValuePair<int, int> entry in storageVector)
            {
                if (entry.Value < minValue)
                {
                    minValue = entry.Value;
                    serverID = entry.Key;
                }
            }
            return this.primaryServerAddress[serverID];
        }

        /**
         * Method that receives the id from a DataServer and 
         * returns its respective URL.
         **/
        public int GetDataServerId(string url)
        {
            int serverID = 0;

            foreach (KeyValuePair<int, string> entry in primaryServerAddress)
            {
                if (entry.Value == url)
                {
                    serverID = entry.Key;
                }
            }

            return serverID;
        }

        /**
         * Method that receives the uid from a PadInt and returns 
         * true if the object PadInt with identifier uid exists,
         * otherwise returns false.
         **/
        public bool ObjectExists(int uid)
        {
            return this.objectLocation.ContainsKey(uid);
        }

        /**
         * Method that updates the information at the MasterServer
         * when a PadInt object is created in a server.
         **/
        public bool ObjCreatedSuccess(string url, int uid)
        {
            this.objectLocation.Add(uid, url);
            // Update the storageVector
            int serverID = GetDataServerId(url);
            this.storageVector[serverID]++;
            return true;
        }

        /**
         * Method that returns the DataServer address where the PadInt
         * with id UID is stored.
         **/
        public string GetPadIntLocation(int uid)
        {
            try
            {
                return this.objectLocation[uid];
            }
            catch (Exception) { return null; }
        }

        /**
         * Method that queries all the registered DataServers to dump
         * it actual state.
         **/
        public bool ShowDataServersState()
        {
            bool answer = false;
            try
            {
                foreach (string url in this.primaryServerAddress.Values)
                {
                    IDataServer remoteServer = GetDataServerInstance(url);
                    answer = remoteServer.DumpState();
                }
            }
            catch (Exception e) { Console.WriteLine(e.StackTrace); }

            return answer;
        }

        /**
         * Method that returns an remote object instance to the 
         * Data Server registered at url.
         **/
        public IDataServer GetDataServerInstance(string url)
        {

            IDataServer remoteServer = (IDataServer)Activator.GetObject(
                      typeof(IDataServer),
                                     url);

            return remoteServer;
        }

        /**
         * Method that generates a new DataServer Address
         **/
        public string GenerateAddress(int port)
        {
            int newPort = port + PORT_INCREMENT;
            return "tcp://" + System.Environment.MachineName + ":" + newPort.ToString() + "/DataServer";
        }

        /**
         * Method that creates a DataServer new instance to
         * be assigned as replica.
         **/
        public string CreateDataServerReplica(int primaryID, int primaryPort)
        {

            string backupAddress = GenerateAddress(primaryPort);
            this.backupServerAddress[primaryID] = backupAddress;

            int port = primaryPort + 1000;

            Process replicaProcess = new Process();

            try
            {
                replicaProcess.StartInfo.UseShellExecute = true;
                replicaProcess.StartInfo.FileName = ServerPath;
                replicaProcess.StartInfo.CreateNoWindow = false;
                // The parameters are passed as : PORT, ID 
                string arguments = port.ToString() + " " + primaryID.ToString();
                replicaProcess.StartInfo.Arguments = arguments;
                replicaProcess.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return backupAddress;
        }


        public void AddWriteToTrans(int uid, int value, TID tid)
        {
            transactionManager.GetTransaction(tid).AddWrite(uid, value);
        }

        public void AddReadToTrans(int uid, TID tid)
        {
            transactionManager.GetTransaction(tid).AddRead(uid);
        }

        public void notifyMasterAboutFailure(int id, string address)
        {
            string primaryAddress = this.primaryServerAddress[id];
            string backupAddress = this.backupServerAddress[id];

            if (!primaryServerAddress.Equals(address) && backupServerAddress.Equals(address))
            {
                this.primaryServerAddress[id] = backupAddress;
                this.backupServerAddress.Remove(id);
            }
        }


        public TID OpenTransaction()
        {
            //get lock
            TID tid = TidGenerator.GenerateTID();
            //release lock

            Transaction trans = new Transaction(tid, transactionManager.GetLastCommit());
            transactionManager.AddTransaction(trans);

            return tid;
        }

        public bool CloseTransaction(TID tid)
        {
            bool result = false;

            //get lock
            if (this.transactionManager.IsValidTransaction(tid))
            {
                foreach (KeyValuePair<int, TranscationPadInt> entry in this.transactionManager.GetTransaction(tid).WriteValues())
                {
                    IDataServer dataServer = this.GetDataServerInstance(this.GetPadIntLocation(entry.Key));
                    dataServer.DoCommit(entry.Value);
                }
                this.transactionManager.SetLastCommit(tid);
                result = true;
            }
            //release lock

            return result;
        }

        public void AbortTransaction(TID tid)
        {

        }

        public void Join(TID tid, int participant)
        {
            Transaction trans = this.transactionManager.GetTransaction(tid);
            if (trans != null)
            {
                trans.AddParticipant(participant);
            }
        }

        public void HaveCommitted(TID tid, int participant)
        {

        }

        public bool GetDecision(TID tid)
        {
            return true;
        }
    }
}
