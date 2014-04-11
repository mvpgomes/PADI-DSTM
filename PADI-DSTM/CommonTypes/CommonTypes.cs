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
    public class PadInt
    {
        private int uid;
        private int value;

        public PadInt(int uid)
        {
            this.uid = uid;
            this.value = 0;
        }

        /**
         *  Method that reads the object PadInt, and return the value of the object.
         *  @throws TxException
         **/
        public int Read()
        {
            return this.value;
        }

        /**
         * Method that writes a value in a PadInt object.
         * @throws TxException
         **/
        public void Write(int value)
        {
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
        PadInt CreateObject(int uid);
        PadInt AccessObject(int uid);
        bool Disconnect();
        bool DumpState();
        bool FreezeDataServer();
        bool RecoverDataServer();

        //Data Servers will implement participant's transaction interface
        //Call from coordinator to participant to ask whether it can commit a transaction
        //Participant replies with its vote
        bool CanCommit(Transaction trans);

        //Call from coordinator to participant to tell participant to commit its part of a transaction
        void DoCommit(Transaction trans);

        //Call from coordinator to participant to tell participant to abort its part of a transaction
        void DoAbort(Transaction trans);
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

        //Master will implement transaction interface

        //Start a new transaction and delivers a unique TID trans
        Transaction OpenTransaction();

        //Ends a transaction: (true) a commit return value indicates that the transaction has committed
        //(false) an abort return value indicates that it has aborted
        bool CloseTransaction(Transaction trans);

        //Aborts the transaction
        void AbortTransaction(Transaction trans);

        //Informs a coordinator that a new participant has joined the transaction trans
        void Join(Transaction trans, int participant);

        //Call from participant to coordinator to confirm that it has committed the transaction
        void HaveCommitted(Transaction trans, int participant);

        //Call from participant to coordinator to ask for the decision on a transaction
        //when it has voted Yes but has still had no reply after some delay
        //Used to recover from server crash or delayed messages
        bool GetDecision(Transaction trans);

    }


    //This is a encapsulation of the Transaction Identifier
    public class TID
    {
        private int id;

        public TID(int id) { this.id = id; }

        public int GetID() { return this.id; }

        public override string ToString()
        {
            return "TID value: " + this.id.ToString();
        }
    }

    //It is a representation of the Transaction
    //TID must be unique
    public class Transaction
    {
        private TID tid;
        private List<int> partcipants;

        public Transaction(TID tid)
        {
            this.tid = tid;
            partcipants = new List<int>();
        }

        public TID GetTID() { return this.tid; }

        public void AddParticipant(int participant)
        {
            this.partcipants.Add(participant);
        }

        public override string ToString()
        {
            return "Trans with " + tid.ToString();
        }
    }

}
