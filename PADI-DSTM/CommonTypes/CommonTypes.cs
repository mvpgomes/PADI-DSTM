using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    /// <summary>
    /// TxException - Exception speficication.
    /// </summary>
    [Serializable()]
    public class TxException : System.Exception
    {
        /**
         *  TxException constructor.
         */
        public TxException() : base() {}
        /**
         *  TxException constructor.
         *  @param message - string message.
         */
        public TxException(string message) : base(message) {}
        /**
         *  TxException constructor.
         *  @param message - string message.
         *  @param inner - System.Exception inner.
         */
        public TxException(string message, System.Exception inner) : base(message, inner) {}
        /**
         *   TxException - constructor.
         *   A constructor is needed for serialization when an 
         *   exception propagates from a remoting server to the client.
         *   @param info - System.Runtime.Serialization.SerializationInfo info.
         *   @param context - System.Runtime.Serialization.StreamingContext context.
         */
        protected TxException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context){}
    }

    /// <summary>
    /// PadInt - Class that represents a PadInt object that is shared between the distributed system.
    /// 
    /// </summary>
    [Serializable()]
    public class PadInt : MarshalByRefObject
    {
        /**
         *  PadInt variables.
         */
        private int uid;
        private int value;
        private bool read;
        private bool write;
        /**
         *  PadInt constructor.
         *  @param uid - int that represents PadInt UID.
         */
        public PadInt(int uid)
        {
            this.uid = uid;
            this.value = 0;
            this.read = false;
            this.write = false;
        }
        /**
         *  PadInt constructor.
         *  @param uid - int that represents PadInt UID.
         *  ~@param value - int that represents PadInt value.
         */
        public PadInt(int uid, int value)
        {
            this.uid = uid;
            this.value = value;
            this.read = false;
            this.write = false;
        }
        /**
         *  PadInt - wasRead - Method to know if the PadInt was read.
         *  @return bool.
         */
        public bool wasRead()
        {
            return read;
        }
        /**
         *  PadInt - wasWrite - Method to know if the PadInt was wrote.
         *  @return bool.
         */
        public bool wasWrite()
        {
            return write;
        }
        /**
         *  PadInt - getUID - Method to get the UID from the padInt.
         *  @return an int that represents de PadInt UID.
         */
        public int getUID()
        {
            return this.uid;
        }
        /**
         *  PadInt - Read - Method that reads the object PadInt, and return the value of the object.
         *  @throws TxException
         *  @return int that represents PadInt value.
         */
        public int Read()
        {
            this.read = true;
            return this.value;
        }
        /**
         *  PadInt - Write - Method that writes a value in a PadInt object.
         *  @param value - write value into PadInt
         *  @throws TxException
         */
        public void Write(int value)
        {
            this.write = true;
            this.value = value;
        }
        /**
         *  PadInt - ToString - ToString the padInt content.
         *  @return string 
         */
        public override string ToString()
        {
            return "uid: " + this.uid + " val: " + this.value;
        }
    }

    /// <summary>
    /// PadIntServer - PadIntServer specification.
    /// This object represents a PadInt on the data server side.
    /// </summary>
    [Serializable()]
    public class PadIntServer
    {
        /**
         *  PadIntServer variables
         */
        private int uid;
        private int value;
        /**
         *  PadInt constructor
         *  @param uid - int that represents PadIntServer uid.
         */
        public PadIntServer(int uid)
        {
            this.uid = uid;
            this.value = 0;
        }
        /**
         *  PadIntServer - GetUID - Method that returns the PadIntServer UID.
         *  Method returns an int.
         */
        public int GetUID()
        {
            return uid;
        }
        /**
         *  PadIntServer - GetValue - Method that returns the PadIntServer value.
         *  @return an int - the PadIntServer value.
         */
        public int GetValue()
        {
            return this.value;
        }
        /**
         *  PadIntServer - SetValue - Method sets the value of value variable.
         *  @param value - int - the new PadIntServer value.
         */
        public void SetValue(int value)
        {
            this.value = value;
        }
        /**
         *  PadIntServer - ToString - ToString PadIntServer content.
         *  @return a string.
         */
        public override string ToString()
        {
            return "uid: " + this.uid + " val: " + this.value;
        }
    }

    /// <summary>
    /// IDataServer - Commom Interface used by the data servers.
    /// </summary>
    public interface IDataServer
    {
        /**
         *  IDataServer - CreateObject.
         *  @param uid - int object uid.
         */
        void CreateObject(int uid);
        /**
         *  IDataServer - AccessObject.
         *  @param uid - int object uid.
         *  @return PadIntServer reference.
         */ 
        PadIntServer AccessObject(int uid);
        /**
         *  IDataServer - Disconnect.
         *  @return bool - confirmation.
         */
        bool Disconnect();
        /**
         *  IDataServer - DumpState.
         *  @return bool - confirmation.
         */
        bool DumpState();
        /**
         *  IDataServer - FreezeDataServer.
         *  @return bool - confirmation.
         */
        bool FreezeDataServer();
        /**
         *  IDataServer - RecoverDataServer.
         *  @return bool - confirmation.
         */
        bool RecoverDataServer();
        /**
         *  IDataServer - CanCommit
         *  Data Servers will implement participant's transaction interface
         *  Call from coordinator to participant to ask whether it can commit a transaction
         *  Participant replies with its vote.
         *  @param trans - Transaction.
         *  @return bool - confirmation.
         */
        bool CanCommit(Transaction trans);

        /**
         *  IDataServer - DoAbort
         *  Call from coordinator to participant to tell participant to commit its part of a transaction.
         *  @param padint - TransactionPadInt
         */
        void DoCommit(TranscationPadInt padint);

        /**
         *  IDataServer - DoCommit
         *  Call from coordinator to participant to tell participant to abort its part of a transaction.
         *  @param trans
         */
        void DoAbort(Transaction trans);

        /**
         *  IDataServer - updatePrimaryState
         *  Replication Methods.
         *  @param primaryIsAlive - boolean
         */
        void updatePrimaryState(bool primaryIsAlive);
    }

    /// <summary>
    /// IMasterServer - Common Interfaces used by the master server.
    /// 
    /// </summary>
    public interface IMasterServer
    {
        /**
         *  IMasterServer - ObjectExists
         *  @param uid - int uid.
         *  @return bool - object exists?.
         */
        bool ObjectExists(int uid);
        /**
         *  IMasterServer - ObjCreatedSuccess
         *  @param url - string url.
         *  @param uid - int uid.
         *  @return bool - object created with success?.
         */
        bool ObjCreatedSuccess(string url, int uid);
        /**
         *  IMasterServer - ShowDataServersState
         *  @return bool - .
         */
        bool ShowDataServersState();
        /**
         *  IMasterServer - GetDataServerAddress
         *  @return string - Data Server Address.
         */
        string GetDataServerAddress();
        /**
         *  IMasterServer - GetPadIntLocation
         *  @param uid - int.
         *  @return string - PadInt Location.
         */
        string GetPadIntLocation(int uid);
        /**
         *  IMasterServer - CreateDataServerReplica
         *  @param dataServerID - data server id.
         *  @param port - replica port.
         *  @return string - Replica location.
         */
        string CreateDataServerReplica(int dataServerID, int port);
        /**
         *  IMasterServer - RegisterDataServer
         *  @param url - string.
         *  @return int - .
         */
        int RegisterDataServer(string url);
        /**
         *  IMasterServer - notifyMasterAboutFailure
         *  Notify Master about the failure.
         *  @param id - int.
         *  @param address - string.
         */
        void notifyMasterAboutFailure(int id, string address);
        /**
         *  IMasterServer - OpenTransaction
         *  Start a new transaction and delivers a unique TID trans.
         *  @return TID
         */
        TID OpenTransaction();
        /**
         *  IMasterServer - CloseTransaction
         *  Ends a transaction: (true) a commit return value indicates that the transaction has committed
         *  (false) an abort return value indicates that it has aborted
         *  @param tid.
         *  @return bool.
         */
        bool CloseTransaction(TID tid);
        /**
         *  IMasterServer - AbortTransaction
         *  Aborts the transaction.
         *  @param tid - TID object.
         */
        void AbortTransaction(TID tid);
        /**
         *  IMasterServer - Join
         *  Informs a coordinator that a new participant has joined the transaction trans.
         *  @param trans - TID.
         *  @param participant - participation identification.
         */
        void Join(TID trans, int participant);
        /**
         *  IMasterServer - HaveCommitted
         *  Call from participant to coordinator to confirm that it has committed the transaction.
         *  @param tid - TID.
         *  @param participant - participation identification.
         */
        void HaveCommitted(TID tid, int participant);
        /**
         *  IMasterServer - GetDecision
         *  Call from participant to coordinator to ask for the decision on a transaction
         *  when it has voted Yes but has still had no reply after some delay.
         *  Used to recover from server crash or delayed messages.
         *  @param tid - TID.
         *  @return bool.
         */
        bool GetDecision(TID tid);
        /**
         *  IMasterServer - AddToTransaction
         *  Add to transaction.
         *  @param wasWrite - bool.
         *  @param value - int.
         *  @param currentTx - TID.
         */
        void AddToTransaction(int UID, bool wasWrite, int value, TID currentTx);
    }

    /// <summary>
    /// TID - This is a encapsulation of the Transaction Identifier.
    /// 
    /// </summary>
    [Serializable()]
    public class TID : IEquatable<TID>
    {
        /**
         * TID variables.
         */ 
        private int id;
        /**
         *  TID constructor
         *  @param id - int that represents transaction id.
         */
        public TID(int id) { 
            this.id = id;
        }
        /**
         *  TID - GetID
         *  @return transaction id.
         */
        public int GetID() { return this.id; }
        /**
         *  TID - ToString
         *  @return string - transaction content.
         */
        public override string ToString()
        {
            return "TID value: " + this.id.ToString();
        }
        /**
         *  TID - Equals
         *  Comparator for lists.
         *  @return bool - equals?.
         */
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            TID tid = obj as TID;
            if ((System.Object)tid == null)
            {
                return false;
            }
            return tid.GetID() == this.id;
        }
        /**
         *  TID - Equals
         *  Comparator for transactions identifier.
         *  @return bool - equals?.
         */
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
    /// <summary>
    /// TranscationPadInt - Used by master to save transaction operations.
    /// </summary>
    [Serializable()]
    public struct TranscationPadInt
    {
        /*
         * TranscationPadInt variables
         */
        public int _UID;
        public bool _wasWrite;
        public int _value;
        /*
         * TranscationPadInt constructor
         * @param UID - int.
         * @param wasWrite - bool.
         * @param value - int.
         */
        public TranscationPadInt(int UID, bool wasWrite, int value)
        {
            _UID = UID;
            _wasWrite = wasWrite;
            _value = value;
        }
    }

    /// <summary>
    /// Transaction - A representation of the Transaction.
    /// TID must be unique.
    /// </summary>
    [Serializable()]
    public class Transaction
    {
        /*
         * Transaction variables.
         */ 
        private TID tid;
        private List<int> partcipants;
        private Dictionary<int, TranscationPadInt> operations;
        /**
         * Transaction constructor
         * @param tid - TID.
         */ 
        public Transaction(TID tid)
        {
            this.tid = tid;
            partcipants = new List<int>();
            operations = new Dictionary<int, TranscationPadInt>();
        }
        /**
         * Transaction - addOperations
         * @param operation - TranscationPadInt.
         */
        public void addOperations(TranscationPadInt operation)
        {
            this.operations.Add(operation._UID, operation);
        }
        /**
         * Transaction - GetTID
         * @return TID - from this transaction.
         */
        public TID GetTID() { return this.tid; }
        /**
         * Transaction - AddParticipant
         * @param participant - int participant identifier.
         */
        public void AddParticipant(int participant)
        {
            this.partcipants.Add(participant);
        }
        /**
         * Transaction - GetParticipants
         * @return List<int> - List of participants.
         */
        public List<int> GetParticipants() { return this.partcipants; }
        /**
         * Transaction - ToString
         * @return string - Transaction content.
         */
        public override string ToString()
        {
            return "Trans with " + tid.ToString();
        }
        /**
         * Transaction - GetWritePadInts
         * @return List<TranscationPadInt> - List of TransactionPadInts representing operations made by some client.
         */
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
} //CommonTypes