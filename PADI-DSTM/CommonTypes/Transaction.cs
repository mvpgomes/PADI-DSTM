using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    //It is a representation of the Transaction
    //TID must be unique
    [Serializable()]
    public class Transaction
    {
        private TID tid;
        private List<int> partcipants;

        private List<PadInt> readSet;
        private List<PadInt> writeSet;

        public Transaction(TID tid)
        {
            this.tid = tid;
            partcipants = new List<int>();
            readSet = new List<PadInt>();
            writeSet = new List<PadInt>();
        }

        public TID GetTID() { return this.tid; }

        public void AddParticipant(int participant)
        {
            this.partcipants.Add(participant);
        }

        public void AddReadSet(PadInt padInt)
        {
            this.readSet.Add(padInt);
        }
        public void AddWriteSet(PadInt padInt)
        {
            this.writeSet.Add(padInt);
        }

        public List<int> GetParticipants() { return this.partcipants; }

        public List<PadInt> GetReadSet() { return this.readSet; }

        public List<PadInt> GetWriteSet() { return this.writeSet; }

        public override string ToString()
        {
            return "Trans with " + tid.ToString();
        }
    }

}
