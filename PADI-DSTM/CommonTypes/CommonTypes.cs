using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{

    [Serializable()]
    public class TxException : System.Exception
    {
        public TxException() : base() { }
        public TxException(string message) : base(message) { }
        public TxException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected TxException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }

    /**
   * Class that represents a PadInt object that is shared between the distributed system.
   **/
    [Serializable()]
    public class PadInt : MarshalByRefObject
    {
        private int uid;
        private int value;
        private bool read;
        private bool write;


        public PadInt(int uid)
        {
            this.uid = uid;
            this.value = 0;
            this.read = false;
            this.write = false;
        }

        public PadInt(int uid, int value)
        {
            this.uid = uid;
            this.value = value;
            this.read = false;
            this.write = false;
        }

        public bool wasRead()
        {
            return read;
        }

        public bool wasWrite()
        {
            return write;
        }

        public int getUID()
        {
            return this.uid;
        }

        /**
         *  Method that reads the object PadInt, and return the value of the object.
         *  @throws TxException
         **/
        public int Read()
        {
            this.read = true;
            return this.value;
        }

        /**
         * Method that writes a value in a PadInt object.
         * @throws TxException
         **/
        public void Write(int value)
        {
            this.write = true;
            //local changes
            this.value = value;
        }

        public override string ToString()
        {
            return "uid: " + this.uid + " val: " + this.value;
        }
    }

    [Serializable()]
    public class PadIntServer
    {
        private int uid;
        private int value;

        public PadIntServer(int uid)
        {
            this.uid = uid;
            this.value = 0;
        }

        public int GetUID()
        {
            return uid;
        }
        
        public int GetValue()
        {
            return this.value;
        }

        public void SetValue(int value)
        {
            //local changes
            this.value = value;
        }

        public override string ToString()
        {
            return "uid: " + this.uid + " val: " + this.value;
        }
    }

    /**
     * Commom Interfaces used by the data servers.
     **/
    public interface IDataServer
    {
        void CreateObject(int uid);
        PadIntServer AccessObject(int uid);
        bool Disconnect();
        bool DumpState();
        bool FreezeDataServer();
        bool RecoverDataServer();

        //Data Servers will implement participant's transaction interface
        //Call from coordinator to participant to ask whether it can commit a transaction
        //Participant replies with its vote
        bool CanCommit(Transaction trans);

        //Call from coordinator to participant to tell participant to commit its part of a transaction
        void DoCommit(TranscationPadInt padint);

        //Call from coordinator to participant to tell participant to abort its part of a transaction
        void DoAbort(Transaction trans);

        // Replication Methods
        void updatePrimaryState(bool primaryIsAlive);
    }

    /**
     * Common Interfaces used by the master server
     **/
    public interface IMasterServer
    {
        bool ObjectExists(int uid);
        bool ObjCreatedSuccess(string url, int uid);
        string GetDataServerAddress();
        string GetPadIntLocation(int uid);
        int RegisterDataServer(string url);
        bool ShowDataServersState();
        string CreateDataServerReplica(int dataServerID, int port);
        void notifyMasterAboutFailure(int id, string address);
        //Master will implement transaction interface

        //Start a new transaction and delivers a unique TID trans
        TID OpenTransaction();

        //Ends a transaction: (true) a commit return value indicates that the transaction has committed
        //(false) an abort return value indicates that it has aborted
        bool CloseTransaction(TID tid);

        //Aborts the transaction
        void AbortTransaction(TID tid);

        //Informs a coordinator that a new participant has joined the transaction trans
        void Join(TID trans, int participant);

        //Call from participant to coordinator to confirm that it has committed the transaction
        void HaveCommitted(TID tid, int participant);

        //Call from participant to coordinator to ask for the decision on a transaction
        //when it has voted Yes but has still had no reply after some delay
        //Used to recover from server crash or delayed messages

        bool GetDecision(TID tid);

        void AddToTransaction(int UID, bool wasWrite, int value, TID currentTx);
    }

    //This is a encapsulation of the Transaction Identifier
    [Serializable()]
    public class TID : IEquatable<TID>
    {
        private int id;
        /**
         * Operations made by client 
         */

        public TID(int id) { 
            this.id = id;
        }

        public int GetID() { return this.id; }

        public override string ToString()
        {
            return "TID value: " + this.id.ToString();
        }
        //Comparator for lists
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }
            // If parameter cannot be cast to TID return false.
            TID tid = obj as TID;
            if ((System.Object)tid == null)
            {
                return false;
            }
            // Return true if the fields match:
            return tid.GetID() == this.id;
        }

        public bool Equals(TID other)
        {
            if (other == null) return false;
            return (this.id.Equals(other.GetID()));
        }

        public override int GetHashCode()
        {
            return this.id;
        }
    }

    [Serializable()]
    public struct TranscationPadInt
    {
        public int _UID;
        public bool _wasWrite;
        public int _value;

        public TranscationPadInt(int UID, bool wasWrite, int value)
        {
            _UID = UID;
            _wasWrite = wasWrite;
            _value = value;
        }
    }

    //It is a representation of the Transaction
    //TID must be unique
    [Serializable()]
    public class Transaction
    {
        private TID tid;
        private List<int> partcipants;
        private Dictionary<int, TranscationPadInt> operations;

        public Transaction(TID tid)
        {
            this.tid = tid;
            partcipants = new List<int>();
            operations = new Dictionary<int, TranscationPadInt>();
        }

        public void addOperations(TranscationPadInt operation)
        {
            this.operations.Add(operation._UID, operation);
        }

        public TID GetTID() { return this.tid; }

        public void AddParticipant(int participant)
        {
            this.partcipants.Add(participant);
        }

        public List<int> GetParticipants() { return this.partcipants; }

        public override string ToString()
        {
            return "Trans with " + tid.ToString();
        }

        public List<TranscationPadInt> GetWritePadInts()
        {
            List<TranscationPadInt> writeList = new List<TranscationPadInt>();

            foreach (TranscationPadInt transPadInt in this.operations.Values)
            {
                if (transPadInt._wasWrite)
                {
                    writeList.Add(transPadInt);
                }
            }

            return writeList;
        }
    }

}
