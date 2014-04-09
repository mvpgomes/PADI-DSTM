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

            PadInt pi_a = PadiDstm.CreatePadInt(0);
            PadInt pi_b = PadiDstm.CreatePadInt(1);
           
            PadiDstm.Status();
            // The following 3 lines assume we have 2 servers: one at port 2001 and another at port 2002
            res = PadiDstm.Freeze("tcp://localhost:8001/DataServer");

            PadInt pi_c = PadiDstm.CreatePadInt(2);
            Console.WriteLine("Olá");
            PadInt pi_d = PadiDstm.CreatePadInt(3);
            Console.WriteLine("Mundo !");
            res = PadiDstm.Recover("tcp://localhost:8001/DataServer");
            res = PadiDstm.Fail("tcp://localhost:8001/DataServer");
         
            Console.ReadLine();


        }
    }
}
