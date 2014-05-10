using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{
    /// PadIntServer - PadIntServer specification.
    /// This object represents a PadInt on the data server side.
    /// </summary>
    [Serializable()]
    public class PadIntServer
    {
        /**
         *  PadIntServer variables
         */
        private int uid;
        private int value;
        /**
         *  PadInt constructor
         *  @param uid - int that represents PadIntServer uid.
         */
        public PadIntServer(int uid)
        {
            this.uid = uid;
            this.value = 0;
        }
        /**
         *  PadIntServer - GetUID - Method that returns the PadIntServer UID.
         *  Method returns an int.
         */
        public int GetUID()
        {
            return uid;
        }
        /**
         *  PadIntServer - GetValue - Method that returns the PadIntServer value.
         *  @return an int - the PadIntServer value.
         */
        public int GetValue()
        {
            return this.value;
        }
        /**
         *  PadIntServer - SetValue - Method sets the value of value variable.
         *  @param value - int - the new PadIntServer value.
         */
        public void SetValue(int value)
        {
            this.value = value;
        }
        /**
         *  PadIntServer - ToString - ToString PadIntServer content.
         *  @return a string.
         */
        public override string ToString()
        {
            return "uid: " + this.uid + " val: " + this.value;
        }
    }
}
