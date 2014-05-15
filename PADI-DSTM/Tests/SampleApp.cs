using System;
using CommonTypes;

class SampleApp {
    static void Main(string[] args) {
        bool res;
        PadInt pi_a, pi_b;
        PadiDstm.Init();

       string[] arg = new string[] {"C"}; 

        // Create 2 PadInts
        if ((arg.Length > 0) && (arg[0].Equals("C"))) {
            res = PadiDstm.TxBegin(); 
            pi_a = PadiDstm.CreatePadInt(12);
            pi_b = PadiDstm.CreatePadInt(2000000002);
            Console.WriteLine("####################################################################");
            Console.WriteLine("BEFORE create commit. Press enter for commit.");
            Console.WriteLine("####################################################################");
            PadiDstm.Status();
            Console.ReadLine();
            res = PadiDstm.TxCommit();
            Console.WriteLine("####################################################################");
            Console.WriteLine("AFTER create commit. commit = " + res + " . Press enter for next transaction.");
            Console.WriteLine("####################################################################");
            Console.ReadLine();
        }


        res = PadiDstm.TxBegin();
        pi_a = PadiDstm.AccessPadInt(1);
        pi_b = PadiDstm.AccessPadInt(2000000000);
        Console.WriteLine("####################################################################");
        Console.WriteLine("Status after AccessPadint");
        Console.WriteLine("####################################################################");
        PadiDstm.Status();
        if ((arg.Length > 0) && ((arg[0].Equals("C")) || (arg[0].Equals("A")))) {
            pi_a.Write(11);
            pi_b.Write(12);
        } else {
            pi_a.Write(21);
            pi_b.Write(22);
        }
        Console.WriteLine("####################################################################");
        Console.WriteLine("Status after write. Press enter for read.");
        Console.WriteLine("####################################################################");
        PadiDstm.Status();
        Console.WriteLine("1 = " + pi_a.Read());
        Console.WriteLine("2000000000 = " + pi_b.Read());
        Console.WriteLine("####################################################################");
        Console.WriteLine("Status after read. Press enter for commit.");
        Console.WriteLine("####################################################################");
        PadiDstm.Status();
        Console.ReadLine();
        res = PadiDstm.TxCommit();
        Console.WriteLine("####################################################################");
        Console.WriteLine("Status after commit. commit = " + res + "Press enter for verification transaction.");
        Console.WriteLine("####################################################################");
        Console.ReadLine();
        res = PadiDstm.TxBegin();
        PadInt pi_c = PadiDstm.AccessPadInt(1);
        PadInt pi_d = PadiDstm.AccessPadInt(2000000000);
        Console.WriteLine("####################################################################");
        Console.WriteLine("1 = " + pi_c.Read());
        Console.WriteLine("2000000000 = " + pi_d.Read());
        Console.WriteLine("Status after verification read. Press enter for commit and exit.");
        Console.WriteLine("####################################################################");
        PadiDstm.Status();
        Console.ReadLine();
        res = PadiDstm.TxCommit();
    }
}
