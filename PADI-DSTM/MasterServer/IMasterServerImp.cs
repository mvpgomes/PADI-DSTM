using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace MasterServer
{
   
    class TidGenerator
    {
        static private int count = 0;

        public static TID GenerateTID()
        {
            return new TID(TidGenerator.count++);
        }
    }

    class TransactionManager
    {
        private Dictionary<TID, Transaction> trans;

        public TransactionManager()
        {
            this.trans = new Dictionary<TID, Transaction>();
        }

        public bool AddTransaction(Transaction trans)
        {
            bool res = false;

            try
            {
                this.trans.Add(trans.GetTID(), trans);
                res = true;
            }
            catch (Exception) { return res; }

            return res;
        }

        public void RemoveTransaction(Transaction trans)
        {
            this.trans.Remove(trans.GetTID());
        }

        public Transaction GetTransaction(TID tid)
        {
            Transaction t = null;
            try
            {
                t = this.trans[tid];
            }
            catch (Exception) { return null; }

            return t;
        }

        public override string ToString()
        {
            string aux = "";

            foreach (KeyValuePair<TID, Transaction> entry in this.trans)
            {
                aux += entry.Key.ToString() + entry.Value.ToString() + " | ";
            }
            return aux;
        }

    }


    class IMasterServerImp : MarshalByRefObject, IMasterServer
    {
        private int dataServerId;
        private static IMasterServerImp instance;
       
        private Dictionary<int, string> primaryServerAddress;
        private Dictionary<int, string> backupServerAddress;
        private Dictionary<int, string> objectLocation;
        private Dictionary<int, int> storageVector;

        private TransactionManager tm;

        private IMasterServerImp() {
            this.primaryServerAddress = new Dictionary<int, string>();
            this.backupServerAddress = new Dictionary<int, string>();
            this.objectLocation = new Dictionary<int,string>();
            this.storageVector = new Dictionary<int, int>();
            this.dataServerId = 0;
            this.tm = new TransactionManager();
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

        /////////////////////
        //Transaction Stuff//
        ////////////////////
        public TID OpenTransaction()
        {
            TID tid = TidGenerator.GenerateTID();
            Transaction trans = new Transaction(tid);
            tm.AddTransaction(trans);
            //tid encapsulates the information about a transaction
            return tid;
        }

        public bool CloseTransaction(TID tid)
        {
            Transaction trans = tm.GetTransaction(tid);

            foreach (int participant in trans.GetParticipants())
            {
                IDataServer dataServer = this.GetDataServerInstance(this.GetPadIntLocation(participant));
                //ask for vote
                if (dataServer.CanCommit(trans))
                {
                    dataServer.DoCommit(trans);
                    tm.RemoveTransaction(trans);                    
                }
            }
            
            return true;
        }

        public void AbortTransaction(TID tid)
        {
           
        }

        public void Join(TID tid, int participant)
        {
            Transaction trans = this.tm.GetTransaction(tid);
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

        public void LogWrite(TID tid, PadInt padint)
        {
            Transaction tran = this.tm.GetTransaction(tid);

            tran.AddWriteSet(padint);

            Console.WriteLine(tran.ToString());
        }
    }
}
