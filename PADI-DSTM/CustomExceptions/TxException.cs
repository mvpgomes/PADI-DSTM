using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExceptions
{
    /// <summary>
    /// TxException - Exception speficication.
    /// </summary>
    [Serializable()]
    public class TxException : System.Exception
    {
        /**
         *  TxException constructor.
         */
        public TxException() : base() { }
        /**
         *  TxException constructor.
         *  @param message - string message.
         */
        public TxException(string message) : base(message) { }
        /**
         *  TxException constructor.
         *  @param message - string message.
         *  @param inner - System.Exception inner.
         */
        public TxException(string message, System.Exception inner) : base(message, inner) { }
        /**
         *   TxException - constructor.
         *   A constructor is needed for serialization when an 
         *   exception propagates from a remoting server to the client.
         *   @param info - System.Runtime.Serialization.SerializationInfo info.
         *   @param context - System.Runtime.Serialization.StreamingContext context.
         */
        protected TxException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
