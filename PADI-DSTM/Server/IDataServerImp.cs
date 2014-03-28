﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadiDstmLibrary;
using CommonTypes;

namespace DataServer
{
    class IDataServerImp : MarshalByRefObject, IDataServer
    {
        private readonly string MASTER_ADDRESS = "tcp://localhost:8080/MasterServer";

        private Dictionary<int, PadInt> padIntDB;

        public IDataServerImp()
        {
            this.padIntDB = new Dictionary<int, PadInt>();
        }

        /**
         * Method that allows a user to create a new PadInt
         * object in the DataServer;
         **/
        public PadInt createPadInt(int uid)
        {
            PadInt padIntObject = null;
            if(!PermissionToCreate(uid)){
               padIntObject = new PadInt(uid);
               this.padIntDB.Add(uid, padIntObject);
            }
            return padIntObject;
        }

        public PadInt accessPadInt(int uid)
        {
            throw new NotImplementedException();
        }

        public bool TxBegin()
        {
            throw new NotImplementedException();
        }

        public bool TxCommit()
        {
            throw new NotImplementedException();
        }

        public bool TxAbort()
        {
            throw new NotImplementedException();
        }

        public bool Fail(string URL)
        {
            throw new NotImplementedException();
        }

        public bool Freeze(string URL)
        {
            throw new NotImplementedException();
        }

        public bool Recover(string URL)
        {
            throw new NotImplementedException();
        }

        /**
         * Method that verifies if the a PadInt object with identifier 
         * uid can be created at the DataServer.
         **/
        public bool PermissionToCreate(int uid)
        {
            bool answerRequest = false;
            IMasterServer remoteObject;
            remoteObject = (IMasterServer)Activator.GetObject(
                typeof(IMasterServer),
                MASTER_ADDRESS);
            try
            {
                answerRequest = remoteObject.ObjectExists(uid);
            } 
            catch(Exception e)
            {
                Console.WriteLine("The remote call throw the exception : " + e);
            }
            return answerRequest;
        }
    }
}
