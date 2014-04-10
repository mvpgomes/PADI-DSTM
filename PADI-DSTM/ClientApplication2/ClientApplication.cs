using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PADI_DSTM;
using CommonTypes;

namespace ClientApplication
{
    class ClientApplication
    {
        static void Main(string[] args)
        {
            bool res;

            PadiDstm.Init();

            // The following 3 lines assume we have 2 servers: one at port 2001 and another at port 2002
            res = PadiDstm.Fail("tcp://localhost:8001/DataServer");
            res = PadiDstm.Recover("tcp://localhost:8001/DataServer");
            PadInt p_a = PadiDstm.CreatePadInt(2); 
         
            Console.ReadLine();


        }
    }
}
