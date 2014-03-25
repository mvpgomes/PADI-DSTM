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
}
