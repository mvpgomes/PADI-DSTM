using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class TransactionSystem
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
            if (!HasJoined(tid))
            {
                //join in master
                master.Join(tid, participant);
                this.joined.Add(tid);
            }
        }

        public bool HasJoined(TID tid)
        {
            return this.joined.Contains(tid);
        }

        public void CloseTransaction(TID tid)
        {
            this.joined.Remove(tid);
        }

    }
}
