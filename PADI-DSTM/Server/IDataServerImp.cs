using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadiDstmLibrary;
using CommonTypes;

namespace DataServer
{
    class IDataServerImp : MarshalByRefObject, IDataServer
    {
        public IDataServerImp()
        {
            //initialize data
        }
        public PadInt createPadInt(int uid)
        {
            throw new NotImplementedException();
        }

        public PadInt accessPadInt(int uid)
        {
            throw new NotImplementedException();
        }

        public bool TxBegin()
        {
            throw new NotImplementedException();
        }

        public bool TxCommit()
        {
            throw new NotImplementedException();
        }

        public bool TxAbort()
        {
            throw new NotImplementedException();
        }

        public bool Fail(string URL)
        {
            throw new NotImplementedException();
        }

        public bool Freeze(string URL)
        {
            throw new NotImplementedException();
        }

        public bool Recover(string URL)
        {
            throw new NotImplementedException();
        }
    }
}
