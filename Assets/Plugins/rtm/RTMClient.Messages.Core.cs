﻿using System;
using com.fpnn.proto;

namespace com.fpnn.rtm
{
    public partial class RTMClient
    {
        //======================[ string message version ]================================//
        private bool InternalSendMessage(long uid, byte mtype, string message, string attrs, MessageIdDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendmsg");
            quest.Param("to", uid);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                //long mtime = 0;
                //if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                //    mtime = answer.Want<long>("mtime");

                callback(mid, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendMessage(long uid, byte mtype, string message, string attrs, SendMessageDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendmsg");
            quest.Param("to", uid);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                long mtime = 0;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                    mtime = answer.Want<long>("mtime");

                callback(mid, mtime, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendGroupMessage(long groupId, byte mtype, string message, string attrs, MessageIdDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendgroupmsg");
            quest.Param("gid", groupId);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                //long mtime = 0;
                //if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                //    mtime = answer.Want<long>("mtime");

                callback(mid, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendGroupMessage(long groupId, byte mtype, string message, string attrs, SendMessageDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendgroupmsg");
            quest.Param("gid", groupId);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                long mtime = 0;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                    mtime = answer.Want<long>("mtime");

                callback(mid, mtime, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendRoomMessage(long roomId, byte mtype, string message, string attrs, MessageIdDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendroommsg");
            quest.Param("rid", roomId);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                //long mtime = 0;
                //if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                //    mtime = answer.Want<long>("mtime");

                callback(mid, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendRoomMessage(long roomId, byte mtype, string message, string attrs, SendMessageDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendroommsg");
            quest.Param("rid", roomId);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                long mtime = 0;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                    mtime = answer.Want<long>("mtime");

                callback(mid, mtime, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //======================[ binary message version ]================================//
        private bool InternalSendMessage(long uid, byte mtype, byte[] message, string attrs, MessageIdDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendmsg");
            quest.Param("to", uid);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                //long mtime = 0;
                //if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                //    mtime = answer.Want<long>("mtime");

                callback(mid, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendMessage(long uid, byte mtype, byte[] message, string attrs, SendMessageDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendmsg");
            quest.Param("to", uid);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                long mtime = 0;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                    mtime = answer.Want<long>("mtime");

                callback(mid, mtime, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendGroupMessage(long groupId, byte mtype, byte[] message, string attrs, MessageIdDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendgroupmsg");
            quest.Param("gid", groupId);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                //long mtime = 0;
                //if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                //    mtime = answer.Want<long>("mtime");

                callback(mid, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendGroupMessage(long groupId, byte mtype, byte[] message, string attrs, SendMessageDelegate callback, long messageId, int timeout)
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

            long mid = messageId; 
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendgroupmsg");
            quest.Param("gid", groupId);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                long mtime = 0;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                    mtime = answer.Want<long>("mtime");

                callback(mid, mtime, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendRoomMessage(long roomId, byte mtype, byte[] message, string attrs, MessageIdDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendroommsg");
            quest.Param("rid", roomId);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                //long mtime = 0;
                //if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                //    mtime = answer.Want<long>("mtime");

                callback(mid, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool InternalSendRoomMessage(long roomId, byte mtype, byte[] message, string attrs, SendMessageDelegate callback, long messageId, int timeout)
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

            long mid = messageId;
            if (messageId == 0)
                mid = MidGenerator.Gen();

            Quest quest = new Quest("sendroommsg");
            quest.Param("rid", roomId);
            quest.Param("mid", mid);
            quest.Param("mtype", mtype);
            quest.Param("msg", message);
            quest.Param("attrs", attrs);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {
                long mtime = 0;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                    mtime = answer.Want<long>("mtime");

                callback(mid, mtime, errorCode);

            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }
    }
}
