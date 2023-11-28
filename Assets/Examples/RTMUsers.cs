using System.Collections.Generic;
using UnityEngine;
using com.fpnn.rtm;

class Users : Main.ITestCase
{
    private RTMClient client;

    public void Start(string endpoint, long pid, long uid, string token)
    {
        var processor = new example.common.RTMExampleQuestProcessor();
        processor.SessionClosedCallback = (int errorCode) =>
        {
            Debug.Log("======== user relogin =========");

            client.Login((long projectId, long uid, bool ok, int errorCode) =>
            {
                if (ok)
                {
                    GetUserInfos(client);
                
                    GetUsersInfos(client, new HashSet<long>() { 99688848, 123456, 234567, 345678, 456789 });
                
                    Debug.Log("============== Demo completed ================");
                }
            }, token);
        };
        client = RTMClient.getInstance(endpoint, pid, uid, processor);
        
        client.Login((long projectId, long uid, bool ok, int errorCode) =>
        {
            if (ok)
            {
                Debug.Log("RTM login success.");
                GetOnlineUsers(client, new HashSet<long>() { 99688848, 123456, 234567, 345678, 456789 });
                
                SetUserInfos(client, "This is public info", "This is private info");
                GetUserInfos(client);
                
                Debug.Log("======== =========");
                
                SetUserInfos(client, "", "This is private info");
                GetUserInfos(client);
                
                Debug.Log("======== =========");
                
                SetUserInfos(client, "This is public info", "");
                GetUserInfos(client);
                
                Debug.Log("======== only change the private infos =========");
                
                SetUserInfos(client, null, "balabala");
                GetUserInfos(client);
                
                SetUserInfos(client, "This is public info", "This is private info");
                client.AsyncClose();
            }
            else
            {
                Debug.Log("RTM login failed, error code: " + errorCode);
                client = null;
            }
        }, token);
    }

    public void Stop() { }

    static void GetOnlineUsers(RTMClient client, HashSet<long> willCheckedUids)
    {
        client.GetOnlineUsers((HashSet<long> onlineUids, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get online users in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get online users in sync success");
                Debug.Log("Only " + onlineUids.Count + " user(s) online in total " + willCheckedUids.Count + " checked users");
                foreach (long uid in onlineUids)
                    Debug.Log("-- online uid: " + uid);
            }            
        }, willCheckedUids);
    }

    static void SetUserInfos(RTMClient client, string publicInfos, string privateInfos)
    {
        client.SetUserInfo((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Set user infos in sync failed, error code is " + errorCode);
            else
                Debug.Log("Set user infos in sync successed.");            
        }, publicInfos, privateInfos);
    }

    static void GetUserInfos(RTMClient client)
    {
        client.GetUserInfo((string publicInfos, string privateInfos, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get user infos in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get user infos in sync successed.");
                Debug.Log("Public info: " + (publicInfos ?? "null"));
                Debug.Log("Private info: " + (privateInfos ?? "null"));
            }            
        });
    }

    static void GetUsersInfos(RTMClient client, HashSet<long> uids)
    {
        client.GetUserPublicInfo((Dictionary<long, string> publicInfos, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get users' info in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get users' info in sync success");
                foreach (var kvp in publicInfos)
                    Debug.Log("-- uid: " + kvp.Key + " info: [" + kvp.Value + "]");
            }            
        }, uids);
    }
}