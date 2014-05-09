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
    public class DataServer
    {
        private static string PRIMARY_ROLE = "Primary";
        private static string BACKUP_ROLE = "Backup";

        private static int PORT_INDEX = 0;
        private static int ID_INDEX = 1;

        public static TcpChannel channel;
        public static string port;
        private static int dataServerID;
        private static string role;

        public static IMasterServer getRemoteMasterInstance()
        {
            //getting master server proxy  
            IMasterServer remoteMaster = (IMasterServer)Activator.GetObject(
                       typeof(IMasterServer),
                       PadiDstm.MASTER_SERVER_ADDRESS);

            return remoteMaster;
        }


        public static int registerServerAtMaster(string addr)
        {
            int serverID;

            try
            {
                IMasterServer remoteMaster = getRemoteMasterInstance();
                serverID = remoteMaster.RegisterDataServer(addr);
            }
            catch (Exception e)
            {
                Console.WriteLine("DataServer Main Exception: " + e.Message);
                Console.WriteLine("<enter> to exit...");
                Console.ReadLine();
                return -1;
            }

            Console.WriteLine("Data Server has registered on Master with id: " + dataServerID);

            return serverID;
        }

        public static void createReplicaServer(IDataServerImp server, string port)
        {
            try
            {
                IMasterServer remoteMaster = getRemoteMasterInstance();
                Console.WriteLine("Creating the replica ...");
                string replicaAddr = remoteMaster.CreateDataServerReplica(dataServerID, Convert.ToInt32(port));
                Console.WriteLine("Replica address " + replicaAddr);
                server.ReplicaAddress = replicaAddr;
            }
            catch (Exception e)
            {
                Console.WriteLine("DataServer Main Exception: " + e.Message);
                Console.WriteLine("<enter> to exit...");
                Console.ReadLine();
                return;
            }
        }

        public static void Main(string[] args){

            // Get the server port
            if (args.Length == 0) { 
                // In the case that the primary will be launched, the port is read 
                // from the user input 
                Console.Write("Run Data Server at port: ");
                port = Console.ReadLine();
            }
            else
            {
                // In the casw of the replica, the port is passed as argument
                Console.WriteLine(args[PORT_INDEX]);
                port = args[PORT_INDEX];
            }


            // Get the server role : PRIMARY or BACKUP
            if (args.Length == 0)
            {
                role = PRIMARY_ROLE;
            }
            else
            {
                role = BACKUP_ROLE;
            }

            //Create the communication channel 
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

            if (args.Length == 0) {
                dataServerID = registerServerAtMaster(dataServerAddr);   
            }
            else
            {
                dataServerID = Convert.ToInt32( args[ID_INDEX] );
            }

            Console.WriteLine(dataServerAddr);

            //Creates the server
            IDataServerImp dataServer = new IDataServerImp(dataServerID, role, dataServerAddr);

            // Creates a replica if this server is the primary
            if (role == PRIMARY_ROLE)
            {
                dataServer.updatePrimaryState(true);
                createReplicaServer(dataServer, port);
            }
           
            //Registering the service
            RemotingServices.Marshal(dataServer, "DataServer", typeof(IDataServerImp));

            //Starts to execute the TimerTask;
            if (role == PRIMARY_ROLE)
            {
                dataServer.RunTimerPrimary();
            }
            else
            {
                System.Threading.Thread.Sleep(5000);
                dataServer.RunTimerBackup();
            }

            Console.WriteLine("Data Server Running at: " + dataServerAddr);

            System.Console.WriteLine("<enter> DataServer is running...");
            System.Console.ReadLine();
        }
    }
}
