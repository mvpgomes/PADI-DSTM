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
    [Serializable()]
    public class PadInt
    {
        private int uid;
        private int value;
        private bool read;
        private bool write;


        public PadInt(int uid)
        {
            this.uid = uid;
            this.value = 0;
            this.read = false;
            this.write = false;
        }

        public PadInt(int uid, int value)
        {
            this.uid = uid;
            this.value = value;
            this.read = false;
            this.write = false;
        }

        public bool isRead()
        {
            return read;
        }

        public bool isWrite()
        {
            return write;
        }

        public int getUID()
        {
            return this.uid;
        }

        /**
         *  Method that reads the object PadInt, and return the value of the object.
         *  @throws TxException
         **/
        public int Read()
        {
            this.read = true;
            return this.value;
        }

        /**
         * Method that writes a value in a PadInt object.
         * @throws TxException
         **/
        public void Write(int value)
        {
            this.write = true;
            //local changes
            this.value = value;
        }

        public override string ToString()
        {
            return "uid: " + this.uid + " val: " + this.value;
        }
    }
}
