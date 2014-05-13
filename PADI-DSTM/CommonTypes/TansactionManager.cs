using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    /// <summary>
    /// TransactionManager
    /// </summary>
    public class TransactionManager
    {
        private List<Transaction> trans;
        private int lastCommit;

        public TransactionManager()
        {
            this.trans = new List<Transaction>();
            this.lastCommit = 0;
        }

        public bool AddTransaction(Transaction trans)
        {
            bool res = false;

            try
            {
                this.trans.Add(trans);
                res = true;
            }
            catch (Exception) { return res; }

            return res;
        }

        public int GetLastCommit()
        {
            return lastCommit;
        }

        public void RemoveTransaction(Transaction trans)
        {
            this.trans.Remove(trans);
        }

        public Transaction GetTransaction(TID tid)
        {
            Transaction t = null;
            try
            {
                t = this.trans[tid.GetID()];
            }
            catch (Exception) { return null; }

            return t;
        }

        private bool OverlapReads(List<int> readSet, List<int> writeSet)
        {
            IEnumerable<int> intersection = readSet.Intersect(writeSet);
            return intersection.Count() != 0;
        }

        public bool IsValidTransaction(TID tid)
        {
            Transaction trans = this.trans[tid.GetID()];

            int startTn = trans.GetSartTn();
            int finishTn = this.GetLastCommit();
            
            bool valid = true;
            for (int i = startTn + 1; i <= finishTn; i++)
            {
                if (OverlapReads(trans.GetReadSet(), this.trans[i].GetWriteSet()))
                {
                    valid = false;
                }
            }

            return valid;
        }

        public void SetLastCommit(TID tid) 
        {
            this.lastCommit = tid.GetID();
        }

        public override string ToString()
        {
            string aux = "";

            foreach (Transaction trans in this.trans)
            {
                aux += trans.ToString() + trans.ToString() + " | ";
            }
            return aux;
        }
    }
}
