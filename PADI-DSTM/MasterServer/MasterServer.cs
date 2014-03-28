﻿using System;
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
           
             //registering server at port 8086
            TcpChannel channel = new TcpChannel(8086);
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(typeof(IMasterServerImp),
               "MasterServer", WellKnownObjectMode.Singleton);

            System.Console.WriteLine("<enter> MasterServer is running...");
            System.Console.ReadLine();
        }
    }
}
