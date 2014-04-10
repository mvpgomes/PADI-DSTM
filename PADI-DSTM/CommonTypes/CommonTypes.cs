using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    /**
    * Class that represents a PadInt object that is shared between the distributed system.
    **/
    public class PadInt
    {
        private int uid;
        private int padIntValue;

        public PadInt(int uid)
        {
            this.uid = uid;
        }
        
        /**
         *  Method that reads the object PadInt, and return the value of the object.
         *  @throws TxException
         **/
        public int Read()
        {
            return this.padIntValue;
        }

        /**
         * Method that writes a value in a PadInt object.
         * @throws TxException
         **/
        public void Write(int value)
        {
            this.padIntValue = value;
        }
    }

    /**
     * Commom Interfaces used by the data servers.
     **/
    public interface IDataServer
    {
        PadInt createObject(int uid);
        PadInt accessObject(int uid);
        //Method to perform Serializable function
        void serializeMyObjects();
        //Get Data Server saved objects
        void getSavedObjects();
        //
        Dictionary<int, PadInt> returnPadIntDB();

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
        string getDataServerAddress();
        string getPadIntLocation(int uid);
        int RegisterDataServer(string url);

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
        
        public Transaction(TID tid) { this.tid = tid; }
        
        public TID GetTID() { return this.tid; }

        public override string ToString()
        {
            return "Trans with " + tid.ToString();
        }
    }
}
