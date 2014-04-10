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
       
        private Dictionary<int, string> serverAddress;
        private Dictionary<int, string> objectLocation;
        private Dictionary<int, int> versionVector;

        private TransactionManager tm;
     
        private IMasterServerImp() 
        {
            this.serverAddress = new Dictionary<int, string>();
            this.objectLocation = new Dictionary<int,string>();
            this.versionVector = new Dictionary<int, int>();

            this.tm = new TransactionManager();

            this.dataServerId = 0;
        }

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

        public int RegisterDataServer(string url)
        {
            int serverId = this.dataServerId;
            this.serverAddress.Add(this.dataServerId, url);
            this.dataServerId++;
            return serverId;
        }


        public string getDataServerAddress()
        {
            int minValue = 2048;
            int serverID = 0;
            foreach (KeyValuePair<int, int> entry in versionVector)
            {
                if (entry.Value <= minValue)
                    minValue = entry.Value;
                    serverID = entry.Key;
            }
            return this.serverAddress[serverID];
        }

        /**
         * Method that receives the id from a DataServer and 
         * returns its respective URL.
         **/
        public string GetServerAddr(int serverID)
        {
            return this.serverAddress[serverID];
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
        public bool ObjCreatedSuccess(string url, int uid){
            this.objectLocation.Add(uid, url);
            //TODO We need to change this!!!
            return true;
        }

        /**
         * Method that returns the DataServer address where the PadInt
         * with id UID is stored.
         **/
        public string getPadIntLocation(int uid){
            return this.objectLocation[uid];
        }
        
        public Transaction OpenTransaction()
        {
            TID tid = TidGenerator.GenerateTID();
            Transaction trans = new Transaction(tid);
            tm.AddTransaction(trans);

            return trans;
        }

        public bool CloseTransaction(Transaction trans)
        {
            throw new NotImplementedException();
        }

        public void AbortTransaction(Transaction trans)
        {
            throw new NotImplementedException();
        }

        public void Join(Transaction trans, int participant)
        {
            trans.AddParticipant(participant);
        }

        public void HaveCommitted(Transaction trans, int participant)
        {
            throw new NotImplementedException();
        }

        public bool GetDecision(Transaction trans)
        {
            throw new NotImplementedException();
        }
    }
}
