using System;
using System.Collections.Generic;
using com.fpnn.proto;

namespace com.fpnn.rtm
{
    public partial class RTMClient
    {
        //===========================[ Sending Chat ]=========================//
        public bool SendChat(MessageIdDelegate callback, long uid, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendMessage(uid, (byte)MessageType.Chat, message, attrs, callback, 0, timeout);
        }

        public bool SendChat(SendMessageDelegate callback, long uid, string message, string attrs = "", int timeout = 0)
        { 
            return InternalSendMessage(uid, (byte)MessageType.Chat, message, attrs, callback, 0, timeout);
        }

        public bool SendGroupChat(MessageIdDelegate callback, long groupId, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendGroupMessage(groupId, (byte)MessageType.Chat, message, attrs, callback, 0, timeout);
        }

        public bool SendGroupChat(SendMessageDelegate callback, long groupId, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendGroupMessage(groupId, (byte)MessageType.Chat, message, attrs, callback, 0, timeout);
        }

        public bool SendRoomChat(MessageIdDelegate callback, long roomId, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendRoomMessage(roomId, (byte)MessageType.Chat, message, attrs, callback, 0, timeout);
        }

        public bool SendRoomChat(SendMessageDelegate callback, long roomId, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendRoomMessage(roomId, (byte)MessageType.Chat, message, attrs, callback, 0, timeout);
        }

        //===========================[ Sending Cmd ]=========================//
        public bool SendCmd(MessageIdDelegate callback, long uid, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendMessage(uid, (byte)MessageType.Cmd, message, attrs, callback, 0, timeout);
        }

        public bool SendCmd(SendMessageDelegate callback, long uid, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendMessage(uid, (byte)MessageType.Cmd, message, attrs, callback, 0, timeout);
        }

        public bool SendGroupCmd(MessageIdDelegate callback, long groupId, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendGroupMessage(groupId, (byte)MessageType.Cmd, message, attrs, callback, 0, timeout);
        }

        public bool SendGroupCmd(SendMessageDelegate callback, long groupId, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendGroupMessage(groupId, (byte)MessageType.Cmd, message, attrs, callback, 0, timeout);
        }

        public bool SendRoomCmd(MessageIdDelegate callback, long roomId, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendRoomMessage(roomId, (byte)MessageType.Cmd, message, attrs, callback, 0, timeout);
        }

        public bool SendRoomCmd(SendMessageDelegate callback, long roomId, string message, string attrs = "", int timeout = 0)
        {
            return InternalSendRoomMessage(roomId, (byte)MessageType.Cmd, message, attrs, callback, 0, timeout);
        }

        //===========================[ History Chat (Chat & Cmd & Audio) ]=========================//
        private static readonly List<byte> chatMTypes = new List<byte>
        {
            (byte)MessageType.Chat,
            (byte)MessageType.Cmd,
            (byte)MessageType.ImageFile,
            (byte)MessageType.AudioFile,
            (byte)MessageType.VideoFile,
            (byte)MessageType.NormalFile,
        };

        public bool GetGroupChat(HistoryMessageDelegate callback, long groupId, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, int timeout = 0)
        {
            return GetGroupMessage(callback, groupId, desc, count, beginMsec, endMsec, lastId, chatMTypes, timeout);
        }

        public bool GetGroupChatByMessageId(HistoryMessageDelegate callback, long groupId, bool desc, int count, long messageId, long beginMsec = 0, long endMsec = 0, int timeout = 0)
        {
            return GetGroupMessageByMessageId(callback, groupId, desc, count, messageId, beginMsec, endMsec, chatMTypes, timeout);
        }

        public bool GetRoomChat(HistoryMessageDelegate callback, long roomId, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, int timeout = 0)
        {
            return GetRoomMessage(callback, roomId, desc, count, beginMsec, endMsec, lastId, chatMTypes, timeout);
        }

        public bool GetRoomChatByMessageId(HistoryMessageDelegate callback, long roomId, bool desc, int count, long messageId, long beginMsec = 0, long endMsec = 0, int timeout = 0)
        {
            return GetRoomMessageByMessageId(callback, roomId, desc, count, messageId, beginMsec, endMsec, chatMTypes, timeout);
        }

        public bool GetBroadcastChat(HistoryMessageDelegate callback, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, int timeout = 0)
        {
            return GetBroadcastMessage(callback, desc, count, beginMsec, endMsec, lastId, chatMTypes, timeout);
        }

        public bool GetBroadcastChatByMessageId(HistoryMessageDelegate callback, bool desc, int count, long messageId, long beginMsec = 0, long endMsec = 0, int timeout = 0)
        {
            return GetBroadcastMessageByMessageId(callback, desc, count, messageId, beginMsec, endMsec, chatMTypes, timeout);
        }

        public bool GetP2PChat(HistoryMessageDelegate callback, long peerUid, bool desc, int count, long beginMsec = 0, long endMsec = 0, long lastId = 0, int timeout = 0)
        {
            return GetP2PMessage(callback, peerUid, desc, count, beginMsec, endMsec, lastId, chatMTypes, timeout);
        }

        public bool GetP2PChatByMessageId(HistoryMessageDelegate callback, long peerUid, bool desc, int count, long messageId, long beginMsec = 0, long endMsec = 0, int timeout = 0)
        {
            return GetP2PMessageByMessageId(callback, peerUid, desc, count, messageId, beginMsec, endMsec, chatMTypes, timeout);
        }

        //===========================[ Unread Chat ]=========================//
        //-- Action<List<p2p_uid>, List<groupId>, errorCode>
        public bool GetUnread(Action<List<long>, List<long>, int> callback, bool clear = false, int timeout = 0)
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

            Quest quest = new Quest("getunread");
            quest.Param("clear", clear);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                List<long> p2pList = null;
                List<long> groupList = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        p2pList = WantLongList(answer, "p2p");
                        groupList = WantLongList(answer, "group");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(p2pList, groupList, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        public bool GetUnread(Action<List<long>, List<long>, long, int> callback, bool clear = false, bool gettime = false,  int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(null, null, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("getunread");
            quest.Param("clear", clear);
            quest.Param("gettime", gettime);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                List<long> p2pList = null;
                List<long> groupList = null;
                long logoutTime = 0;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        p2pList = WantLongList(answer, "p2p");
                        groupList = WantLongList(answer, "group");
                        logoutTime = answer.Get<long>("logouttime", 0);
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(p2pList, groupList, logoutTime, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, null, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Clear Unread ]=========================//
        public bool ClearUnread(DoneDelegate callback, int timeout = 0)
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

            Quest quest = new Quest("cleanunread");
            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get P2P Unread ]=========================//
        //-- Action<Dictionary<peerUid, unreadCount>, errorCode>
        public bool GetP2PUnread(Action<Dictionary<long, int>, int> callback, HashSet<long> uids, HashSet<byte> mTypes = null, int timeout = 0)
        {
            return GetP2PUnread(callback, uids, 0, mTypes, timeout);
        }

        public bool GetP2PUnread(Action<Dictionary<long, int>, int> callback, HashSet<long> uids, long startTime, HashSet<byte> mTypes = null, int timeout = 0)
        {
            return GetP2PUnread((Dictionary<long, int> unreadDictionary, Dictionary<long, long> _, int errorCode) => { callback(unreadDictionary, errorCode); }, uids, startTime, mTypes, timeout);
        }

        //-- Action<Dictionary<peerUid, unreadCount>, Dictionary<peerUid, lastUnreadTimestamp>, errorCode>
        public bool GetP2PUnread(Action<Dictionary<long, int>, Dictionary<long, long>, int> callback, HashSet<long> uids, HashSet<byte> mTypes = null, int timeout = 0)
        {
            return GetP2PUnread(callback, uids, 0, mTypes, timeout);
        }

        public bool GetP2PUnread(Action<Dictionary<long, int>, Dictionary<long, long>, int> callback, HashSet<long> uids, long startTime, HashSet<byte> mTypes = null, int timeout = 0)
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

            Quest quest = new Quest("getp2punread");
            quest.Param("uids", uids);

            if (startTime > 0)
                quest.Param("mtime", startTime);

            if (mTypes != null && mTypes.Count > 0)
                quest.Param("mtypes", mTypes);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                Dictionary<long, int> unreadDictionary = null;
                Dictionary<long, long> lastUnreadTimestampDictionary = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        unreadDictionary = WantLongIntDictionary(answer, "p2p");
                        lastUnreadTimestampDictionary = WantLongLongDictionary(answer, "ltime");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }

                callback(unreadDictionary, lastUnreadTimestampDictionary, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get Group Unread ]=========================//
        //-- Action<Dictionary<groupId, unreadCount>, errorCode>
        public bool GetGroupUnread(Action<Dictionary<long, int>, int> callback, HashSet<long> groupIds, HashSet<byte> mTypes = null, int timeout = 0)
        {
            return GetGroupUnread(callback, groupIds, 0, mTypes, timeout);
        }

        public bool GetGroupUnread(Action<Dictionary<long, int>, int> callback, HashSet<long> groupIds, long startTime, HashSet<byte> mTypes = null, int timeout = 0)
        {
            return GetGroupUnread((Dictionary<long, int> unreadDictionary, Dictionary<long, long> _, int errorCode) => { callback(unreadDictionary, errorCode); }, groupIds, startTime, mTypes, timeout);
        }

        //-- Action<Dictionary<groupId, unreadCount>, Dictionary<groupId, lastUnreadTimestamp>, errorCode>
        public bool GetGroupUnread(Action<Dictionary<long, int>, Dictionary<long, long>, int> callback, HashSet<long> groupIds, HashSet<byte> mTypes = null, int timeout = 0)
        {
            return GetGroupUnread(callback, groupIds, 0, mTypes, timeout);
        }

        public bool GetGroupUnread(Action<Dictionary<long, int>, Dictionary<long, long>, int> callback, HashSet<long> groupIds, long startTime, HashSet<byte> mTypes = null, int timeout = 0)
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

            Quest quest = new Quest("getgroupunread");
            quest.Param("gids", groupIds);

            if (startTime > 0)
                quest.Param("mtime", startTime);

            if (mTypes != null && mTypes.Count > 0)
                quest.Param("mtypes", mTypes);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                Dictionary<long, int> unreadDictionary = null;
                Dictionary<long, long> lastUnreadTimestampDictionary = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        unreadDictionary = WantLongIntDictionary(answer, "group");
                        lastUnreadTimestampDictionary = WantLongLongDictionary(answer, "ltime");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }

                callback(unreadDictionary, lastUnreadTimestampDictionary, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Get Session ]=========================//
        //-- Action<List<p2p_uid>, List<groupId>, errorCode>
        public bool GetSession(Action<List<long>, List<long>, int> callback, int timeout = 0)
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

            Quest quest = new Quest("getsession");
            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                List<long> p2pList = null;
                List<long> groupList = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        p2pList = WantLongList(answer, "p2p");
                        groupList = WantLongList(answer, "group");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(p2pList, groupList, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Remove Session ]=========================//
        public bool RemoveSession(DoneDelegate callback, long toUid, bool oneway = false, int timeout = 0)
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

            Quest quest = new Quest("removesession");
            quest.Param("to", toUid);
            quest.Param("oneway", oneway);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Delete Chat ]=========================//
        //-- toId: peer uid, or groupId, or roomId
        public bool DeleteChat(DoneDelegate callback, long fromUid, long toId, long messageId, MessageCategory messageCategory, int timeout = 0)
        {
            return DeleteMessage(callback, fromUid, toId, messageId, (byte)messageCategory, timeout);
        }

        //===========================[ Get Chat ]=========================//
        //-- toId: peer uid, or groupId, or roomId
        public bool GetChat(Action<RetrievedMessage, int> callback, long fromUid, long toId, long messageId, MessageCategory messageCategory, int timeout = 0)
        {
            return GetMessage(callback, fromUid, toId, messageId, (byte)messageCategory, timeout);
        }

        //===========================[ Set Translated Languag ]=========================//
        public bool SetTranslatedLanguage(DoneDelegate callback, TranslateLanguage targetLanguage, int timeout = 0)
        {
            return SetTranslatedLanguage(callback, GetTranslatedLanguage(targetLanguage), timeout);
        }
        private bool SetTranslatedLanguage(DoneDelegate callback, string targetLanguage, int timeout = 0)
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

            Quest quest = new Quest("setlang");
            quest.Param("lang", targetLanguage);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Translate ]=========================//
        public enum TranslateType
        {
            Chat,
            Mail
        }

        public enum ProfanityType
        {
            Off,
            Stop,
            Censor
        }

        //-- Action<TranslatedInfo, errorCode>
        public bool Translate(Action<TranslatedInfo, int> callback, string text,
            string destinationLanguage, string sourceLanguage = "",
            TranslateType type = TranslateType.Chat, ProfanityType profanity = ProfanityType.Off,
            int timeout = 0)
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

            Quest quest = new Quest("translate");
            quest.Param("text", text);
            quest.Param("dst", destinationLanguage);

            if (sourceLanguage.Length > 0)
                quest.Param("src", sourceLanguage);

            if (type == TranslateType.Mail)
                quest.Param("type", "mail");
            else
                quest.Param("type", "chat");

            switch (profanity)
            {
                case ProfanityType.Stop: quest.Param("profanity", "stop"); break;
                case ProfanityType.Censor: quest.Param("profanity", "censor"); break;
                case ProfanityType.Off: quest.Param("profanity", "off"); break;
            }

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                TranslatedInfo tm = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        tm = new TranslatedInfo();
                        tm.sourceLanguage = answer.Want<string>("source");
                        tm.targetLanguage = answer.Want<string>("target");
                        tm.sourceText = answer.Want<string>("sourceText");
                        tm.targetText = answer.Want<string>("targetText");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(tm, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ SpeechToText ]=========================//
        //-------- url version ----------//
        //-- Action<string text, string language, errorCode>
        //public bool SpeechToText(Action<string, string, int> callback, string audioUrl, string language, string codec = null, int sampleRate = 0, int timeout = 120)
        //{
        //    WebSocketClient client = GetCoreClient();
        //    if (client == null)
        //    {
        //        if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
        //            RTMControlCenter.callbackQueue.PostAction(() =>
        //            {
        //                callback(string.Empty, string.Empty, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
        //            });

        //        return false;
        //    }

        //    Quest quest = new Quest("speech2text");
        //    quest.Param("audio", audioUrl);
        //    quest.Param("type", 1);
        //    quest.Param("lang", language);

        //    if (codec != null && codec.Length > 0)
        //        quest.Param("codec", codec);

        //    if (sampleRate > 0)
        //        quest.Param("srate", sampleRate);

        //    bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

        //        string text = "";
        //        string resultLanguage = "";

        //        if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
        //        {
        //            try
        //            {
        //                text = answer.Want<string>("text");
        //                resultLanguage = answer.Want<string>("lang");
        //            }
        //            catch (Exception)
        //            {
        //                errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
        //            }
        //        }
        //        callback(text, resultLanguage, errorCode);
        //    }, timeout);

        //    if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
        //        RTMControlCenter.callbackQueue.PostAction(() =>
        //        {
        //            callback(string.Empty, string.Empty, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
        //        });

        //    return asyncStarted;
        //}

        //public int SpeechToText(out string resultText, out string resultLanguage, string audioUrl, string language, string codec = null, int sampleRate = 0, int timeout = 120)
        //{
        //    resultText = "";
        //    resultLanguage = "";

        //    WebSocketClient client = GetCoreClient();
        //    if (client == null)
        //        return fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION;

        //    Quest quest = new Quest("speech2text");
        //    quest.Param("audio", audioUrl);
        //    quest.Param("type", 1);
        //    quest.Param("lang", language);

        //    if (codec != null && codec.Length > 0)
        //        quest.Param("codec", codec);

        //    if (sampleRate > 0)
        //        quest.Param("srate", sampleRate);

        //    Answer answer = client.SendQuest(quest, timeout);

        //    if (answer.IsException())
        //        return answer.ErrorCode();

        //    try
        //    {
        //        resultText = answer.Want<string>("text");
        //        resultLanguage = answer.Want<string>("lang");

        //        return fpnn.ErrorCode.FPNN_EC_OK;
        //    }
        //    catch (Exception)
        //    {
        //        return fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
        //    }
        //}

        //-------- binary version ----------//

        public bool SpeechToText(Action<string, string, int> callback, byte[] audioBinaryContent, string language, string codec = null, int sampleRate = 0, int timeout = 120)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(string.Empty, string.Empty, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("speech2text");
            quest.Param("audio", audioBinaryContent);
            quest.Param("type", 2);
            quest.Param("lang", language);

            if (codec != null && codec.Length > 0)
                quest.Param("codec", codec);

            if (sampleRate > 0)
                quest.Param("srate", sampleRate);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                string text = "";
                string resultLanguage = "";

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        text = answer.Want<string>("text");
                        resultLanguage = answer.Want<string>("lang");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(text, resultLanguage, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(string.Empty, string.Empty, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ SpeechTranslate ]=========================//
        public bool SpeechTranslate(Action<TranslatedInfo, int> callback, byte[] audioBinaryContent, string speechLanguage, string textLanguage = null, string codec = null, int sampleRate = 0, int timeout = 120)
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

            Quest quest = new Quest("speechtranslation");
            quest.Param("audio", audioBinaryContent);
            quest.Param("speechLanguageCode", speechLanguage);
            if (textLanguage != null && textLanguage.Length > 0)
                quest.Param("textLanguageCode", textLanguage);

            if (codec != null && codec.Length > 0)
                quest.Param("codec", codec);

            if (sampleRate > 0)
                quest.Param("srate", sampleRate);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {
                TranslatedInfo translatedInfo = null;

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        translatedInfo = new TranslatedInfo();
                        translatedInfo.sourceLanguage = answer.Want<string>("source");
                        translatedInfo.targetLanguage = answer.Want<string>("target");
                        translatedInfo.sourceText = answer.Want<string>("sourceText");
                        translatedInfo.targetText = answer.Want<string>("targetText");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(translatedInfo, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ TextCheck ]=========================//
        //-- Action<TextCheckResult result, errorCode>
        public bool TextCheck(Action<TextCheckResult, int> callback, string text, string strategyId = null, int timeout = 120)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(new TextCheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("tcheck");
            quest.Param("text", text);
            if (strategyId != null)
                quest.Param("strategyId", strategyId);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                TextCheckResult result = new TextCheckResult();

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        result.result = answer.Want<int>("result");
                        result.text = answer.Get<string>("text", null);
                        result.tags = GetIntList(answer, "tags");
                        result.wlist = GetStringList(answer, "wlist");
                        result.language = answer.Get<string>("language", null);
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
                    callback(new TextCheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ ImageCheck ]=========================//
        //-------- url version ----------//
        public bool ImageCheck(Action<CheckResult, int> callback, string imageUrl, string strategyId = null, int timeout = 120)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("icheck");
            quest.Param("image", imageUrl);
            quest.Param("type", 1);
            if (strategyId != null)
                quest.Param("strategyId", strategyId);


            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                CheckResult result = new CheckResult();

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        result.result = answer.Want<int>("result");
                        result.tags = GetIntList(answer, "tags");
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
                    callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //-------- binary version ----------//
        public bool ImageCheck(Action<CheckResult, int> callback, byte[] imageContent, string strategyId = null, int timeout = 120)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("icheck");
            quest.Param("image", imageContent);
            quest.Param("type", 2);
            if (strategyId != null)
                quest.Param("strategyId", strategyId);


            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                CheckResult result = new CheckResult();

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        result.result = answer.Want<int>("result");
                        result.tags = GetIntList(answer, "tags");
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
                    callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ AudioCheck ]=========================//

        //-------- url version ----------//
        public bool AudioCheck(Action<CheckResult, int> callback, string audioUrl, string language, string codec = null, int sampleRate = 0, string strategyId = null, int timeout = 120)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("acheck");
            quest.Param("audio", audioUrl);
            quest.Param("type", 1);
            quest.Param("lang", language);
            if (strategyId != null)
                quest.Param("strategyId", strategyId);

            if (codec != null && codec.Length > 0)
                quest.Param("codec", codec);

            if (sampleRate > 0)
                quest.Param("srate", sampleRate);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                CheckResult result = new CheckResult();

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        result.result = answer.Want<int>("result");
                        result.tags = GetIntList(answer, "tags");
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
                    callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //-------- binary version ----------//
        public bool AudioCheck(Action<CheckResult, int> callback, byte[] audioContent, string language, string codec = null, int sampleRate = 0, string strategyId = null, int timeout = 120)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("acheck");
            quest.Param("audio", audioContent);
            quest.Param("type", 2);
            quest.Param("lang", language);
            if (strategyId != null)
                quest.Param("strategyId", strategyId);

            if (codec != null && codec.Length > 0)
                quest.Param("codec", codec);

            if (sampleRate > 0)
                quest.Param("srate", sampleRate);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                CheckResult result = new CheckResult();

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        result.result = answer.Want<int>("result");
                        result.tags = GetIntList(answer, "tags");
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
                    callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ VideoCheck ]=========================//

        //-------- url version ----------//
        public bool VideoCheck(Action<CheckResult, int> callback, string videoUrl, string videoName, string strategyId = null, int timeout = 120)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("vcheck");
            quest.Param("video", videoUrl);
            quest.Param("type", 1);
            quest.Param("videoName", videoName);
            if (strategyId != null)
                quest.Param("strategyId", strategyId);


            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                CheckResult result = new CheckResult();

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        result.result = answer.Want<int>("result");
                        result.tags = GetIntList(answer, "tags");
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
                    callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //-------- binary version ----------//
        public bool VideoCheck(Action<CheckResult, int> callback, byte[] videoContent, string videoName, string strategyId = null, int timeout = 120)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("vcheck");
            quest.Param("video", videoContent);
            quest.Param("type", 2);
            quest.Param("videoName", videoName);
            if (strategyId != null)
                quest.Param("strategyId", strategyId);


            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) =>
            {

                CheckResult result = new CheckResult();

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        result.result = answer.Want<int>("result");
                        result.tags = GetIntList(answer, "tags");
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
                    callback(new CheckResult(), fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }
    }
}
