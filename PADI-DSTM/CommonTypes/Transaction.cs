using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
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
}
