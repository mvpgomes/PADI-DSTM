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
        private static IMasterServerImp instance;

        private IMasterServerImp() {}
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
