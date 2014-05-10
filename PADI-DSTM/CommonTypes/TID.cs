using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    /// <summary>
    /// TID - This is a encapsulation of the Transaction Identifier.
    /// 
    /// </summary>
    [Serializable()]
    public class TID : IEquatable<TID>
    {
        /**
         * TID variables.
         */
        private int id;
        /**
         *  TID constructor
         *  @param id - int that represents transaction id.
         */
        public TID(int id)
        {
            this.id = id;
        }
        /**
         *  TID - GetID
         *  @return transaction id.
         */
        public int GetID() { return this.id; }
        /**
         *  TID - ToString
         *  @return string - transaction content.
         */
        public override string ToString()
        {
            return "TID value: " + this.id.ToString();
        }
        /**
         *  TID - Equals
         *  Comparator for lists.
         *  @return bool - equals?.
         */
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            TID tid = obj as TID;
            if ((System.Object)tid == null)
            {
                return false;
            }
            return tid.GetID() == this.id;
        }
        /**
         *  TID - Equals
         *  Comparator for transactions identifier.
         *  @return bool - equals?.
         */
        public bool Equals(TID other)
        {
            if (other == null) return false;
            return (this.id.Equals(other.GetID()));
        }

        public override int GetHashCode()
        {
            return this.id;
        }
    }
}
