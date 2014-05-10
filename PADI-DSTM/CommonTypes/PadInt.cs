using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    /// <summary>
    /// PadInt - Class that represents a PadInt object that is shared between the distributed system.
    /// 
    /// </summary>
    [Serializable()]
    public class PadInt : MarshalByRefObject
    {
        /**
         *  PadInt variables.
         */
        private int uid;
        private int value;
        private bool read;
        private bool write;
        /**
         *  PadInt constructor.
         *  @param uid - int that represents PadInt UID.
         */
        public PadInt(int uid)
        {
            this.uid = uid;
            this.value = 0;
            this.read = false;
            this.write = false;
        }
        /**
         *  PadInt constructor.
         *  @param uid - int that represents PadInt UID.
         *  ~@param value - int that represents PadInt value.
         */
        public PadInt(int uid, int value)
        {
            this.uid = uid;
            this.value = value;
            this.read = false;
            this.write = false;
        }
        /**
         *  PadInt - wasRead - Method to know if the PadInt was read.
         *  @return bool.
         */
        public bool wasRead()
        {
            return read;
        }
        /**
         *  PadInt - wasWrite - Method to know if the PadInt was wrote.
         *  @return bool.
         */
        public bool wasWrite()
        {
            return write;
        }
        /**
         *  PadInt - getUID - Method to get the UID from the padInt.
         *  @return an int that represents de PadInt UID.
         */
        public int getUID()
        {
            return this.uid;
        }
        /**
         *  PadInt - Read - Method that reads the object PadInt, and return the value of the object.
         *  @throws TxException
         *  @return int that represents PadInt value.
         */
        public int Read()
        {
            this.read = true;
            return this.value;
        }
        /**
         *  PadInt - Write - Method that writes a value in a PadInt object.
         *  @param value - write value into PadInt
         *  @throws TxException
         */
        public void Write(int value)
        {
            this.write = true;
            this.value = value;
        }
        /**
         *  PadInt - ToString - ToString the padInt content.
         *  @return string 
         */
        public override string ToString()
        {
            return "uid: " + this.uid + " val: " + this.value;
        }
    }
}
