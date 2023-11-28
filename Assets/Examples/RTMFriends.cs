using System.Collections.Generic;
using UnityEngine;
using com.fpnn.rtm;

class Friends : Main.ITestCase
{
    private RTMClient client;

    public void Start(string endpoint, long pid, long uid, string token)
    {
        client = RTMClient.getInstance(endpoint, pid, uid, new example.common.RTMExampleQuestProcessor());

        client.Login((long projectId, long uid, bool ok, int errorCode) =>
        {
            if (ok)
            {
                AddFriends(client, new HashSet<long>() { 123456, 234567, 345678, 456789 });
                
                GetFriends(client);
                
                DeleteFriends(client, new HashSet<long>() { 234567, 345678 });
                
                System.Threading.Thread.Sleep(2000);   //-- Wait for server sync action.
                
                GetFriends(client);
                
                //-- Blacklist
                AddBlacklist(client, new HashSet<long>() { 123456, 234567, 345678, 456789 });
                
                GetBlacklist(client);
                
                DeleteBlacklist(client, new HashSet<long>() { 234567, 345678 });
                
                GetBlacklist(client);
                
                DeleteBlacklist(client, new HashSet<long>() { 123456, 234567, 345678, 456789 });
                
                GetBlacklist(client);
                
                Debug.Log("Demo completed.");
            }
        }, token);
    }

    public void Stop() { }


    //------------------------[ Friend Operations ]-------------------------//
    static void AddFriends(RTMClient client, HashSet<long> uids)
    {
        client.AddFriends((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Add friends in sync failed, error code is " + errorCode);
            else
                Debug.Log("Add friends in sync success");            
        }, uids);
    }

    static void DeleteFriends(RTMClient client, HashSet<long> uids)
    {
        client.DeleteFriends((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Delete friends in sync failed, error code is " + errorCode);
            else
                Debug.Log("Delete friends in sync success");            
        }, uids);
    }

    static void GetFriends(RTMClient client)
    {
        client.GetFriends((HashSet<long> uids, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get friends in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get friends in sync success");
                foreach (long uid in uids)
                    Debug.Log("-- Friend uid: " + uid);
            }            
        });
    }

    //------------------------[ Blacklist Operations ]-------------------------//
    static void AddBlacklist(RTMClient client, HashSet<long> uids)
    {
        client.AddBlacklist((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Add users to blacklist in sync failed, error code is " + errorCode);
            else
                Debug.Log("Add users to blacklist in sync success");            
        }, uids);
    }

    static void DeleteBlacklist(RTMClient client, HashSet<long> uids)
    {
        client.DeleteBlacklist((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Delete from blacklist in sync failed, error code is " + errorCode);
            else
                Debug.Log("Delete from blacklist in sync success");            
        }, uids);
    }

    static void GetBlacklist(RTMClient client)
    {
        client.GetBlacklist((HashSet<long> uids, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get blacklist in sync failed, error code is " + errorCode);
            else
            {
                Debug.Log("Get blacklist in sync success");
                foreach (long uid in uids)
                    Debug.Log("-- blocked uid: " + uid);
            }            
        });
    }
}