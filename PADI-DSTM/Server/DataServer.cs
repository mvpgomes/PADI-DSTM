using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace DataServer
{
    class DataServer
    {

        private static string port;
        private static string DATA_SEVER_ADDRESS;

        static void Main(string[] args)
        {
            port = args[0];
            DATA_SEVER_ADDRESS = "tcp://" + System.Environment.MachineName + ":" + port + "/DataServer";

            TcpChannel channel = new TcpChannel(Convert.ToInt32(port));
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(IDataServerImp),
                "DataServer", WellKnownObjectMode.Singleton);

            System.Console.WriteLine("<enter> DataServer is running...");
            System.Console.ReadLine();
        }
    }
}
