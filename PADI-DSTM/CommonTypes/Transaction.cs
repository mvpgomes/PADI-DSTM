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

        private Dictionary<int, TranscationPadInt> writeSet;
        private Dictionary<int, TranscationPadInt> readSet;

        private int startTn;
        
        /**
         * Transaction constructor
         * @param tid - TID.
         */
        public Transaction(TID tid, int startTn)
        {
            this.tid = tid;
            this.partcipants = new List<int>();

            this.writeSet = new Dictionary<int, TranscationPadInt>();
            this.readSet = new Dictionary<int, TranscationPadInt>();

            this.startTn = startTn;
        }

        public int GetSartTn() 
        {
            return startTn;
        }

        public Dictionary<int, TranscationPadInt> WriteValues() 
        {
            return this.writeSet;
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

        public void AddWrite(int uid, int value)
        {
            TranscationPadInt trans = new TranscationPadInt(uid, value);
            this.writeSet.Add(uid, trans);
        }

        public void AddRead(int uid)
        {
            TranscationPadInt trans = new TranscationPadInt(uid);
            this.readSet.Add(uid, trans);
        }

        public List<int> GetReadSet()
        {
            return new List<int>(this.readSet.Keys);
        }

        public List<int> GetWriteSet()
        {
            return new List<int>(this.writeSet.Keys);
        }
    }
}
