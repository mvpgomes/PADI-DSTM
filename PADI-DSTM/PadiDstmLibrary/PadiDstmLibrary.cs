using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadiDstmLibrary
{
    /**
     * This interface is implemented by the DataServer and its used by the client to create 
     * and access a PadInt object.
     **/
    public interface IDataServer
    {
        PadInt createPadInt(int uid);
        PadInt accessPadInt(int uid);
    }

    public interface IMasterServer
    {
        string RegDataServer(string url);
        string GetServerAddr();
        bool ExistObject(int uid);
        void ObjCreatedSuccess(string url, int uid);
    }
    
    /**
     * TODO : Verficar como funciona a biblioteca.
     **/ 
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
         *  Method that reads the object PadInt, and return the value of the object.
         *  @throws TxException
         **/
        public int Read()
        {
            return this.padIntValue;
        }

        /**
         * Method that writes a value in a PadInt object.
         * @throws TxException
         **/
        public void Write(int value)
        {
            this.padIntValue = value;
        }
    }
}
