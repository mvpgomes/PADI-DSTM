using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;

namespace PadiDstmLibrary
{
    /**
     * This interface is implemented by the DataServer and its used by the client to create 
     * and access a PadInt object, and to realize some tests.
     **/
    public interface IDataServer
    {
        PadInt createPadInt(int uid);
        PadInt accessPadInt(int uid);
        bool TxBegin();
        bool TxCommit();
        bool TxAbort();
        bool Fail(string URL);
        bool Freeze(string URL);
        bool Recover(string URL);
    }

    /*
     * This interface is implemented by the MasterServer and its used by the Application
     * and the DataServer.
     **/
    public interface IMasterServer
    {
        bool Init();
        bool Status();
        string RegDataServer(string url);
        string GetServerAddr();
        bool ExistObject(int uid);
        void ObjCreatedSuccess(string url, int uid);
     }
}
