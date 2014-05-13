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
using CustomExceptions;

namespace DataServer
{

   public class IDataServerImp : MarshalByRefObject, IDataServer
    {
        // COnstants
        private readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";
        public static readonly string PRIMARY_SERVER = "Primary";
        public static readonly string BACKUP_SERVER = "Backup";
        public static readonly int MILLI = 1000;
        // Data Structres
        private Dictionary<int, PadIntServer> padIntDB;
        private enum State { Failed, Freezed, Functional }
        private object waiting = new object();
        // Variables
        private int WorkingThreads;
        private int DataServerState;
        private int dataServerID;
        private string url;
        // Replication Variables
        private string role;
        private string replicaAddress;
        private bool primaryIsAlive;
        // Timer Variables
        private int period;

        /// <summary>
        /// We need this?
        /// </summary>
        private int delay;

        private Timer PrimaryTimerReference;
        private Timer BackupTimerReference;
        private bool PrimaryTimerCanceled;
        private bool BackupTimerCanceled;

        public IDataServerImp()
        {
            this.padIntDB = new Dictionary<int, PadIntServer>();
            this.DataServerState = (int)State.Functional;
            this.WorkingThreads = 0;
        }

        public IDataServerImp(int dataServerID, string role, string url)
        {
            this.padIntDB = new Dictionary<int, PadIntServer>();
            this.DataServerState = (int)State.Functional;
            this.WorkingThreads = 0;
            this.dataServerID = dataServerID;
            this.role = role;
            this.url = url;
            this.period = 5;
            this.delay = 3;
        }

        public string ReplicaAddress 
        {
            get { return replicaAddress; }
            set { replicaAddress = value; } 

        }

        private void ReplacePadIntServer(PadIntServer padIntServer)
        {
            this.padIntDB[padIntServer.GetUID()].SetValue(padIntServer.GetValue());
        }

        private bool hasPadInt(int uid)
        {
            return this.padIntDB.ContainsKey(uid);
        }



        /**
         * Method that allows a user to create a new PadInt
         * object in the DataServer;
         **/

        public void CreateObject(int uid)
        {
            lock (this.waiting)
            {

                if (this.DataServerState == (int)State.Freezed)
                {
                    this.WorkingThreads++;
                    Monitor.Wait(this.waiting);
                }
                if (!PadIntExists(uid))
                {
                    PadIntServer padIntObject = new PadIntServer(uid);
                    this.padIntDB.Add(uid, padIntObject);
                    NotifyMasterServer(this.url, uid);
                    Console.WriteLine("Object with id " + uid + " created with sucess");
                }
                else
                {
                    throw new TxException("TxException: The object with id " + uid + " already exists.");
                }
            }
        }
        /**
         * Method that return an reference to the PadIntServer object with identifier uid. 
         **/
        public PadIntServer AccessObject(int uid)
        {
            lock (this.waiting)
            {
                if (this.DataServerState == (int)State.Freezed)
                {
                    this.WorkingThreads++;
                    Monitor.Wait(this.waiting);
                }

                if (this.padIntDB.ContainsKey(uid))
                {
                    return this.padIntDB[uid];
                }
                throw new TxException("TxException: The object with id " + uid + " does not exists.");
            }
        }

        /**
         * Method that verifies if the a PadInt object with identifier 
         * uid can be created at the DataServer.
         **/
        public bool PadIntExists(int uid)
        {
            bool answerRequest = false;
            IMasterServer remoteObject = getMasterRemoteInstance();
            answerRequest = remoteObject.ObjectExists(uid);
            return answerRequest;
        }

        /**
         * Method that notify the MasterServer when a PadInt object is created
         * sucessfully.
         **/
        public void NotifyMasterServer(string url, int uid)
        {
            IMasterServer remoteObject = getMasterRemoteInstance();
           
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
            lock (this.waiting)
            {
                if (this.DataServerState == (int)State.Freezed)
                {
                    this.WorkingThreads++;
                    Monitor.Wait(this.waiting);
                }
                Console.WriteLine("DataServer URL : " + this.url);
                Console.WriteLine("Data Server ID : " + this.dataServerID);
                Console.WriteLine("Stored PadInt's in this Server :");
                foreach (int id in padIntDB.Keys)
                {
                    Console.WriteLine("ID : " + id);
                }
                return true;
            }
        }

        public IMasterServer getMasterRemoteInstance()
        {
            IMasterServer remoteInstance = (IMasterServer)Activator.GetObject(
                typeof(IDataServer), MASTER_SERVER_ADDRESS);

            return remoteInstance;
        }
        
        public IDataServer getReplicaRemoteInstance(string replicaAddress)
        {
            IDataServer remoteInstance = (IDataServer)Activator.GetObject(
                typeof(IDataServer), replicaAddress);

            return remoteInstance;
        }

        /**
         * Method that makes the Data Server disconnect from the current channel and
         * after that register itself in a secret channel to accept recover calls from
         * the PADI-DSTM lib. This method returns the port from the secret channel.
         **/
        public bool Disconnect()
        {
            lock (this.waiting)
            {
                if (this.DataServerState == (int)State.Freezed)
                {
                    this.WorkingThreads++;
                    Monitor.Wait(this.waiting);
                }

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
        }

        /**
         * Method that change the Data Server actual state to Freezed.
         * To stop responding call but it maintains all calls for later
         * reply, a Monitor is used to change the active Threads state.  
         **/
        public bool FreezeDataServer()
        {
            lock(this.waiting)
            {

                if (this.DataServerState == (int)State.Functional)
                {
                    this.DataServerState = (int)State.Freezed;
                    Monitor.Enter(this.waiting);
                    return true;
                }
                else { return false; }
            }
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
                    int ActiveThreads = this.WorkingThreads;
                    this.DataServerState = (int)State.Functional;
                    for (int i = 0; i < ActiveThreads; i++)
                    {
                        Thread.Sleep(1000);
                        lock (this.waiting)
                        {
                            this.WorkingThreads--;
                            Monitor.Pulse(this.waiting);
                        }
                    }
                  answer = true;
                }
                else { answer = false; }
            }
            catch (SynchronizationLockException e) { Console.WriteLine(e.Message); }

            return answer;
        }

        /**
         * Transactions  
         **/

        public bool CanCommit(Transaction trans)
        {
            bool valid = true;

            //for(int Ti = startTn+1; Ti <= finishTn; Ti++)
            // if(read set of Tv intersects write set of Ti) valid = false

            return valid;
        }

        public void DoCommit(TranscationPadInt transPadInt)
        {
            //do changes
            this.padIntDB[transPadInt._UID].SetValue(transPadInt._value);
        }

        public void DoAbort(Transaction trans)
        {
            throw new NotImplementedException();
        }

        /**
         *   --------------   Replication ----------------
         **/

       /**
        * Populate the replica when it is created 
        **/
        public void PopulateReplica(Dictionary<int, PadIntServer> primaryDataBase)
        {
            foreach (KeyValuePair<int, PadIntServer> key in primaryDataBase)
            {
                this.padIntDB.Add(key.Key, key.Value);
            }
        }

       /**
        * Update the PadInt after a write operation is performed
        **/
        public void updatePadInt(int id, int value)
        {
            this.padIntDB[id].SetValue(value);
        }

        public void createReplicaServer(int serverID, string port)
        {
            try
            {
                IMasterServer remoteMaster = getMasterRemoteInstance();
                Console.WriteLine("Creating the replica ...");
                string replicaAddr = remoteMaster.CreateDataServerReplica(serverID, Convert.ToInt32(port));
                Console.WriteLine("Replica address " + replicaAddr);
                this.ReplicaAddress = replicaAddr;
            }
            catch (Exception e)
            {
                Console.WriteLine("DataServer Main Exception: " + e.Message);
                Console.WriteLine("<enter> to exit...");
                Console.ReadLine();
                return;
            }
        }

         /**
         * Method that is called by the primary server and
         * updates the state of the primary server at the replica.
         **/ 
        public void updatePrimaryState(bool primaryIsAlive)
        {
            this.primaryIsAlive = primaryIsAlive;
            Console.WriteLine("Updating primary state ...");
        }

        /**
         * Method that runs the primary server TimerTask
         **/ 
        public void RunTimerPrimary()
        {

            TimerCallback TimerDelegate = new TimerCallback(PrimaryTimerTask);
            System.Threading.Timer PrimaryTimerItem = new System.Threading.Timer(TimerDelegate, this, this.period * MILLI, this.period * MILLI);
            this.PrimaryTimerReference = PrimaryTimerItem;

            while (this.role == PRIMARY_SERVER)
            {
                System.Threading.Thread.Sleep(this.period * MILLI);
            }
            this.PrimaryTimerCanceled = true;

        }

        /**
        * Method that runs the backup server TimerTask
        **/ 
        public void RunTimerBackup()
        {
            TimerCallback TimerDelegate = new TimerCallback(BackupTimerTask);
            System.Threading.Timer BackupTimerItem = new System.Threading.Timer(TimerDelegate, this, this.period * MILLI, this.period * MILLI);
            this.BackupTimerReference = BackupTimerItem;

            while (this.role == BACKUP_SERVER)
            {
                System.Threading.Thread.Sleep( (this.period) * MILLI);
            }
            this.BackupTimerCanceled = true;
        }


        /**
         * Primary server task : this task is responsible to update 
         * the server state (primaryIsAlive) at the replica server.
         **/ 
        public void PrimaryTimerTask(object StateObj)
        {

            if (this.replicaAddress != null)
            {
                IDataServer remoteReplica = getReplicaRemoteInstance(this.replicaAddress);
                Console.WriteLine("Sending I'm alives to replica ...");
                remoteReplica.updatePrimaryState(this.primaryIsAlive);
                Console.WriteLine("Sleeping for " + this.period * MILLI + " seconds");
            }
            else
            {
                Console.WriteLine("Replica not yet assigned ...");
            }

            if (this.PrimaryTimerCanceled)
            {
                this.PrimaryTimerReference.Dispose();
                Console.WriteLine("Primary Timer Disposed ...");
            }
        }

        /**
         * Backup server task : this task is responsible to verify
         * what is the state of the primary server. If the primary
         * is dead, the backup server assumes the PRIMARY role.
         **/
        public void BackupTimerTask(object StateObject)
        {
            if (!this.primaryIsAlive)
            {
                Console.WriteLine("Primary is dead ...");
                // Needs to assume the role of primary server
                if (this.replicaAddress == null)
                {
                    this.role = PRIMARY_SERVER;
                    this.BackupTimerReference.Dispose();
                    IMasterServer remoteMaster = getMasterRemoteInstance();
                    remoteMaster.notifyMasterAboutFailure(this.dataServerID, this.url);
                    this.primaryIsAlive = true;
                    createReplicaServer(this.dataServerID, DataServer.port);
                    RunTimerPrimary();
                }
            }
            else
            {
                Console.WriteLine("Primary is alive ...");
                this.primaryIsAlive = false;
            }

            if (this.BackupTimerCanceled)
            {
                this.BackupTimerReference.Dispose();
                Console.WriteLine("Backup Timer Disposed ...");
            }
        }

    }
}
