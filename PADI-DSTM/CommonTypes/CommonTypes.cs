
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTypes
{

    /// <summary>
    /// IDataServer - Commom Interface used by the data servers.
    /// </summary>
    public interface IDataServer
    {
        /**
         *  IDataServer - CreateObject.
         *  @param uid - int object uid.
         */
        void CreateObject(int uid);
        /**
         *  IDataServer - AccessObject.
         *  @param uid - int object uid.
         *  @return PadIntServer reference.
         */
        PadIntServer AccessObject(int uid);
        /**
         *  IDataServer - Disconnect.
         *  @return bool - confirmation.
         */
        bool Disconnect();
        /**
         *  IDataServer - DumpState.
         *  @return bool - confirmation.
         */
        bool DumpState();
        /**
         *  IDataServer - FreezeDataServer.
         *  @return bool - confirmation.
         */
        bool FreezeDataServer();
        /**
         *  IDataServer - RecoverDataServer.
         *  @return bool - confirmation.
         */
        bool RecoverDataServer();
        /**
         *  IDataServer - CanCommit
         *  Data Servers will implement participant's transaction interface
         *  Call from coordinator to participant to ask whether it can commit a transaction
         *  Participant replies with its vote.
         *  @param tid - Transaction.
         *  @return bool - confirmation.
         */
        bool CanCommit(TID tid);

        /**
         *  IDataServer - DoAbort
         *  Call from coordinator to participant to tell participant to commit its part of a transaction.
         *  @param padint - TransactionPadInt
         */
        void DoCommit(TranscationPadInt padint);

        /**
         *  IDataServer - DoCommit
         *  Call from coordinator to participant to tell participant to abort its part of a transaction.
         *  @param trans
         */
        void DoAbort(TID tid);

        /**
         *  IDataServer - updatePrimaryState
         *  Replication Methods.
         *  @param primaryIsAlive - boolean
         */
        void updatePrimaryState(bool primaryIsAlive);

        /**
       * IDataServer - updatePadInt 
       * Replication Methods
       * @param id - integer
       * @param value - integer
       **/
        void UpdatePadInt(int id, int value);

        /**
        * IDataServer - PopulateReplica 
        * Replication Methods
        * @param primaryDataBase - Dictionary
        **/
        void PopulateReplica(Dictionary<int, PadIntServer> primaryDataBase);
    }

    /// <summary>
    /// IMasterServer - Common Interfaces used by the master server.
    /// 
    /// </summary>
    public interface IMasterServer
    {
        /**
         *  IMasterServer - ObjectExists
         *  @param uid - int uid.
         *  @return bool - object exists?.
         */
        bool ObjectExists(int uid);
        /**
         *  IMasterServer - ObjCreatedSuccess
         *  @param url - string url.
         *  @param uid - int uid.
         *  @return bool - object created with success?.
         */
        bool ObjCreatedSuccess(string url, int uid);
        /**
         *  IMasterServer - ShowDataServersState
         *  @return bool - .
         */
        bool ShowDataServersState();
        /**
         *  IMasterServer - GetDataServerAddress
         *  @return string - Data Server Address.
         */
        string GetDataServerAddress();
        /**
         *  IMasterServer - GetPadIntLocation
         *  @param uid - int.
         *  @return string - PadInt Location.
         */
        string GetPadIntLocation(int uid);
        /**
         *  IMasterServer - CreateDataServerReplica
         *  @param dataServerID - data server id.
         *  @param port - replica port.
         *  @return string - Replica location.
         */
        string CreateDataServerReplica(int dataServerID, int port);
        /**
         *  IMasterServer - RegisterDataServer
         *  @param url - string.
         *  @return int - .
         */
        int RegisterDataServer(string url);
        /**
         *  IMasterServer - notifyMasterAboutFailure
         *  Notify Master about the failure.
         *  @param id - int.
         *  @param address - string.
         */
        void notifyMasterAboutFailure(int id, string address);
        /**
         *  IMasterServer - OpenTransaction
         *  Start a new transaction and delivers a unique TID trans.
         *  @return TID
         */
        TID OpenTransaction();
        /**
         *  IMasterServer - CloseTransaction
         *  Ends a transaction: (true) a commit return value indicates that the transaction has committed
         *  (false) an abort return value indicates that it has aborted
         *  @param tid.
         *  @return bool.
         */
        bool CloseTransaction(TID tid);
        /**
         *  IMasterServer - AbortTransaction
         *  Aborts the transaction.
         *  @param tid - TID object.
         */
        void AbortTransaction(TID tid);
        /**
         *  IMasterServer - Join
         *  Informs a coordinator that a new participant has joined the transaction trans.
         *  @param trans - TID.
         *  @param participant - participation identification.
         */
        void Join(TID trans, int participant);
        /**
         *  IMasterServer - HaveCommitted
         *  Call from participant to coordinator to confirm that it has committed the transaction.
         *  @param tid - TID.
         *  @param participant - participation identification.
         */
        void HaveCommitted(TID tid, int participant);
        /**
         *  IMasterServer - GetDecision
         *  Call from participant to coordinator to ask for the decision on a transaction
         *  when it has voted Yes but has still had no reply after some delay.
         *  Used to recover from server crash or delayed messages.
         *  @param tid - TID.
         *  @return bool.
         */

        bool GetDecision(TID tid);

        void AddWriteToTrans(int uid, int value, TID tid);

        void AddReadToTrans(int uid, TID tid);
    }

    /// <summary>
    /// TranscationPadInt - Used by master to save transaction operations.
    /// </summary>
    [Serializable()]
    public struct TranscationPadInt
    {
        /*
         * TranscationPadInt variables
         */
        public int _UID;
        public int _value;

        /*
         * TranscationPadInt constructor
         * @param UID - int.
         * @param wasWrite - bool.
         * @param value - int.
         */
        public TranscationPadInt(int UID, int value)
        {
            _UID = UID;
            _value = value;
        }

        public TranscationPadInt(int UID)
        {
            _UID = UID;
            _value = 0;
        }
    }
}