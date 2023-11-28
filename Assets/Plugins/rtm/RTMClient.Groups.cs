using System;
using System.Collections.Generic;
using com.fpnn.proto;

namespace com.fpnn.rtm
{
    public partial class RTMClient
    {
        //===========================[ Add Group Members ]=========================//
        public bool AddGroupMembers(DoneDelegate callback, long groupId, HashSet<long> uids, int timeout = 0)
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

            Quest quest = new Quest("addgroupmembers");
            quest.Param("gid", groupId);
            quest.Param("uids", uids);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Delete Group Members ]=========================//
        public bool DeleteGroupMembers(DoneDelegate callback, long groupId, HashSet<long> uids, int timeout = 0)
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

            Quest quest = new Quest("delgroupmembers");
            quest.Param("gid", groupId);
            quest.Param("uids", uids);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get Group Members ]=========================//
        //-- Action<uids, errorCode>
        public bool GetGroupMembers(Action<HashSet<long>, int> callback, long groupId, int timeout = 0)
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

            Quest quest = new Quest("getgroupmembers");
            quest.Param("gid", groupId);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                HashSet<long> uids = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        uids = WantLongHashSet(answer, "uids");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(uids, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //-- Action<member_uids, online_uids, errorCode>
        public bool GetGroupMembers(Action<HashSet<long>, HashSet<long>, int> callback, long groupId, int timeout = 0)
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

            Quest quest = new Quest("getgroupmembers");
            quest.Param("gid", groupId);
            quest.Param("online", true);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                HashSet<long> allUids = null;
                HashSet<long> onlineUids = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        allUids = WantLongHashSet(answer, "uids");
                        onlineUids = GetLongHashSet(answer, "onlines");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(allUids, onlineUids, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get Group Count ]=========================//
        //-- Action<member_count, errorCode>
        public bool GetGroupCount(Action<int, int> callback, long groupId, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("getgroupcount");
            quest.Param("gid", groupId);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                int memberCount = 0;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        memberCount = answer.Want<int>("cn");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(memberCount, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //-- Action<member_count, online_count, errorCode>
        public bool GetGroupCount(Action<int, int, int> callback, long groupId, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("getgroupcount");
            quest.Param("gid", groupId);
            quest.Param("online", true);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                int memberCount = 0;
                int onlineCount = 0;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        memberCount = answer.Want<int>("cn");
                        onlineCount = answer.Want<int>("online");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(memberCount, onlineCount, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get User Groups ]=========================//
        //-- Action<groupIds, errorCode>
        public bool GetUserGroups(Action<HashSet<long>, int> callback, int timeout = 0)
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

            Quest quest = new Quest("getusergroups");
            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                HashSet<long> groupIds = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        groupIds = WantLongHashSet(answer, "gids");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(groupIds, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Set Group Info ]=========================//
        public bool SetGroupInfo(DoneDelegate callback, long groupId, string publicInfo = null, string privateInfo = null, int timeout = 0)
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

            Quest quest = new Quest("setgroupinfo");
            quest.Param("gid", groupId);
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

        //===========================[ Get Group Info ]=========================//
        //-- Action<publicInfo, privateInfo, errorCode>
        public bool GetGroupInfo(Action<string, string, int> callback, long groupId, int timeout = 0)
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

            Quest quest = new Quest("getgroupinfo");
            quest.Param("gid", groupId);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                string publicInfo = "";
                string privateInfo = "";

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        publicInfo = answer.Want<string>("oinfo");
                        privateInfo = answer.Want<string>("pinfo");
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

        //===========================[ Get Group Open Info ]=========================//
        //-- Action<public_info, errorCode>
        public bool GetGroupPublicInfo(Action<string, int> callback, long groupId, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(string.Empty, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("getgroupopeninfo");
            quest.Param("gid", groupId);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                string publicInfo = "";
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    { publicInfo = answer.Want<string>("oinfo"); }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(publicInfo, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(string.Empty, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get Groups Open Info ]=========================//
        //-- Action<Dictionary<groupId, public_info>, errorCode>
        public bool GetGroupsPublicInfo(Action<Dictionary<long, string>, int> callback, HashSet<long> groupIds, int timeout = 0)
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

            Quest quest = new Quest("getgroupsopeninfo");
            quest.Param("gids", groupIds);

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
