using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadiDstmLibrary;

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
    }
}
