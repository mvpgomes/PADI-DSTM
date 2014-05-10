using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using CustomExceptions;

namespace CommonTypes
{
    /// <summary>
    /// PadiDstm
    /// </summary>
    public class PadiDstm
    {
        /*
         * PadiDstm variables
         */ 
        public static readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";
        private static TcpChannel channel;
        private static IMasterServer remoteMaster;
        private static TID currentTx;
        private static Dictionary<int, PadInt> accessedPadInts;
       
        /*
         * PadiDstm constructor.
         */ 
        static PadiDstm() 
        {
            accessedPadInts = new Dictionary<int, PadInt>();
        }
        /*
         * PadiDstm - Init.
         * this method is called only once by the application and initializes the PADI-DSTM library.
         * @return bool.
         */
        public static bool Init()
        {
            channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);
            currentTx = null;
            return true;
        }
        /*
         * PadiDstm - TxBegin.
         * this method starts a new transaction and returns a boolean value in-
         * dicating whether the operation succeeded. This method may throw a TxException. A
         * TxException should include a string indicating what caused the exception.
         * @return bool.
         * @throw TxException.
         */
        public static bool TxBegin()
        {
            remoteMaster = getMasterInstance();
            if (currentTx == null)
            {
                currentTx = remoteMaster.OpenTransaction();
                return true;
            }
            else
            {
                throw new TxException("The transaction is already opened.");
            }
        }
        /*
         * PadiDstm - sendDirtyPadints.
         * Sends PadInt that were modified.
         */
        private static void sendDirtyPadints()
        {
            foreach ( KeyValuePair<int, PadInt> padInt in accessedPadInts)
            {
                if (padInt.Value.wasRead() || padInt.Value.wasWrite())
                {
                    getMasterInstance().AddToTransaction(padInt.Value.getUID(), padInt.Value.wasWrite(), padInt.Value.Read(), currentTx);
                }
            }
        }
        /*
         * PadiDstm - TxCommit.
         * this method attempts to commit the current transaction and returns
         * a boolean value indicating whether the operation succeded. This method may throw a
         * TxException.
         * @return bool.
         * @throw TxException.
         */
        public static bool TxCommit()
        {
            remoteMaster = getMasterInstance();
            if (currentTx != null)
            {
                sendDirtyPadints();
                remoteMaster.CloseTransaction(currentTx);
                currentTx = null;
                return true;
            }
            else
            {
                throw new TxException("The transaction does not exist or has already aborted.");
            }
        }
        /**
         * PadiDstm - Status 
         * Method that makes all nodes in the system dump to their output
         * their current state.
         * @return bool.
         **/
        public static bool Status()
        {
            IMasterServer remoteMaster = getMasterInstance();
            bool answer = remoteMaster.ShowDataServersState();
            return answer;
        }
        /**
         * PadiDstm - Fail 
         * Method that makes the serfver at the URL stop responding to external
         * calls wxcept for a Recover call.
         * @return bool.
         */
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
            Console.WriteLine("trying create a padint");
            try
            {
                IMasterServer remoteMaster = getMasterInstance();
                string dataServerAddress = remoteMaster.GetDataServerAddress();
                Console.WriteLine(dataServerAddress);
                
                IDataServer remoteServer = (IDataServer)Activator.GetObject(
                    typeof(IDataServer), dataServerAddress);

                remoteServer.CreateObject(uid);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            //crete a local padint
            PadInt padint = new PadInt(uid);
            accessedPadInts.Add(uid, padint);

            return padint;
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
            PadInt padInt = null;
            PadIntServer padIntServer = null;

            //Have already acessed?
            if (accessedPadInts.ContainsKey(uid))
            {
                padInt = accessedPadInts[uid];
            }
            else
            {
                // otherwise create one
                try
                {
                    IMasterServer remoteMaster = getMasterInstance();
                    string dataServerAddress = remoteMaster.GetPadIntLocation(uid);
                    IDataServer remoteServer = getDataServerInstance(dataServerAddress);
                    padIntServer = remoteServer.AccessObject(uid);
                    //Do the conversion
                    padInt = convertPadInt(padIntServer);
                    accessedPadInts.Add(padInt.getUID(), padInt);
                }
                catch (Exception) { return null; }
            }

            return padInt;
        }

        private static PadInt convertPadInt(PadIntServer padIntServer){
            return new PadInt(padIntServer.GetUID(), padIntServer.GetValue());
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
