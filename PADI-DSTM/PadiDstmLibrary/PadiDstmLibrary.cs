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
}
