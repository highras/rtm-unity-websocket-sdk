using System.Threading;
using UnityEngine;
using com.fpnn.rtm;

class Files : Main.ITestCase
{
    private static long peerUid = 12345678;
    private static long groupId = 223344;
    private static long roomId = 556677;

    private static string filename = "demo.bin";
    private static byte[] fileContent = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

    private RTMClient client;

    public void Start(string endpoint, long pid, long uid, string token)
    {
        client = RTMClient.getInstance(endpoint, pid, uid, new example.common.RTMExampleQuestProcessor());

        client.Login((long projectId, long uid, bool ok, int errorCode) =>
        {
            if (ok)
            {
                SendP2PFileInAsync(client, peerUid, MessageType.NormalFile);
                SendP2PFileInSync(client, peerUid, MessageType.NormalFile);
                
                SendGroupFileInAsync(client, groupId, MessageType.NormalFile);
                SendGroupFileInSync(client, groupId, MessageType.NormalFile);
                
                client.EnterRoom((int errorCode) =>
                {
                    if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                    {
                        Debug.Log("Enter room " + roomId + " in sync failed.");
                    }
                    else
                    {
                        SendRoomFileInAsync(client, roomId, MessageType.NormalFile);
                        SendRoomFileInSync(client, roomId, MessageType.NormalFile);            
                    }
                }, roomId);
                Debug.Log("============== Demo completed ================");
            }
        }, token);
    }

    public void Stop() { }

    //--------------[ Send files Demo ]---------------------//
    static void SendP2PFileInAsync(RTMClient client, long peerUid, MessageType mtype)
    {
        bool status = client.SendFile((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send file to user " + peerUid + " in async successed, messageId is " + messageId);
            else
                Debug.Log("Send text message to user " + peerUid + " in async failed, errorCode is " + errorCode);
        }, peerUid, mtype, fileContent, filename);

        if (!status)
            Debug.Log("Perpare send file to user " + peerUid + " in async failed.");
        else
            Thread.Sleep(3000);     //-- Waiting callback desipay result info
    }

    static void SendP2PFileInSync(RTMClient client, long peerUid, MessageType mtype)
    {
        client.SendFile((long messageId, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send file to user " + peerUid + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send file to user " + peerUid + " in sync failed, error code " + errorCode);            
        }, peerUid, mtype, fileContent, filename);
    }

    static void SendGroupFileInAsync(RTMClient client, long groupId, MessageType mtype)
    {
        bool status = client.SendGroupFile((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send file to group " + groupId + " in async successed, messageId is " + messageId);
            else
                Debug.Log("Send file to group " + groupId + " in async failed, errorCode is " + errorCode);
        }, groupId, mtype, fileContent, filename);

        if (!status)
            Debug.Log("Perpare send file to group " + groupId + " in async failed.");
        else
            Thread.Sleep(3000);     //-- Waiting callback desipay result info
    }

    static void SendGroupFileInSync(RTMClient client, long groupId, MessageType mtype)
    {
        client.SendGroupFile((long messageId, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send file to group " + groupId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send file to group " + groupId + " in sync failed, error code " + errorCode);            
        }, groupId, mtype, fileContent, filename);
    }

    static void SendRoomFileInAsync(RTMClient client, long roomId, MessageType mtype)
    {
        bool status = client.SendRoomFile((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send file to room " + roomId + " in async successed, messageId is " + messageId);
            else
                Debug.Log("Send file to room " + roomId + " in async failed, errorCode is " + errorCode);
        }, roomId, mtype, fileContent, filename);

        if (!status)
            Debug.Log("Perpare send file to room " + roomId + " in async failed.");
        else
            Thread.Sleep(3000);     //-- Waiting callback desipay result info
    }

    static void SendRoomFileInSync(RTMClient client, long roomId, MessageType mtype)
    {
        client.SendRoomFile((long messageId, int errorCode) =>
        {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send file to room " + roomId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send file to room " + roomId + " in sync failed, error code " + errorCode);            
        }, roomId, mtype, fileContent, filename);
    }
}