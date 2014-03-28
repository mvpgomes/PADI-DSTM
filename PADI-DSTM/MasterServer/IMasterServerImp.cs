using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadiDstmLibrary;

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

        public bool Init()
        {
            throw new NotImplementedException();
        }

        public bool Status()
        {
            throw new NotImplementedException();
        }


        public string RegDataServer(string url)
        {
            throw new NotImplementedException();
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
        public void ObjCreatedSuccess(string url, int uid)
        {
            this.objectLocation.Add(uid, url);
        }
    }
}
