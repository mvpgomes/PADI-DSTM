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
        public static int dataServerID;
        private static string port;
        public static string dataServerAddr;
        

        private static readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";

        static void Main(string[] args){

            Console.Write("Run Data Server at port: ");
            port = Console.ReadLine();
            
            //Data Server Adress 
            dataServerAddr = "tcp://" + System.Environment.MachineName + ":" + port + "/DataServer";
          
            //TCP Channel setup and registing that channel
            channel = new TcpChannel(Convert.ToInt32(port));
            ChannelServices.RegisterChannel(channel, false);

            //Registering the service
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(IDataServerImp),
                "DataServer", WellKnownObjectMode.Singleton);

            Console.WriteLine("Data Server Running at: " + dataServerAddr);

            //DataServer register itself in the MasterServer 
            IMasterServer remoteMaster = (IMasterServer)Activator.GetObject(
                                                typeof(IMasterServer),
                                                 MASTER_SERVER_ADDRESS);

            try
            {
                dataServerID = remoteMaster.RegisterDataServer(dataServerAddr);
            }
            catch (Exception e) 
            { 
                Console.WriteLine("DataServer Main Exception: " + e.Message); 
            }

            System.Console.WriteLine("<enter> DataServer is running...");
            System.Console.ReadLine();
        }
    }
}
