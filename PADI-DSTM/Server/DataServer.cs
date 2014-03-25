using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Threading.Tasks;

namespace DataServer
{
    class DataServer
    {
        static void Main(string[] args)
        {
            IDataServerImp myDataServ = new IDataServerImp();

            //registering server at port 8081
            TcpChannel channel = new TcpChannel(8081);
            ChannelServices.RegisterChannel(channel, true);

            RemotingServices.Marshal(myDataServ, "DataServer",
                typeof(IDataServerImp));

            System.Console.WriteLine("<enter> DataServer is running...");
            System.Console.ReadLine();
        }
    }
}
