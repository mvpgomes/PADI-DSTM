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
    }

    //This is a encapsulation of the Transaction Identifier
    public class TID
    {
        private int id;
        public TID(int id) { this.id = id; }

        int getID() { return this.id; }
    }

    //It is a representation of the Transaction
    //TID must be unique
    public class Transaction
    {
        TID tid;
        public Transaction(TID tid) { this.tid = tid; }

        TID getTID() { return this.tid; }
    }
}
