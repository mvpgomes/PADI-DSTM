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

        public string GetServerAddr()
        {
            throw new NotImplementedException();
        }

        public bool ExistObject(int uid)
        {
            throw new NotImplementedException();
        }

        public void ObjCreatedSuccess(string url, int uid)
        {
            throw new NotImplementedException();
        }
    }
}
