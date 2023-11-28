using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using com.fpnn.rtm;

class Rooms : Main.ITestCase
{
    private static long roomId = 556677;

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
                    Debug.Log("======== enter room =========");
                    client.EnterRoom((int errorCode) =>
                    {
                        if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                            Debug.Log("Enter room " + roomId + " in sync failed.");
                        else
                        {
                            Debug.Log("Enter room " + roomId + " in sync successed.");     
                                                        
                            GetRoomInfos(client, roomId);
                                        
                            GetRoomsPublicInfo(client, new HashSet<long>() { 556677, 778899, 445566, 334455, 1234 });
                                        
                            Debug.Log("======== get room members immediately =========");
                                        
                            GetRoomMemberCount(client, new HashSet<long>() { roomId, 778899, 445566, 334455, 1234 });
                            GetRoomMembers(client, roomId);
                                        
                            Debug.Log("============== Demo completed ================");
                        }
                    }, roomId);
                }
                else
                {
                    Debug.Log("RTM login failed, error code: " + errorCode);
                    client = null;
                }
            }, token);
        };
        client = RTMClient.getInstance(endpoint, pid, uid, processor);

        client.Login((long projectId, long uid, bool ok, int errorCode) =>
        {
            if (ok)
            {
                Debug.Log("======== enter room =========");
                client.EnterRoom((int errorCode) =>
                {
                    if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                        Debug.Log("Enter room " + roomId + " in sync failed.");
                    else
                    {
                        Debug.Log("Enter room " + roomId + " in sync successed.");     
                        Debug.Log("======== get self rooms =========");
                        GetSelfRooms(client);
                        Debug.Log("======== leave room =========");
                        client.LeaveRoom((int errorCode) =>
                        {
                            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                                Debug.Log("Leave room " + roomId + " in sync failed.");
                            else
                            {
                                Debug.Log("Leave room " + roomId + " in sync successed.");
                                Debug.Log("======== get self rooms =========");
                                GetSelfRooms(client);
                                Debug.Log("======== enter room =========");
                                client.EnterRoom((int errorCode) =>
                                {
                                    if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                                        Debug.Log("Enter room " + roomId + " in sync failed.");
                                    else
                                    {
                                        Debug.Log("Enter room " + roomId + " in sync successed.");     
                                        Debug.Log("======== set room infos =========");
                                                                
                                        SetRoomInfos(client, roomId, "This is public info", "This is private info");
                                        GetRoomInfos(client, roomId);
                                                                
                                        Debug.Log("======== change room infos =========");
                                                                
                                        SetRoomInfos(client, roomId, "", "This is private info");
                                        GetRoomInfos(client, roomId);
                                                                
                                        Debug.Log("======== change room infos =========");
                                                                
                                        SetRoomInfos(client, roomId, "This is public info", "");
                                        GetRoomInfos(client, roomId);
                                                                
                                        Debug.Log("======== only change the private infos =========");
                                                                
                                        SetRoomInfos(client, roomId, null, "balabala");
                                        GetRoomInfos(client, roomId);
                                                                
                                        SetRoomInfos(client, roomId, "This is public info", "This is private info");
                                        System.Threading.Thread.Sleep(1000);
                                        client.AsyncClose();
                                    }
                                }, roomId);
                            }
                        }, roomId);
                    }
                }, roomId);
            }
        }, token);
    }

    public void Stop() { }

    static void GetSelfRooms(RTMClient client)
    {
        client.GetUserRooms((HashSet<long> rids, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get user rooms in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get user rooms in sync successed, current I am in " + rids.Count + " rooms.");
                foreach (long rid in rids)
                    Debug.Log("-- room id: " + rid);
            }            
        });
    }

    static void SetRoomInfos(RTMClient client, long roomId, string publicInfos, string privateInfos)
    {
        client.SetRoomInfo((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Set room infos in sync failed, error code is " + errorCode);
            else
                Debug.Log("Set room infos in sync successed.");            
        }, roomId, publicInfos, privateInfos);
    }

    static void GetRoomInfos(RTMClient client, long roomId)
    {
        client.GetRoomInfo((string publicInfos, string privateInfos, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get room infos in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get room infos in sync successed.");
                Debug.Log("Public info: " + (publicInfos ?? "null"));
                Debug.Log("Private info: " + (privateInfos ?? "null"));
            }            
        }, roomId);
    }

    static void GetRoomsPublicInfo(RTMClient client, HashSet<long> roomIds)
    {
        client.GetRoomsPublicInfo((Dictionary<long, string> publicInfos, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get rooms' info in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get rooms' info in sync success");
                foreach (var kvp in publicInfos)
                    Debug.Log("-- room id: " + kvp.Key + " info: [" + kvp.Value + "]");
            }            
        }, roomIds);
    }

    static void GetRoomMembers(RTMClient client, long roomId)
    {
        bool status = client.GetRoomMembers((HashSet<long> uids2, int errorCode2) => {
            if (errorCode2 == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Get room members in async success");
                foreach (var uid in uids2)
                    Debug.Log($"-- room member: {uid}");
            }
            else
                Debug.Log($"Get room members in async failed, error code is {errorCode2}.");
        }, roomId);
        if (!status)
            Debug.Log("Launch room members in async failed.");

        Thread.Sleep(3 * 1000);
    }

    static void GetRoomMemberCount(RTMClient client, HashSet<long> roomIds)
    {
        bool status = client.GetRoomMemberCount((Dictionary<long, int> counts2, int errorCode2) => {
            if (errorCode2 == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Get room members count in async success");
                foreach (var kvp in counts2)
                    Debug.Log($"-- room: {kvp.Key}, count: {kvp.Value}");
            }
            else
                Debug.Log($"Get room members count in async failed, error code is {errorCode2}.");
        }, roomIds);
        if (!status)
            Debug.Log("Launch room members count in async failed.");

        Thread.Sleep(3 * 1000);
    }
}