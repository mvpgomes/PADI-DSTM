using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadiDstmLibrary
{
    public interface PadiDstmLibrary
    {
        bool Init();
        bool TxBegin();
        bool TxCommit();
        bool TxAbort();
        bool Status();
        bool Fail(string URL);
        bool Freeze(string URL);
        bool Recover(string URL);
    }

    /**
    * Class that represents a PadInt object that is shared between the distributed system.
    **/
    public class PadInt
    {
        private int uid;
        private int padIntValue;

        /**
         * Declare a ID property of type integer : 
         **/
        public int ID
        {
            get
            {
                return uid;
            }

            set
            {
                uid = value;
            }
        }

        /**
         * Declare a Value property of type integer : 
         **/
        public int Value
        {
            get
            {
                return padIntValue;
            }

            set
            {
                padIntValue = value;
            }
        }
    }
}
