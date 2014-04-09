using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using PADI_DSTM;

namespace DataServer
{
    class IDataServerImp : MarshalByRefObject, IDataServer
    {
        private readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";

        private string DATA_SERVER_ADDRESS;
        private Dictionary<int, PadInt> padIntDB;

        public IDataServerImp(int port){
            this.DATA_SERVER_ADDRESS = "tcp://" + System.Environment.MachineName + port + "/DataServer";
            this.padIntDB = new Dictionary<int, PadInt>();
        }

        /**
         */
        public Dictionary<int, PadInt> returnPadIntDB() {
            return this.padIntDB;
        }

        /**
         * Method that allows a user to create a new PadInt
         * object in the DataServer;
         **/
        public PadInt createObject(int uid){


            if (!PermissionToCreate(uid))
            {
                PadInt padIntObject = new PadInt(uid);
                this.padIntDB.Add(uid, padIntObject);
                NotifyMasterServer(DATA_SERVER_ADDRESS, uid);
                Console.WriteLine("Object with id " + uid + " created with sucess\r\n");
                //THIS IS JUST FOR DEBUG ->
                serializeMyObjects();

                return padIntObject;
            }
            else
            {
                Console.WriteLine("ERROR: The object with id " + uid + " already exists.");
                return null;
            }
        }


        public PadInt accessObject(int uid){
            return this.padIntDB[uid];
        }
    
        /**
         * Method that verifies if the a PadInt object with identifier 
         * uid can be created at the DataServer.
         **/
        public bool PermissionToCreate(int uid){
            bool answerRequest = false;
            IMasterServer remoteObject;
            remoteObject = (IMasterServer)Activator.GetObject(
                typeof(IMasterServer),
                MASTER_SERVER_ADDRESS);
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

        /**
         * Method that notify the MasterServer when a PadInt object is created
         * sucessfully.
         **/
        public void NotifyMasterServer(string url, int uid){
            IMasterServer remoteObject;
            remoteObject = (IMasterServer)Activator.GetObject(
                typeof(IMasterServer),
                MASTER_SERVER_ADDRESS);
            try
            {
                remoteObject.ObjCreatedSuccess(url, uid);
            }
            catch (Exception e)
            {
                Console.WriteLine("The remote call throw the exception : " + e);
            }
        }

        //Method to perform Serializable function
        public void serializeMyObjects() { 
            bool result = PadiDstm.serializeObjects(this.DATA_SERVER_ADDRESS, this.padIntDB);
            //TODO do something with the bool value
        }

        //Get Data Server saved objects
        public void getSavedObjects() { 
        
        }


        public bool CanCommit(Transaction trans)
        {
            throw new NotImplementedException();
        }

        public void DoCommit(Transaction trans)
        {
            throw new NotImplementedException();
        }

        public void DoAbort(Transaction trans)
        {
            throw new NotImplementedException();
        }
    }
}
