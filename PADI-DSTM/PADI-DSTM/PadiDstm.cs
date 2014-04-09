using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTypes;
using System.Runtime.Remoting;
using System.IO;

namespace PADI_DSTM
{
    public class PadiDstm
    {
        private static IDataServer remoteServer;
        private static IMasterServer remoteMaster;
        private static System.Xml.Serialization.XmlSerializer x;

        private static Dictionary<int, PadInt> cachedObjects;

        public static readonly string MASTER_SERVER_ADDRESS = "tcp://localhost:8086/MasterServer";

        /**
         * 
         */
        public static bool Init()
        {
            cachedObjects = cachedObjects = new Dictionary<int, PadInt>();
            remoteMaster = getMasterInstance();
            return true;
        }

        /**
         * 
         */
        public static bool TxBegin()
        {
            return true;
        }

        /**
         * 
         */
        public static bool TxCommit()
        { 
            return true;
        }

        /**
         * 
         */
        public static bool Status()
        {
            return true;
        }

        /**
         * 
         */
        public static bool Fail(string URL)
        {
            return true;
        }

        /**
         * 
         */
        public static bool Freeze(string URL)
        {
            return true;
        }

        /**
         * 
         */
        public static bool Recover(string URL)
        {
            return true;
        }

        /**
         * Create PadInt
         * Arguments: uid - PadInt identifier
         * Objective: Create a new PadInt
         * First we ask Master Server for a location.
         * After stablishing connection to Data Server, that server asks Master Server if that object already exists.
         * If it already exist: A null message is returned
         * If does not exist: 1) A new PadInt is created with uid as identifier
         *                    2) A reference to that object is returned to the client 
         */
        public static PadInt CreatePadInt(int uid)
        {
            PadInt reference = null;

            try
            {
               string dataServerAddress = remoteMaster.getDataServerAddress();
                remoteServer = (IDataServer)Activator.GetObject(
                    typeof(IDataServer),
                    dataServerAddress);

                reference = remoteServer.createObject(uid);
                cachedObjects.Add(uid, reference);
            }
            catch (Exception) { return null; }

            return reference;
        }

        /**
         * Access PadInt
         * Arguments: uid - PadInt identifier
         * Objective: Access some PadInt
         * If the client already contains PadInt location: an access to that location is performed
         * If the client doesn't have that information: 1) He asks Master Server for it.
         *                                              2) After saving that information, cliente requests Data Server for PadInt reference.
         *                                              3) Client have not a reference for that PadInt.
         */
        public static PadInt AccessPadInt(int uid)
        {
            PadInt reference = null;

            if (!cachedObjects.ContainsKey(uid))
            {
                try
                {
                    string dataServerAddress = remoteMaster.getPadIntLocation(uid);
                    remoteServer = (IDataServer)Activator.GetObject(
                         typeof(IDataServer),
                         dataServerAddress);

                    reference = remoteServer.accessObject(uid);
                }
                catch (Exception) { return null; }
            }
            return reference;
        }

        private static IMasterServer getMasterInstance()
        {
            if (remoteMaster == null)
            {
                remoteMaster = (IMasterServer)Activator.GetObject(
                    typeof(IMasterServer),
                    MASTER_SERVER_ADDRESS);
            }
            return remoteMaster;
        }

        /**
         * Serializable functions (bool)
         */
        public static bool serializeObjects(string dataServerName, Dictionary<int, PadInt> objectsToWrite){

            string path = Directory.GetCurrentDirectory();
            string target = @"c:\temp\PADI-DSTM-OBJECTS";
            if (!Directory.Exists(target)){
                Directory.CreateDirectory(target);
            }
            //Let's write
            TextWriter tw = new StreamWriter(@"c:\" + dataServerName + ".txt");

            x = new System.Xml.Serialization.XmlSerializer(objectsToWrite.GetType());
            x.Serialize(tw, objectsToWrite);
            tw.Close();

            return true;
        }

        /**
         * Read from a file that objects
         */
        public static Dictionary<int, PadInt> readObjects(string dataServerName) {
            //Maybe we need to change this
            Dictionary<int, PadInt> returnDictionary = new Dictionary<int, PadInt>();
            try{
                TextReader tr = new StreamReader(@"c:\" + dataServerName + ".txt");
                Dictionary<int, PadInt> padIntContainer = (Dictionary<int, PadInt>)x.Deserialize(tr);
                tr.Close();
                returnDictionary = padIntContainer;
            }
            catch (Exception) { /*We need to do something if its not possible to read later saved objects to 
                                 that server */
            }
            return returnDictionary;
        }


    }

}
