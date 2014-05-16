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
    public class Transaction : IEquatable<Transaction>
    {
        /*
         * Transaction variables.
         */
        private TID tid;
  
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
  
            this.writeSet = new Dictionary<int, TranscationPadInt>();
            this.readSet = new Dictionary<int, TranscationPadInt>();

            this.startTn = startTn;
        }

        public int GetSartTn() 
        {
            return startTn;
        }

        public List<int> GetOperatedUID()
        {
            List<int> writes = new List<int>(writeSet.Keys);
            List<int> reads = new List<int>(readSet.Keys);
            writes.AddRange(reads);

            return writes;
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

        public bool Equals(Transaction other)
        {
            if (other == null)
            {
                return false;
            }
            return this.tid.Equals(other.GetTID());
        }
    }
}
