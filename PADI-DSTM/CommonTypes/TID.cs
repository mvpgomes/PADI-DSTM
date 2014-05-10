using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    //This is a encapsulation of the Transaction Identifier
    [Serializable()]
    public class TID : IEquatable<TID>
    {
        private int id;
        /**
         * Operations made by client 
         */
        private List<PadInt> operations;

        public TID(int id)
        {
            this.id = id;
            this.operations = new List<PadInt>();
        }

        public int GetID() { return this.id; }

        public void addOperations(List<PadInt> listOfOperations)
        {
            this.operations.AddRange(listOfOperations);
        }

        public override string ToString()
        {
            return "TID value: " + this.id.ToString();
        }
        //Comparator for lists
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }
            // If parameter cannot be cast to TID return false.
            TID tid = obj as TID;
            if ((System.Object)tid == null)
            {
                return false;
            }
            // Return true if the fields match:
            return tid.GetID() == this.id;
        }

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
