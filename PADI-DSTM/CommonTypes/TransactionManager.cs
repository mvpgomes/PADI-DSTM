using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    public class TransactionManager
    {
        private Dictionary<TID, Transaction> trans;

        public TransactionManager()
        {
            this.trans = new Dictionary<TID, Transaction>();
        }

        public bool AddTransaction(Transaction trans)
        {
            bool res = false;

            try
            {
                this.trans.Add(trans.GetTID(), trans);
                res = true;
            }
            catch (Exception) { return res; }

            return res;
        }

        public void RemoveTransaction(Transaction trans)
        {
            this.trans.Remove(trans.GetTID());
        }

        public Transaction GetTransaction(TID tid)
        {
            Transaction t = null;
            try
            {
                t = this.trans[tid];
            }
            catch (Exception) { return null; }

            return t;
        }

        public override string ToString()
        {
            string aux = "";

            foreach (KeyValuePair<TID, Transaction> entry in this.trans)
            {
                aux += entry.Key.ToString() + entry.Value.ToString() + " | ";
            }
            return aux;
        }

    }

}
