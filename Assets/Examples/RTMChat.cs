using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using com.fpnn.rtm;

class Chat : Main.ITestCase
{
    private static long peerUid = 12345678;
    private static long groupId = 223344;
    private static long roomId = 556677;

    private static string textMessage = "Hello, RTM!";

    private RTMClient client;

    public void Start(string endpoint, long pid, long uid, string token)
    {
        client = RTMClient.getInstance(endpoint, pid, uid, new example.common.RTMExampleQuestProcessor());

        client.Login((long projectId, long uid, bool ok, int errorCode) =>
        {
            if (ok)
            {
                SendP2PChatInAsync(client, peerUid);
                SendP2PCmdInAsync(client, peerUid);
                
                SendGroupChatInAsync(client, groupId);
                SendGroupCmdInAsync(client, groupId);
                
                client.EnterRoom((int errorCode) =>
                {
                    if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                        Debug.Log("Enter room " + roomId + " in sync failed.");
                    else
                    {
                        SendRoomChatInAsync(client, roomId);
                        SendRoomCmdInAsync(client, roomId);
                    }
                }, roomId);
                        
                GetP2PUnreadInAsync(client, new HashSet<long> { peerUid, peerUid+1, peerUid+2 }, new HashSet<byte>{ 30, 40, 50 });
                GetP2PUnreadInAsyncPlus(client, new HashSet<long> { peerUid, peerUid + 1, peerUid + 2 }, new HashSet<byte> { 30, 40, 50 });
                GetGroupUnreadInAsync(client, new HashSet<long> { groupId }, new HashSet<byte> { 30, 40, 50 });
                GetGroupUnreadInAsyncPlus(client, new HashSet<long> { groupId }, new HashSet<byte> { 30, 40, 50 });
                        
                TextAudit(client, "sdaada asdasd asdasd asdas dds");
                TextAudit(client, "ssds 他妈的， 去你妈逼，操你妈的");
                TextAudit(client, "sdaada fuck you mother dds");
                
                ImageAudit(client, "https://box.bdimg.com/static/fisp_static/common/img/searchbox/logo_news_276_88_1f9876a.png");
                AudioAudit(client, "https://opus-codec.org/static/examples/samples/speech_orig.wav");
                VideoAudit(client, "http://vfx.mtime.cn/Video/2019/02/04/mp4/190204084208765161.mp4");
                        
                Debug.Log("Running for receiving server pushed chat & cmd &c audio if those are being demoed ...");
            }
        }, token);
    }

    public void Stop() { }

    //------------------------[ Chat Demo ]-------------------------//
    static void SendP2PChatInAsync(RTMClient client, long peerUid)
    {
        bool status = client.SendChat((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send chat message to user " + peerUid + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send chat message to user " + peerUid + " in sync failed, errorCode is " + errorCode);
        }, peerUid, textMessage);

        if (!status)
            Debug.Log("Perpare send chat message to user " + peerUid + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }

    static void SendGroupChatInAsync(RTMClient client, long groupId)
    {
        bool status = client.SendGroupChat((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send chat message to group " + groupId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send chat message to group " + groupId + " in sync failed, errorCode is " + errorCode);
        }, groupId, textMessage);

        if (!status)
            Debug.Log("Perpare send chat message to group " + groupId + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }

    static void SendRoomChatInAsync(RTMClient client, long roomId)
    {
        bool status = client.SendRoomChat((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send chat message to room " + roomId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send chat message to room " + roomId + " in sync failed, errorCode is " + errorCode);
        }, roomId, textMessage);

        if (!status)
            Debug.Log("Perpare send chat message to room " + roomId + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }

    //------------------------[ Cmd Demo ]-------------------------//
    static void SendP2PCmdInAsync(RTMClient client, long peerUid)
    {
        bool status = client.SendCmd((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send cmd message to user " + peerUid + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send cmd message to user " + peerUid + " in sync failed, errorCode is " + errorCode);
        }, peerUid, textMessage);

        if (!status)
            Debug.Log("Perpare send cmd message to user " + peerUid + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }

    static void SendGroupCmdInAsync(RTMClient client, long groupId)
    {
        bool status = client.SendGroupCmd((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send cmd message to group " + groupId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send cmd message to group " + groupId + " in sync failed, errorCode is " + errorCode);
        }, groupId, textMessage);

        if (!status)
            Debug.Log("Perpare send cmd message to group " + groupId + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }

    static void SendRoomCmdInAsync(RTMClient client, long roomId)
    {
        bool status = client.SendRoomCmd((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send cmd message to room " + roomId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send cmd message to room " + roomId + " in sync failed, errorCode is " + errorCode);
        }, roomId, textMessage);

        if (!status)
            Debug.Log("Perpare cmd chat message to room " + roomId + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }

    //------------------------[ Get P2P & Group Unread ]-------------------------//
    static void GetP2PUnreadInAsync(RTMClient client, HashSet<long> uids, HashSet<byte> mtypes)
    {
        client.GetP2PUnread((Dictionary<long, int> unreadMap, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Fetch P2P unread in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Debug.Log($" -- peer: {kvp.Key}, unread message {kvp.Value}");
                }
            }
            else
                Debug.Log($"Fetch P2P unread in sync failed. Error code {errorCode}");
        }, uids);
        
        client.GetP2PUnread((Dictionary<long, int> unreadMap, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Fetch P2P unread with mTypes in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Debug.Log($" -- peer: {kvp.Key}, unread message {kvp.Value}");
                }
            }
            else
                Debug.Log($"Fetch P2P unread with mTypes in sync failed. Error code {errorCode}");                
        } , uids, mtypes);
    }

    static void GetP2PUnreadInAsyncPlus(RTMClient client, HashSet<long> uids, HashSet<byte> mtypes)
    {
        client.GetP2PUnread((Dictionary<long, int> unreadMap, Dictionary<long, long> lastUnreadTimestampDictionary,
            int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Fetch P2P unread in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Debug.Log($" -- peer: {kvp.Key}, unread message {kvp.Value}");
                }
                foreach (KeyValuePair<long, long> kvp in lastUnreadTimestampDictionary)
                {
                    Debug.Log($" -- peer: {kvp.Key}, last unread message UTC in msec is {kvp.Value}");
                }
            }
            else
                Debug.Log($"Fetch P2P unread in sync failed. Error code {errorCode}");    
        } , uids);

        client.GetP2PUnread((Dictionary<long, int> unreadMap, Dictionary<long, long> lastUnreadTimestampDictionary,
            int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Fetch P2P unread with mTypes in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Debug.Log($" -- peer: {kvp.Key}, unread message {kvp.Value}");
                }
                foreach (KeyValuePair<long, long> kvp in lastUnreadTimestampDictionary)
                {
                    Debug.Log($" -- peer: {kvp.Key}, last unread message UTC in msec is {kvp.Value}");
                }
            }
            else
                Debug.Log($"Fetch P2P unread with mTypes in sync failed. Error code {errorCode}");     
        }, uids, mtypes);
    }

    static void GetGroupUnreadInAsync(RTMClient client, HashSet<long> groupIds, HashSet<byte> mtypes)
    {
        client.GetGroupUnread((Dictionary<long, int> unreadMap, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Fetch group unread in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Debug.Log($" -- group: {kvp.Key}, unread message {kvp.Value}");
                }
            }
            else
                Debug.Log($"Fetch group unread in sync failed. Error code {errorCode}");
        }, groupIds);

        
        client.GetGroupUnread((Dictionary<long, int> unreadMap, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Fetch group unread with mTypes in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Debug.Log($" -- group: {kvp.Key}, unread message {kvp.Value}");
                }
            }
            else
                Debug.Log($"Fetch group unread with mTypes in sync failed. Error code {errorCode}");     
        }, groupIds, mtypes);
    }

    static void GetGroupUnreadInAsyncPlus(RTMClient client, HashSet<long> groupIds, HashSet<byte> mtypes)
    {
        client.GetGroupUnread((Dictionary<long, int> unreadMap, Dictionary<long, long> lastUnreadTimestampDictionary, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Fetch group unread in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Debug.Log($" -- group: {kvp.Key}, unread message {kvp.Value}");
                }
                foreach (KeyValuePair<long, long> kvp in lastUnreadTimestampDictionary)
                {
                    Debug.Log($" -- group: {kvp.Key}, last unread message UTC in msec is {kvp.Value}");
                }
            }
            else
                Debug.Log($"Fetch group unread in sync failed. Error code {errorCode}");                
        }, groupIds);



        client.GetGroupUnread((Dictionary<long, int> unreadMap, Dictionary<long, long> lastUnreadTimestampDictionary, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Fetch group unread with mTypes in sync successful.");
                foreach (KeyValuePair<long, int> kvp in unreadMap)
                {
                    Debug.Log($" -- group: {kvp.Key}, unread message {kvp.Value}");
                }
                foreach (KeyValuePair<long, long> kvp in lastUnreadTimestampDictionary)
                {
                    Debug.Log($" -- group: {kvp.Key}, last unread message UTC in msec is {kvp.Value}");
                }
            }
            else
                Debug.Log($"Fetch group unread with mTypes in sync failed. Error code {errorCode}");                
        }, groupIds, mtypes);
    }

    //------------------------[ Text Image Audio Vedio Audit ]-------------------------//
    static void TextAudit(RTMClient client, string text)
    {
        client.TextCheck((TextCheckResult result, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("TextCheck in sync failed, error " + errorCode);
            else
            {
                Debug.Log("TextCheck in sync successed");
                Debug.Log("  -- result " + result.result);
                Debug.Log("  -- text " + result.text);
       
                if (result.tags != null)
                    Debug.Log("  -- tags.Count " + result.tags.Count);
                if (result.wlist != null)
                    Debug.Log("  -- wlist.Count " + result.wlist.Count);
            }     
        }, text);
    }

    static void ImageAudit(RTMClient client, string url)
    {
        client.ImageCheck((CheckResult result, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("ImageCheck in sync failed, error " + errorCode);
            else
            {
                Debug.Log("ImageCheck in sync successed");
                Debug.Log("  -- result " + result.result);
                if (result.tags != null)
                    Debug.Log("  -- tags.Count " + result.tags.Count);
            }            
        },url);
    }

    static void AudioAudit(RTMClient client, string url)
    {
        client.AudioCheck((CheckResult result, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("AudioCheck in sync failed, error " + errorCode);
            else
            {
                Debug.Log("AudioCheck in sync successed");
                Debug.Log("  -- result " + result.result);
                if (result.tags != null)
                    Debug.Log("  -- tags.Count " + result.tags.Count);
            }            
        }, url, "zh-CN");
    }

    static void VideoAudit(RTMClient client, string url)
    {
        client.VideoCheck((CheckResult result, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("VideoCheck in sync failed, error " + errorCode);
            else
            {
                Debug.Log("VideoCheck in sync successed");
                Debug.Log("  -- result " + result.result);
                if (result.tags != null)
                    Debug.Log("  -- tags.Count " + result.tags.Count);
            }            
        }, url, "testVideo");
    }
}