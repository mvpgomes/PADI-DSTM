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

    class TransactionSystem
    {
        //At certain point a DataServer 
        //may join on several transactions
        private List<TID> joined;
        private int participant;

        public TransactionSystem(int participant) 
        {
            this.joined = new List<TID>();
            this.participant = participant;
        }

        public void JoinTransaction(TID tid, IMasterServer master)
        {
            if(!HasJoined(tid)) {
                //join in master
                master.Join(tid, participant);
                this.joined.Add(tid);
            }
        }

        public bool HasJoined(TID tid) 
        { 
            return this.joined.Contains(tid); 
        }
        //test
        public void WriteValue(TID tid, PadInt padint, IMasterServer master)
        {
            master.LogWrite(tid, padint);
        }

        public void CloseTransaction(TID tid)
        {
            this.joined.Remove(tid);
        }

    }

    public class IDataServerImp : MarshalByRefObject, IDataServer
    {
        // COnstants
        private readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";
        public static readonly string PRIMARY_SERVER = "Primary";
        public static readonly string BACKUP_SERVER = "Backup";
        public static readonly int MILLI = 1000;
        // Data Structres
        private Dictionary<int, PadInt> padIntDB;
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
        private int delay;
        private Timer PrimaryTimerReference;
        private Timer BackupTimerReference;
        private bool PrimaryTimerCanceled;
        private bool BackupTimerCanceled;

        //Transaction
        private TransactionSystem transactionSys;

        public IDataServerImp()
        {
            this.padIntDB = new Dictionary<int, PadInt>();
            this.DataServerState = (int)State.Functional;
            this.WorkingThreads = 0;
        }

        public IDataServerImp(int dataServerID, string role, string url)
        {
            this.padIntDB = new Dictionary<int, PadInt>();
            this.DataServerState = (int)State.Functional;
            this.WorkingThreads = 0;
            this.dataServerID = dataServerID;
            this.role = role;
            this.url = url;
            this.transactionSys = new TransactionSystem(dataServerID);
            this.period = 5;
            this.delay = 3;
        }

        public string ReplicaAddress 
        {
            get { return replicaAddress; }
            set { replicaAddress = value; } 

        }

        private void ReplacePadInt(PadInt padInt)
        {
            this.padIntDB[padInt.uid].Write(padInt.value);
        }

        private bool hasPadInt(int uid)
        {
            return this.padIntDB.ContainsKey(uid);
        }



        /**
         * Method that allows a user to create a new PadInt
         * object in the DataServer;
         **/
        public PadInt CreateObject(int uid, TID tid)
        {
            lock (this.waiting)
            {

                if (this.DataServerState == (int)State.Freezed)
                {
                    this.WorkingThreads++;
                    Monitor.Wait(this.waiting);
                }
                if (!PermissionToCreate(uid))
                {
                    PadInt padIntObject = new PadInt(uid, this.url, tid);
                    this.padIntDB.Add(uid, padIntObject);
                    NotifyMasterServer(this.url, uid);
                    //join in the transaciton
                    this.transactionSys.JoinTransaction(tid, this.getMasterRemoteInstance());
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
            lock (this.waiting)
            {
                if (this.DataServerState == (int)State.Freezed)
                {
                    this.WorkingThreads++;
                    Monitor.Wait(this.waiting);
                }
                
                PadInt padint = this.padIntDB[uid];
                
                if (padint != null)
                {
                    this.transactionSys.JoinTransaction(padint.GetTID(), this.getMasterRemoteInstance());
                }
                
                return padint;
            }
        }

        /**
         * Method that verifies if the a PadInt object with identifier 
         * uid can be created at the DataServer.
         **/
        public bool PermissionToCreate(int uid)
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

        public void WriteLog(PadInt padint)
        {
            this.transactionSys.WriteValue(padint.GetTID(), padint, getMasterRemoteInstance());
        }

        public bool CanCommit(Transaction trans)
        {
            bool valid = true;

            //for(int Ti = startTn+1; Ti <= finishTn; Ti++)
            // if(read set of Tv intersects write set of Ti) valid = false

            return valid;
        }

        public void DoCommit(Transaction trans)
        {
            //do changes
            foreach (PadInt padInt in trans.GetWriteSet())
            {
                if (this.hasPadInt(padInt.uid))
                {
                    this.ReplacePadInt(padInt);
                }
            }
        }

        public void DoAbort(Transaction trans)
        {
            throw new NotImplementedException();
        }

        /**
         *  Replication
         **/

        public void updatePrimaryState(bool primaryIsAlive)
        {
            this.primaryIsAlive = primaryIsAlive;
        }

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

        public void RunTimerBackup()
        {
            TimerCallback TimerDelegate = new TimerCallback(BackupTimerTask);
            System.Threading.Timer BackupTimerItem = new System.Threading.Timer(TimerDelegate, this, this.period * MILLI, this.period * MILLI);
            this.BackupTimerReference = BackupTimerItem;

            while (this.role == BACKUP_SERVER)
            {
                System.Threading.Thread.Sleep( (this.period + this.delay) * MILLI);
            }
            this.BackupTimerCanceled = true;
        }

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

        public void BackupTimerTask(object StateObject)
        {
            if (!this.primaryIsAlive)
            {
                Console.WriteLine("Primary is dead ...");
                // Needs to assume the role of primary server
                if (this.replicaAddress != null)
                {
                    this.role = PRIMARY_SERVER;
                    this.BackupTimerReference.Dispose();
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
