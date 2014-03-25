using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadiDstmLibrary;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace MasterServer
{
    class MasterServer
    {
        static void Main(string[] args)
        {
            IMasterServerImp myMS = IMasterServerImp.Instance;

            //registering server at port 8086
            TcpChannel channel = new TcpChannel(8086);
            ChannelServices.RegisterChannel(channel, true);

            RemotingServices.Marshal(myMS, "MasterServer",
                typeof(IMasterServerImp));

            System.Console.WriteLine("<enter> MasterServer is running...");
            System.Console.ReadLine();
        }
    }
}
