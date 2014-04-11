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
        public static TcpChannel channel;
        private static int dataServerID;
        static void Main(string[] args){

            //Getting the port number from user
            Console.Write("Run Data Server at port: ");
            string port = Console.ReadLine();

            //Firstly MUST create the channel 
            try
            {
                channel = new TcpChannel(Convert.ToInt32(port));
                ChannelServices.RegisterChannel(channel, false);
            }
            catch (Exception e)
            {
                Console.WriteLine("DataServer Main Exception: " + e.Message);
                Console.WriteLine("<enter> to exit...");
                Console.ReadLine();
                return;
            }
            
            //Creating Data Server Address 
            string dataServerAddr = "tcp://" + System.Environment.MachineName + ":" + port + "/DataServer";
            
            try
            {
                //getting master server proxy  
                IMasterServer remoteMaster = (IMasterServer)Activator.GetObject(
                    typeof(IMasterServer),
                    PadiDstm.MASTER_SERVER_ADDRESS);

                dataServerID = remoteMaster.RegisterDataServer(dataServerAddr);    
            }
            catch (Exception e)
            {
                Console.WriteLine("DataServer Main Exception: " + e.Message);
                Console.WriteLine("<enter> to exit...");
                Console.ReadLine();
                return;
            }
            
            Console.WriteLine("Data Server has registered on Master with id: " + dataServerID);

            //Finally creates the server
            IDataServerImp dataServer = new IDataServerImp(dataServerID, dataServerAddr);
          
            //Registering the service
            RemotingServices.Marshal(dataServer, "DataServer",
            typeof(IDataServerImp));

            Console.WriteLine("Data Server Running at: " + dataServerAddr);

            System.Console.WriteLine("<enter> DataServer is running...");
            System.Console.ReadLine();
        }
    }
}
