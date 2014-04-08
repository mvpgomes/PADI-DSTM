using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using PADI_DSTM;

namespace DataServer
{
    class DataServer
    {

        private static int DATA_SERVER_ID;
        private static string port;
        private static string DATA_SERVER_ADDRESS;

        private static readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";

        static void Main(string[] args){
            //TODO Change this
            port = "0001";
            //Data Server Adress 
            DATA_SERVER_ADDRESS = "tcp://" + System.Environment.MachineName + ":" + port + "/DataServer";

            //TCP Channel setup and registing that channel
            TcpChannel channel = new TcpChannel(Convert.ToInt32(port));
            ChannelServices.RegisterChannel(channel, false);

            //Registing the service
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(IDataServerImp),
                "DataServer", WellKnownObjectMode.Singleton);

            /**
             *  DataServer register itself in the MasterServer
             **/
            IMasterServer remoteMaster = (IMasterServer)Activator.GetObject(
                                                typeof(IMasterServer),
                                                 MASTER_SERVER_ADDRESS);

            try{
                DATA_SERVER_ID = remoteMaster.RegisterDataServer(DATA_SERVER_ADDRESS);

                /*
                //Testing
                IDataServer remoteData = (IDataServer)Activator.GetObject(
                                                typeof(IDataServer),
                                                 DATA_SERVER_ADDRESS);

                Console.WriteLine(PadiDstm.serializeObjects(DATA_SERVER_ADDRESS, remoteData.returnPadIntDB()));
                 * */

            }
            catch (Exception e) { 
                Console.WriteLine(e.StackTrace); 
            }

            System.Console.WriteLine("<enter> DataServer is running...");
            System.Console.ReadLine();
        }
    }
}
