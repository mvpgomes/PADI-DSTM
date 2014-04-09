using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using CommonTypes;

namespace DataServer
{
    class IDataServerImp : MarshalByRefObject, IDataServer
    {
        private int DataServerState;

        private readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";

        private Dictionary<int, PadInt> padIntDB;

        private enum State { Failed, Freezed, Functional }


        public IDataServerImp()
        {
            this.padIntDB = new Dictionary<int, PadInt>();
            this.DataServerState = (int)State.Functional;
        }

        /**
         * Method that allows a user to create a new PadInt
         * object in the DataServer;
         **/
        public PadInt CreateObject(int uid)
        {
            lock (this)
            {

                if (this.DataServerState == (int)State.Freezed)
                {
                    Monitor.Wait(this);
                    return null;
                }
                else if (!PermissionToCreate(uid))
                {
                    PadInt padIntObject = new PadInt(uid);
                    this.padIntDB.Add(uid, padIntObject);
                    NotifyMasterServer(DataServer.dataServerAddr, uid);
                    Console.WriteLine("Object with id " + uid + " created with sucess"); ;
                    return padIntObject;
                }
                else
                {
                    Console.WriteLine("ERROR: The object with id " + uid + " already exists.");
                    return null;
                }
            }
        }
        /**
         * Method that return an reference to the PadInt object with identifier uid. 
         **/
        public PadInt AccessObject(int uid)
        {
            return this.padIntDB[uid];
        }

        /**
         * Method that verifies if the a PadInt object with identifier 
         * uid can be created at the DataServer.
         **/
        public bool PermissionToCreate(int uid)
        {
            bool answerRequest = false;
            IMasterServer remoteObject;
            remoteObject = (IMasterServer)Activator.GetObject(
                typeof(IMasterServer),
                MASTER_SERVER_ADDRESS);
            answerRequest = remoteObject.ObjectExists(uid);
            return answerRequest;
        }

        /**
         * Method that notify the MasterServer when a PadInt object is created
         * sucessfully.
         **/
        public void NotifyMasterServer(string url, int uid)
        {
            IMasterServer remoteObject;
            remoteObject = (IMasterServer)Activator.GetObject(
                typeof(IMasterServer),
                MASTER_SERVER_ADDRESS);
            try
            {
                remoteObject.ObjCreatedSuccess(url, uid);
            }
            catch (Exception e)
            {
                Console.WriteLine("The remote call throw the exception : " + e);
            }
        }

        /**
         * Method that shows the state of the DataServer.
         * The DataServer state is described by its id and the id's of the PadInt's that 
         * are stored in. 
         **/
        public bool DumpState()
        {
            Console.WriteLine("Data Server ID : " + DataServer.dataServerID);
            Console.WriteLine("Stored PadInt's in this Server :");
            foreach (int id in padIntDB.Keys)
            {
                Console.WriteLine("ID : " + id);
            }
            return true;
        }

        /**
         * Method that makes the Data Server disconnect from the current channel and
         * after that register itself in a secret channel to accept recover calls from
         * the PADI-DSTM lib. This method returns the port from the secret channel.
         **/
        public bool Disconnect()
        {
            try
            {
                this.DataServerState = (int)State.Failed;
                ChannelServices.UnregisterChannel(DataServer.channel);
                return true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        /**
         * Method that change the Data Server actual state to Freezed.
         * To stop responding call but it maintains all calls for later
         * reply, a Monitor is used to change the active Threads state.  
         **/
        public bool FreezeDataServer()
        {
            if (this.DataServerState == (int)State.Functional)
            {
                this.DataServerState = (int)State.Freezed;
                return true;
            }
            else { return false; }
        }

        /**
         * Method that change the Data Server actual state to Functional.
         * This method also release all process that are in the Monitor
         * Wait room.
         **/
        public bool RecoverDataServer()
        {
            bool answer = false;
            try
            {
                if (this.DataServerState == (int)State.Freezed)
                {
                    this.DataServerState = (int)State.Functional;
                    Monitor.PulseAll(this);
                    answer = true;
                }
                else { answer = false; }
            }
            catch (SynchronizationLockException e) { Console.WriteLine(e.Message); }

            return answer;
        }
    }
}
