using System;
using System.Collections.Generic;
using com.fpnn.proto;

namespace com.fpnn.rtm
{
    public partial class RTMClient
    {
        public void Bye()
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                AsyncClose();
                return;
            }

            lock (interLocker)
            {
                if (autoReloginInfo != null)
                    autoReloginInfo.Disable();
            }

            Quest quest =  new Quest("bye");
            bool success = client.SendQuest(quest, (Answer answer, int errorCode) => { AsyncClose(); });
            if (!success)
                AsyncClose();
        }

        //===========================[ Add Attributes ]=========================//
        public bool AddAttributes(DoneDelegate callback, Dictionary<string, string> attrs, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("addattrs");
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get Attributes ]=========================//
        //-- Action<attributes, errorCode>
        public bool GetAttributes(Action<Dictionary<string, string>, int> callback, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("getattrs");
            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                Dictionary<string, string> result = null;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        result = WantStringDictionary(answer, "attrs");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(result, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Add Debug Log ]=========================//
        public bool AddDebugLog(DoneDelegate callback, string message, string attrs, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("adddebuglog");
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Add Device ]=========================//
        public bool AddDevice(DoneDelegate callback, string appType, string deviceToken, string tag = null, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("adddevice");
            quest.Param("apptype", appType);
            quest.Param("devicetoken", deviceToken);
            if (tag != null)
                quest.Param("tag", tag);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Remove Device ]=========================//
        public bool RemoveDevice(DoneDelegate callback, string deviceToken, string tag = null, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("removedevice");
            quest.Param("devicetoken", deviceToken);
            if (tag != null)
                quest.Param("tag", tag);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Add Device Push Option ]=========================//
        public bool AddDevicePushOption(DoneDelegate callback, MessageCategory messageCategory, long targetId, HashSet<byte> mTypes = null, int timeout = 0)
        {
            byte type = 99;
            switch (messageCategory)
            {
                case MessageCategory.P2PMessage:
                    type = 0; break;
                case MessageCategory.GroupMessage:
                    type = 1; break;
            }

            return AddDevicePushOption(callback, type, targetId,  mTypes, timeout);
        }

        internal bool AddDevicePushOption(DoneDelegate callback, byte type, long targetId, HashSet<byte> mTypes = null, int timeout = 0)
        {
            if (type > 1)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(ErrorCode.RTM_EC_INVALID_PARAMETER);
                    });

                return false;
            }

            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("addoption");
            quest.Param("type", type);
            quest.Param("xid", targetId);

            if (mTypes != null)
                quest.Param("mtypes", mTypes);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Remove Device Push Option ]=========================//
        public bool RemoveDevicePushOption(DoneDelegate callback, MessageCategory messageCategory, long targetId, HashSet<byte> mTypes = null, int timeout = 0)
        {
            byte type = 99;
            switch (messageCategory)
            {
                case MessageCategory.P2PMessage:
                    type = 0; break;
                case MessageCategory.GroupMessage:
                    type = 1; break;
            }

            return RemoveDevicePushOption(callback, type, targetId, mTypes, timeout);
        }

        internal bool RemoveDevicePushOption(DoneDelegate callback, byte type, long targetId, HashSet<byte> mTypes = null, int timeout = 0)
        {
            if (type > 1)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(ErrorCode.RTM_EC_INVALID_PARAMETER);
                    });

                return false;
            }

            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("removeoption");
            quest.Param("type", type);
            quest.Param("xid", targetId);

            if (mTypes != null)
                quest.Param("mtypes", mTypes);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        
        //===========================[ Get Device Push Option ]=========================//
        //-- Utilities functions
        private Dictionary<long, HashSet<byte>> WantLongByteHashSetDictionary(Message message, string key)
        {
            Dictionary <long, HashSet<byte>> rev = new Dictionary<long, HashSet<byte>>();

            Dictionary<object, object> originalDict = (Dictionary<object, object>)message.Want(key);
            foreach (KeyValuePair<object, object> kvp in originalDict)
            {
                List<object> originalList = (List<object>)(kvp.Value);
                HashSet<byte> resultSet = new HashSet<byte>();

                foreach (object obj in originalList)
                {
                    resultSet.Add((byte)Convert.ChangeType(obj, TypeCode.Byte));
                }
                
                rev.Add((long)Convert.ChangeType(kvp.Key, TypeCode.Int64), resultSet);
            }

            return rev;
        }

        //-- Action<Dictionary<p2p_uid，HashSet<mType>>, Dictionary<groupId, HashSet<mType>>, errorCode>
        public bool GetDevicePushOption(Action<Dictionary<long, HashSet<byte>>, Dictionary<long, HashSet<byte>>, int> callback, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(null, null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("getoption");
            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                Dictionary<long, HashSet<byte>> p2pDictionary = null;
                Dictionary<long, HashSet<byte>> groupDictionary = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        p2pDictionary = WantLongByteHashSetDictionary(answer, "p2p");
                        groupDictionary = WantLongByteHashSetDictionary(answer, "group");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(p2pDictionary, groupDictionary, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }
    }
}