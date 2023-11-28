using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using com.fpnn.rtm;

class Groups : Main.ITestCase
{
    private static long groupId = 223344;

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
                    GetGroupInfos(client, groupId);
          
                    GetGroupsPublicInfo(client, new HashSet<long>() { 223344, 334455, 445566, 667788, 778899 });
          
                    Debug.Log("============== Demo completed ================");
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
                Debug.Log("======== get group members =========");
                GetGroupMembers(client, groupId);

                Debug.Log("======== add group members =========");
                AddGroupMembers(client, groupId, new HashSet<long>() { 99688868, 99688878, 99688888 });

                System.Threading.Thread.Sleep(1500); //-- Wait for server sync action.

                Debug.Log("======== get group members =========");
                GetGroupMembers(client, groupId);

                Debug.Log("======== delete group members =========");
                DeleteGroupMembers(client, groupId, new HashSet<long>() { 99688878 });

                System.Threading.Thread.Sleep(1500); //-- Wait for server sync action.

                Debug.Log("======== get group members =========");
                GetGroupMembers(client, groupId);
                GetGroupMembersAndOnlineMembers(client, groupId);

                Debug.Log("======== get group member count =========");
                GetGroupMemberCount(client, groupId);
                GetGroupMemberCountWithOnlineMemberCount(client, groupId);


                Debug.Log("======== get self groups =========");
                GetSelfGroups(client);


                Debug.Log("======== set group infos =========");

                SetGroupInfos(client, groupId, "This is public info", "This is private info");
                GetGroupInfos(client, groupId);

                Debug.Log("======== change group infos =========");

                SetGroupInfos(client, groupId, "", "This is private info");
                GetGroupInfos(client, groupId);

                Debug.Log("======== change group infos =========");

                SetGroupInfos(client, groupId, "This is public info", "");
                GetGroupInfos(client, groupId);

                Debug.Log("======== only change the private infos =========");

                SetGroupInfos(client, groupId, null, "balabala");
                GetGroupInfos(client, groupId);

                SetGroupInfos(client, groupId, "This is public info", "This is private info");
                
                client.AsyncClose();
                System.Threading.Thread.Sleep(1500); //-- Wait for server sync action.
            }
        }, token);
    }

    public void Stop() { }

    static void AddGroupMembers(RTMClient client, long groupId, HashSet<long> uids)
    {
        client.AddGroupMembers((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Add group members in sync failed, error code is " + errorCode);
            else
                Debug.Log("Add group members in sync successed.");            
        }, groupId, uids);
    }

    static void DeleteGroupMembers(RTMClient client, long groupId, HashSet<long> uids)
    {
        client.DeleteGroupMembers((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Delete group members in sync failed, error code is " + errorCode);
            else
                Debug.Log("Delete group members in sync successed.");            
        }, groupId, uids);
    }

    static void GetGroupMembers(RTMClient client, long groupId)
    {
        client.GetGroupMembers((HashSet<long> uids, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get group members in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get group members in sync successed, current has " + uids.Count + " members.");
                foreach (long uid in uids)
                    Debug.Log("-- member uid: " + uid);
            }            
        }, groupId);
    }

    static void GetGroupMembersAndOnlineMembers(RTMClient client, long groupId)
    {
        bool status = client.GetGroupMembers((HashSet<long> allUids2, HashSet<long> onlineUids2, int errorCode2) => {
            if (errorCode2 == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log($"Get group members with online members in async success, current has {allUids2.Count} members, {onlineUids2.Count} are online.");
                foreach (long uid in allUids2)
                    Debug.Log("-- [ALL] member uid: " + uid);
                foreach (long uid in onlineUids2)
                    Debug.Log("-- [Online] member uid: " + uid);
            }
            else
                Debug.Log($"Get group members with online members in async failed, error code is {errorCode2}.");
        }, groupId);
        if (!status)
            Debug.Log("Launch group members with online members in async failed.");

        System.Threading.Thread.Sleep(3 * 1000);
    }

    static void GetGroupMemberCount(RTMClient client, long groupId)
    {
        bool status = client.GetGroupCount((int memberCount2, int errorCode2) => {
            if (errorCode2 == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log($"Get group members count in async success, total {memberCount2}");
            }
            else
                Debug.Log($"Get group members count in async failed, error code is {errorCode2}.");
        }, groupId);
        if (!status)
            Debug.Log("Launch group members count in async failed.");

        System.Threading.Thread.Sleep(3 * 1000);
    }

    static void GetGroupMemberCountWithOnlineMemberCount(RTMClient client, long groupId)
    {
        bool status = client.GetGroupCount((int memberCount2, int onlineCount2, int errorCode2) => {
            if (errorCode2 == com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log($"Get group members count with online member count in async success, total {memberCount2}, online {onlineCount2}");
            }
            else
                Debug.Log($"Get group members count with online member count in async failed, error code is {errorCode2}.");
        }, groupId);
        if (!status)
            Debug.Log("Launch group members count with online member count in async failed.");

        System.Threading.Thread.Sleep(3 * 1000);
    }

    static void GetSelfGroups(RTMClient client)
    {
        client.GetUserGroups((HashSet<long> gids, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get user groups in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get user groups in sync successed, current I am in " + gids.Count + " groups.");
                foreach (long gid in gids)
                    Debug.Log("-- group id: " + gid);
            }            
        });
    }

    static void SetGroupInfos(RTMClient client, long groupId, string publicInfos, string privateInfos)
    {
        client.SetGroupInfo((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Set group infos in sync failed, error code is " + errorCode);
            else
                Debug.Log("Set group infos in sync successed.");            
        }, groupId, publicInfos, privateInfos);
    }

    static void GetGroupInfos(RTMClient client, long groupId)
    {
        client.GetGroupInfo((string publicInfos, string privateInfos, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get group infos in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get group infos in sync successed.");
                Debug.Log("Public info: " + (publicInfos ?? "null"));
                Debug.Log("Private info: " + (privateInfos ?? "null"));
            }            
        }, groupId);
    }

    static void GetGroupsPublicInfo(RTMClient client, HashSet<long> groupIds)
    {
        client.GetGroupsPublicInfo((Dictionary<long, string> publicInfos, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get groups' info in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get groups' info in sync success");
                foreach (var kvp in publicInfos)
                    Debug.Log("-- group id: " + kvp.Key + " info: [" + kvp.Value + "]");
            }            
        }, groupIds);
    }
}