using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace DataServer
{
    class DataServer
    {
        public static TcpChannel channel;
        public static int DATA_SERVER_ID;
        private static string port;
        public static string DATA_SERVER_ADDRESS;

        private static readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";

        public DataServer() { }

        static void Main(string[] args)
        {
            // Fix the port assignement
            port = "8081";
            DATA_SERVER_ADDRESS = "tcp://" + System.Environment.MachineName + ":" + port + "/DataServer";

            channel = new TcpChannel(Convert.ToInt32(port));
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(IDataServerImp),
                "DataServer", WellKnownObjectMode.Singleton);

            /**
             *  DataServer register itself in the MasterServer
             **/
            IMasterServer remoteMaster = (IMasterServer)Activator.GetObject(
                                                typeof(IMasterServer),
                                                 MASTER_SERVER_ADDRESS);

            try
            {
                DATA_SERVER_ID = remoteMaster.RegisterDataServer(DATA_SERVER_ADDRESS);
            }
            catch (Exception e) { Console.WriteLine(e.StackTrace); }

            System.Console.WriteLine("<enter> DataServer is running...");
            System.Console.ReadLine();
        }
    }
}
