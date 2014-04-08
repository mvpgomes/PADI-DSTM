using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace MasterServer
{
    class IMasterServerImp : MarshalByRefObject, IMasterServer
    {
        private int dataServerId;
        private static IMasterServerImp instance;
       
        private Dictionary<int, string> serverAddress;
        private Dictionary<int, string> objectLocation;
        private Dictionary<int, int> versionVector;

        private IMasterServerImp() {
            this.serverAddress = new Dictionary<int, string>();
            this.objectLocation = new Dictionary<int,string>();
            this.versionVector = new Dictionary<int, int>();
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
            Console.WriteLine("The server hosted at " + url + " was registred with sucess !");
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
        public bool ObjCreatedSuccess(string url, int uid)
        {
            this.objectLocation.Add(uid, url);
            return true;
        }

        /**
         * Method that returns the DataServer address where the PadInt
         * with id UID is stored.
         **/
        public string getPadIntLocation(int uid)
        {
            return this.objectLocation[uid];
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
                foreach (string url in this.serverAddress.Values)
                {
                    IDataServer remoteServer = getDataServerInstance(url);
                    answer = remoteServer.DumpState();
                }
            }
            catch (Exception e) { Console.WriteLine(e.StackTrace); }
            return answer;
        }

        public IDataServer getDataServerInstance(string url)
        {
           
          IDataServer remoteServer = (IDataServer)Activator.GetObject(
                    typeof(IDataServer),
                                   url);
            
            return remoteServer;
        }
    }
}
