using System;
using System.Collections.Generic;
using com.fpnn.common;
using com.fpnn.proto;

namespace com.fpnn.rtm
{
    public partial class RTMClient
    {
        //===========================[ Enter Room ]=========================//
        public bool EnterRoom(DoneDelegate callback, long roomId, int timeout = 0)
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

            Quest quest = new Quest("enterroom");
            quest.Param("rid", roomId);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        public bool EnterRooms(DoneDelegate callback, HashSet<long> roomIds, int timeout = 0)
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

            Quest quest = new Quest("enterrooms");
            quest.Param("rids", roomIds);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Leave Room ]=========================//
        public bool LeaveRoom(DoneDelegate callback, long roomId, int timeout = 0)
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

            Quest quest = new Quest("leaveroom");
            quest.Param("rid", roomId);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get User Rooms ]=========================//
        //-- Action<roomIds, errorCode>
        public bool GetUserRooms(Action<HashSet<long>, int> callback, int timeout = 0)
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

            Quest quest = new Quest("getuserrooms");
            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                HashSet<long> roomIds = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        roomIds = WantLongHashSet(answer, "rooms");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(roomIds, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Set Room Info ]=========================//
        public bool SetRoomInfo(DoneDelegate callback, long roomId, string publicInfo = null, string privateInfo = null, int timeout = 0)
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

            Quest quest = new Quest("setroominfo");
            quest.Param("rid", roomId);
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

        //===========================[ Get Room Info ]=========================//
        //-- Action<publicInfo, privateInfo, errorCode>
        public bool GetRoomInfo(Action<string, string, int> callback, long roomId, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback("", "", fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("getroominfo");
            quest.Param("rid", roomId);

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
                    callback("", "", fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get Room Open Info ]=========================//
        //-- Action<public_info, errorCode>
        public bool GetRoomPublicInfo(Action<string, int> callback, long roomId, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback("", fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("getroomopeninfo");
            quest.Param("rid", roomId);

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
                    callback("", fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get Rooms Open Info ]=========================//
        //-- Action<Dictionary<roomId, public_info>, errorCode>
        public bool GetRoomsPublicInfo(Action<Dictionary<long, string>, int> callback, HashSet<long> roomIds, int timeout = 0)
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

            Quest quest = new Quest("getroomsopeninfo");
            quest.Param("rids", roomIds);

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

        //===========================[ Get Room Members ]=========================//
        //-- Action<HashSet<uids>, errorCode>
        public bool GetRoomMembers(Action<HashSet<long>, int> callback, long roomId, int timeout = 0)
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

            Quest quest = new Quest("getroommembers");
            quest.Param("rid", roomId);

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

        //===========================[ Get Room Count ]=========================//
        //-- Action<Dictionary<roomId, count>, errorCode>
        public bool GetRoomMemberCount(Action<Dictionary<long, int>, int> callback, HashSet<long> roomIds, int timeout = 0)
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

            Quest quest = new Quest("getroomcount");
            quest.Param("rids", roomIds);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                Dictionary<long, int> counts = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        counts = WantLongIntDictionary(answer, "cn");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(counts, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get User Room Last Message ]=========================//
        void GetRoomLastMessage(ref Dictionary<long, HistoryMessage> roomMessages, Answer answer)
        {
            Dictionary<object, object> originalDict = (Dictionary<object, object>)answer.Want("rooms");
            foreach (KeyValuePair<object, object> kvp in originalDict)
            {
                long roomId = (long)Convert.ChangeType(kvp.Key, TypeCode.Int64);
                List<object> items = (List<object>)kvp.Value;

                HistoryMessage message = new HistoryMessage();
                message.cursorId = (long)Convert.ChangeType(items[0], TypeCode.Int64);
                message.fromUid = (long)Convert.ChangeType(items[1], TypeCode.Int64);
                message.toId = roomId;
                message.messageType = (byte)Convert.ChangeType(items[2], TypeCode.Byte);
                message.messageId = (long)Convert.ChangeType(items[3], TypeCode.Int64);

                if (!CheckBinaryType(items[5]))
                    message.stringMessage = (string)Convert.ChangeType(items[5], TypeCode.String);
                else
                    message.binaryMessage = (byte[])items[5];

                message.attrs = (string)Convert.ChangeType(items[6], TypeCode.String);
                message.modifiedTime = (long)Convert.ChangeType(items[7], TypeCode.Int64);

                if (message.messageType >= 40 && message.messageType <= 50)
                    RTMClient.BuildFileInfo(message, errorRecorder);

                roomMessages.Add(roomId, message);
            }
        }

        public bool GetUserRoomLastMessage(Action<Dictionary<long, HistoryMessage>, int> callback, HashSet<long> mtypes = null, int timeout = 0)
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

            Quest quest = new Quest("getuserroomsandlastmsg");
            if (mtypes != null)
                quest.Param("mtypes", mtypes);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                Dictionary<long, HistoryMessage> roomMessages = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        roomMessages = new Dictionary<long, HistoryMessage>();
                        GetRoomLastMessage(ref roomMessages, answer);               
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(roomMessages, errorCode);
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
