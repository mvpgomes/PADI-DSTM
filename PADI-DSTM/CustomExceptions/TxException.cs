using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExceptions
{
    [Serializable()]
    public class TxException : System.Exception
    {
        public TxException() : base() { }
        public TxException(string message) : base(message) { }
        public TxException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected TxException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
