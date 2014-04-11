using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace PADI_DSTM
{
    public class PadiDstm
    {
        public static readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";
        
        private static IMasterServer remoteMaster;
        private static TcpChannel channel;
        private static Dictionary<int, PadInt> cachedObjects;
        
        
        static PadiDstm() { }

        public static bool Init()
        {
            channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);
            cachedObjects = new Dictionary<int, PadInt>();
            return true;
        }

        public static bool TxBegin()
        {
            return true;
        }

        public static bool TxCommit()
        { 
            return true;
        }

        /**
         * Method that makes all nodes in the system dump to their output
         * their current state.
         **/ 
        public static bool Status()
        {
            IMasterServer remoteMaster = getMasterInstance();
            bool answer = remoteMaster.ShowDataServersState();
            return answer;
        }

        /**
         * Method that makes the serfver at the URL stop responding to external
         * calls wxcept for a Recover call.
         **/ 
        public static bool Fail(string URL)
        {
            IDataServer remoteServer = getDataServerInstance(URL);
            return remoteServer.Disconnect();
        }

        /**
         * Method that makes the server at the URL stop responding to external calls
         * but it maintains all calls for later reply. 
         **/ 
        public static bool Freeze(string URL)
        {
            IDataServer remoteServer = getDataServerInstance(URL);
            return remoteServer.FreezeDataServer();
        }

        /**
         * Method that makes the server at URL recover from a previous Fail or Freeze call.
         **/ 
        public static bool Recover(string URL)
        {
            IDataServer remoteServer = getDataServerInstance(URL);
            return remoteServer.RecoverDataServer();
        }

        /**
         * Create PadInt
         * Arguments: uid - PadInt identifier
         * Objective: Create a new PadInt
         * First we ask Master Server for a location.
         * After stablishing connection to Data Server, that server asks Master Server if that object already exists.
         * If it already exist: A null message is returned
         * If does not exist: 1) A new PadInt is created with uid as identifier
         *                    2) A reference to that object is returned to the client 
         */
        public static PadInt CreatePadInt(int uid)
        {
            PadInt reference = null;

            try
            {
               IMasterServer remoteMaster = getMasterInstance();
               string dataServerAddress = remoteMaster.GetDataServerAddress();
               Console.WriteLine(dataServerAddress);
               IDataServer remoteServer = getDataServerInstance(dataServerAddress); 
               reference = remoteServer.CreateObject(uid);
               cachedObjects.Add(uid, reference);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return reference;
        }

        /**
         * Access PadInt
         * Arguments: uid - PadInt identifier
         * Objective: Access some PadInt
         * If the client already contains PadInt location: an access to that location is performed
         * If the client doesn't have that information: 1) He asks Master Server for it.
         *                                              2) After saving that information, cliente requests Data Server for PadInt reference.
         *                                              3) Client have not a reference for that PadInt.
         */
        public static PadInt AccessPadInt(int uid)
        {
            PadInt reference = null;

            try
            {
                if (!cachedObjects.ContainsKey(uid))
                {


                    IMasterServer remoteMaster = getMasterInstance();
                    string dataServerAddress = remoteMaster.GetPadIntLocation(uid);

                    IDataServer remoteServer = getDataServerInstance(dataServerAddress);

                    reference = remoteServer.AccessObject(uid);
                }

            }
            catch (Exception) { return null; }
            
            return reference;
        }

        /**
         * Method that returns an remote object instance from the Master Server. 
         **/ 
        private static IMasterServer getMasterInstance()
        {
            if (remoteMaster == null)
            {
                remoteMaster = (IMasterServer)Activator.GetObject(
                         typeof(IMasterServer),
                         MASTER_SERVER_ADDRESS);
            }
            return remoteMaster;
        }

        /**
         * Method that returns an remote object instance from the Data Server registered at the
         * url DATA_SERVER_ADDRESS. 
         **/ 
        private static IDataServer getDataServerInstance(string DATA_SERVER_ADDRESS)
        {
            IDataServer remoteServer = (IDataServer)Activator.GetObject(
                     typeof(IDataServer),
                     DATA_SERVER_ADDRESS);

            return remoteServer;
        }
    }
}
