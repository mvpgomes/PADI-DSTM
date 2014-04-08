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
    [Serializable]
    public class PadInt
    {
        private int uid;
        private int padIntValue;

        public PadInt(int uid)
        {
            this.uid = uid;
        }
        
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

    /**
     * Commom Interfaces used by the data servers.
     **/
    public interface IDataServer
    {
        PadInt CreateObject(int uid);
        PadInt AccessObject(int uid);
        string Disconnect();
        bool DumpState();
    }

    /**
     * Common Interfaces used by the master server
     **/
    public interface IMasterServer
    {
        bool ObjectExists(int uid);
        bool ObjCreatedSuccess(string url, int uid);
        string GetDataServerAddress();
        string GetPadIntLocation(int uid);
        int RegisterDataServer(string url);
        bool ShowDataServersState();
    }
}
