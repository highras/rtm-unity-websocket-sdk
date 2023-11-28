using System;
using System.Collections.Generic;
using com.fpnn.proto;

namespace com.fpnn.rtm
{
    public partial class RTMClient
    {
        //===========================[ Get Online Users ]=========================//
        //-- Action<online_uids, errorCode>
        public bool GetOnlineUsers(Action<HashSet<long>, int> callback, HashSet<long> uids, int timeout = 0)
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

            Quest quest = new Quest("getonlineusers");
            quest.Param("uids", uids);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                HashSet<long> onlineUids = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        onlineUids = WantLongHashSet(answer, "uids");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(onlineUids, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Set User Info ]=========================//
        public bool SetUserInfo(DoneDelegate callback, string publicInfo = null, string privateInfo = null, int timeout = 0)
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

            Quest quest = new Quest("setuserinfo");
            if (publicInfo != null)
                quest.Param("oinfo", publicInfo);
            if (privateInfo != null)
                quest.Param("pinfo", privateInfo);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get User Info ]=========================//
        //-- Action<publicInfo, privateInfo, errorCode>
        public bool GetUserInfo(Action<string, string, int> callback, int timeout = 0)
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

            Quest quest = new Quest("getuserinfo");
            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                string publicInfo = null;
                string privateInfo = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        publicInfo = answer.Want<string>("oinfo");
                        privateInfo = answer.Want<string> ("pinfo");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(publicInfo, privateInfo, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get User Open Info ]=========================//
        //-- Action<Dictionary<uid, public_info>, errorCode>
        public bool GetUserPublicInfo(Action<Dictionary<long, string>, int> callback, HashSet<long> uids, int timeout = 0)
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

            Quest quest = new Quest("getuseropeninfo");
            quest.Param("uids", uids);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                Dictionary<long, string> publicInfos = null;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        publicInfos = WantLongStringDictionary(answer, "info");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(publicInfos, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }
    }
}
