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

   public class IDataServerImp : MarshalByRefObject, IDataServer
    {
        /*
         * IDataServerImp Constants
         */
        private readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";
        public static readonly string PRIMARY_SERVER = "Primary";
        public static readonly string BACKUP_SERVER = "Backup";
        public static readonly int MILLI = 1000;
        /*
         * IDataServerImp Data Structres
         */
        private Dictionary<int, PadIntServer> padIntDB;
        private enum State { Failed, Freezed, Functional }
        private object waiting = new object();
        /*
         * IDataServerImp Variables
         */
        private int WorkingThreads;
        private int DataServerState;
        private int dataServerID;
        private string url;
        /*
         * IDataServerImp Replication Variables
         */
        private string role;
        private string replicaAddress;
        private bool primaryIsAlive;
        /*
         * IDataServerImp Timer Variables
         */
        private int period;
        private Timer PrimaryTimerReference;
        private Timer BackupTimerReference;
        private bool PrimaryTimerCanceled;
        private bool BackupTimerCanceled;

        /*
         * IDataServerImp Constructor
         * A new DataServer Instance is created.
         */     
        public IDataServerImp()
        {
            this.padIntDB = new Dictionary<int, PadIntServer>();
            this.DataServerState = (int)State.Functional;
            this.WorkingThreads = 0;
        }

        /*
         * IDataServerImp Constructor
         * A new DataServer Instance is created.
         * @param dataServerID - int - data server identifier.
         * @param role - string - role.
         * @param url - string - string.
         */
        public IDataServerImp(int dataServerID, string role, string url)
        {
            this.padIntDB = new Dictionary<int, PadIntServer>();
            this.DataServerState = (int)State.Functional;
            this.WorkingThreads = 0;
            this.dataServerID = dataServerID;
            this.role = role;
            this.url = url;
            this.period = 5;
        }

        /*
         * ReplicaAddress
         */
        public string ReplicaAddress 
        {
            get { return replicaAddress; }
            set { replicaAddress = value; } 
        }

        /*
         * IDataServerImp - ReplacePadIntServer.
         * Replace PadIntServer value with a new one.
         * @param padIntServer - PadIntServer.
         */
        private void ReplacePadIntServer(PadIntServer padIntServer)
        {
            this.padIntDB[padIntServer.GetUID()].SetValue(padIntServer.GetValue());
        }

        /*
         * IDataServerImp - NotifyMasterServer.
         * Method that notify the MasterServer when a PadInt object is created
         * sucessfully.
         * @param url - string
         * @param uid - int 
         */
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

        /*
         * IDataServerImp - hasPadInt
         * Checks if a padIntServer with identifier uid
         * already exists in this data server.
         * @param uid - id of padIntServer.
         */
        private bool hasPadInt(int uid)
        {
            return this.padIntDB.ContainsKey(uid);
        }

        /**
          *  IDataServer - CreateObject.
          * Method that allows a user to create a new PadInt
          * object in the DataServer.
          *  @param uid - int object uid.
          */
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
                    Console.WriteLine("Object with id " + uid + " created with sucess"); ;
                    // update the replica
                    IDataServer remoteReplica = getReplicaRemoteInstance(this.ReplicaAddress);
                    remoteReplica.UpdatePadInt(uid, 0);
                }
                else
                {
                    Console.WriteLine("ERROR: The object with id " + uid + " already exists.");
                }
            }
        }

        /**
         *  IDataServer - AccessObject.
         *  Method that return an reference to the PadIntServer object with identifier uid.
         *  @param uid - int object uid.
         *  @return PadIntServer reference.
         */
        public PadIntServer AccessObject(int uid)
        {
            lock (this.waiting)
            {
                if (this.DataServerState == (int)State.Freezed)
                {
                    this.WorkingThreads++;
                    Monitor.Wait(this.waiting);
                }
                return this.padIntDB[uid];
            }
        }
        
        /**
         *  IDataServer - PadIntExists.
         *  Method that verifies if the a PadInt object with identifier 
         *  uid can be created at the DataServer.
         *  @param uid - int object uid.
         *  @return bool - result.
         */
        public bool PadIntExists(int uid)
        {
            bool answerRequest = false;
            IMasterServer remoteObject = getMasterRemoteInstance();
            answerRequest = remoteObject.ObjectExists(uid);
            return answerRequest;
        }  

        /**
         *  IDataServer - Disconnect.
         * Method that makes the Data Server disconnect from the current channel and
         * after that register itself in a secret channel to accept recover calls from
         * the PADI-DSTM lib. This method returns the port from the secret channel.
         *  @return bool - result.
         */
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
         *  IDataServer - DumpState.
         * Method that shows the state of the DataServer.
         * The DataServer state is described by its id and the id's of the PadInt's that 
         * are stored in. 
         *  @return bool - result.
         */
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

        /**
         *  IDataServer - FreezeDataServer.
         * Method that change the Data Server actual state to Freezed.
         * To stop responding call but it maintains all calls for later
         * reply, a Monitor is used to change the active Threads state.  
         *  @return bool - result.
         */
        public bool FreezeDataServer()
        {
            lock (this.waiting)
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
         *  IDataServer - RecoverDataServer.
         * Method that change the Data Server actual state to Functional.
         * This method also release all process that are in the Monitor
         * Wait room.  
         *  @return bool - result.
         */
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
         * --------------   Transactions    ----------------
         **/

        public bool CanCommit(TID tid)
        {
            //record the tid and the result of this method
            if(this.DataServerState == (int)State.Failed ||
                this.DataServerState == (int)State.Freezed) {
                return false;
            } 
            return true;
        }

        /**
         *  IDataServer - DoCommit.
         * Commit transaction. 
         * @param transPadInt - TranscationPadInt.
         */
        public void DoCommit(TranscationPadInt transPadInt)
        {
            this.padIntDB[transPadInt._UID].SetValue(transPadInt._value);
            //Updates the replica
            IDataServer remoteReplica = getReplicaRemoteInstance(this.ReplicaAddress);
            remoteReplica.UpdatePadInt(transPadInt._UID, transPadInt._value);
        }

        /**
         *  IDataServer - DoAbort.
         * @param tid - TID.
         */
        public void DoAbort(TID tid)
        {
            //log tid aborted
        }
       
        /**
         *   --------------   Replication    ----------------
         **/
        /**
         *  IDataServer - DoAbort.
        * Populate the replica when it is created. 
         * @param primaryDataBase - Dictionary<int, PadIntServer>.
         */
        public void PopulateReplica(Dictionary<int, PadIntServer> primaryDataBase)
        {
            foreach (KeyValuePair<int, PadIntServer> key in primaryDataBase)
            {
                this.padIntDB.Add(key.Key, key.Value);
                Console.WriteLine("Update PadInt with uid : " 
                    + key.Value.GetUID() + " value : " + key.Value.GetValue());
            }
        }

        /**
         *  IDataServer - getMasterRemoteInstance.
         * 
         * @return IMasterServer.
         */
        public IMasterServer getMasterRemoteInstance()
        {
            IMasterServer remoteInstance = (IMasterServer)Activator.GetObject(
                typeof(IDataServer), MASTER_SERVER_ADDRESS);

            return remoteInstance;
        }

        /**
          *  IDataServer - getReplicaRemoteInstance.
          * @param replicaAddress - string.
          * @return IDataServer.
          */
        public IDataServer getReplicaRemoteInstance(string replicaAddress)
        {
            IDataServer remoteInstance = (IDataServer)Activator.GetObject(
                typeof(IDataServer), replicaAddress);

            return remoteInstance;
        }

        /**
          *  IDataServer - UpdatePadInt.
          *  Update the PadInt after a write operation is performed
          * @param id - int - PadInt id.
          * @param value - int - PadInt Value.
          */
        public void UpdatePadInt(int id, int value)
        {
            if (this.padIntDB.ContainsKey(id)) { 
                this.padIntDB[id].SetValue(value);
                Console.WriteLine("Update PadInt with uid : " + id + " value : " + value);
            }
            else
            {
                PadIntServer padInt = new PadIntServer(id);
                this.padIntDB.Add(id, padInt);
                Console.WriteLine("Created PadInt with uid : " + id);
            }
        }
  
        /**

         **/
        /**
           *  IDataServer - createReplicaServer.
           * Method that is called by the primary server to assign
           * a new replica server.
           * @param serverID - int.
           * @param port - string.
           */
        public void createReplicaServer(int serverID, string port)
        {
            try
            {
                IMasterServer remoteMaster = getMasterRemoteInstance();
                Console.WriteLine("Creating the replica ...");
                string replicaAddr = remoteMaster.CreateDataServerReplica(serverID, Convert.ToInt32(port));
                Console.WriteLine("Replica address " + replicaAddr);
                this.ReplicaAddress = replicaAddr;
                // Populate the replica server with the existents padInt on the database
                IDataServer remoteReplica = getReplicaRemoteInstance(this.ReplicaAddress);
                remoteReplica.PopulateReplica(this.padIntDB);
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
         */ 
        public void RunTimerPrimary()
        {
            TimerCallback TimerDelegate = new TimerCallback(PrimaryTimerTask);
            System.Threading.Timer PrimaryTimerItem = 
                new System.Threading.Timer(TimerDelegate, this, this.period * MILLI, this.period * MILLI);
            this.PrimaryTimerReference = PrimaryTimerItem;

            while (this.role == PRIMARY_SERVER)
            {
                System.Threading.Thread.Sleep(this.period * MILLI);
            }
            this.PrimaryTimerCanceled = true;
        }
    
        /**
        * Method that runs the backup server TimerTask
        */ 
        public void RunTimerBackup()
        {
            TimerCallback TimerDelegate = new TimerCallback(BackupTimerTask);
            System.Threading.Timer BackupTimerItem = 
                new System.Threading.Timer(TimerDelegate, this, this.period * MILLI, this.period * MILLI);
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
         */ 
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
                    remoteMaster.NotifyMasterAboutFailure(this.dataServerID, this.url);
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

        /**
         *   --------------   Replication    ----------------
         */
    }
}
