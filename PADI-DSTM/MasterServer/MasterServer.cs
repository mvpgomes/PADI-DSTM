using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace MasterServer
{

    class MasterServer
    {
        static void Main(string[] args)
        {
           try
           {
               //registering server at port 8086
               TcpChannel channel = new TcpChannel(8086);
               ChannelServices.RegisterChannel(channel, false);
           }
           catch (Exception e)
           {
               Console.WriteLine("MasterServer exception: " + e.Message);
               Console.WriteLine("<enter> to exit ...");
               Console.ReadLine();
               return;
           }
            
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(IMasterServerImp),
               "MasterServer", WellKnownObjectMode.Singleton);

            System.Console.WriteLine("<enter> MasterServer is running...");
            System.Console.WriteLine("The machine name is : " + System.Environment.MachineName);
            System.Console.ReadLine();
        }
    }
}
