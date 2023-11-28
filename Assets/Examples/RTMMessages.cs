using System.Threading;
using UnityEngine;
using com.fpnn.rtm;

class Messages: Main.ITestCase
{
    private static long peerUid = 12345678;
    private static long groupId = 223344;
    private static long roomId = 556677;
    private static byte customMType = 60;

    private static string textMessage = "Hello, RTM!";
    private static byte[] binaryMessage = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

    private RTMClient client;

    public void Start(string endpoint, long pid, long uid, string token)
    {
        client = RTMClient.getInstance(endpoint, pid, uid, new example.common.RTMExampleQuestProcessor());

        client.Login((long projectId, long uid, bool ok, int errorCode) =>
        {
            if (ok)
            {
                SendP2PMessageInAsync(client, peerUid, customMType);
                
                SendGroupMessageInAsync(client, groupId, customMType);
                        
                client.EnterRoom((int errorCode) =>
                {
                    if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                        Debug.Log("Enter room " + roomId + " in async failed.");
                    else
                    {
                        SendRoomMessageInAsync(client, roomId, customMType);
                    }
                }, roomId);
                
                Debug.Log("Running for receiving server pushed messsage if those are being demoed ...");
            }
        }, token);
    }

    public void Stop() { }

    static void SendP2PMessageInAsync(RTMClient client, long peerUid, byte mtype)
    {
        bool status = client.SendMessage((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send text message to user " + peerUid + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send text message to user " + peerUid + " in sync failed, errorCode is " + errorCode);
        }, peerUid, mtype, textMessage);

        if (!status)
            Debug.Log("Perpare send text message to user " + peerUid + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info

        status = client.SendMessage((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send binary message to user " + peerUid + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send binary message to user " + peerUid + " in sync failed, errorCode is " + errorCode);
        }, peerUid, mtype, binaryMessage);

        if (!status)
            Debug.Log("Perpare send binary message to user " + peerUid + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }

    static void SendGroupMessageInAsync(RTMClient client, long groupId, byte mtype)
    {
        bool status = client.SendGroupMessage((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send text message to group " + groupId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send text message to group " + groupId + " in sync failed, errorCode is " + errorCode);
        }, groupId, mtype, textMessage);

        if (!status)
            Debug.Log("Perpare send text message to group " + groupId + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info

        status = client.SendGroupMessage((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send binary message to group " + groupId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send binary message to group " + groupId + " in sync failed, errorCode is " + errorCode);
        }, groupId, mtype, binaryMessage);

        if (!status)
            Debug.Log("Perpare send binary message to group " + groupId + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }

    static void SendRoomMessageInAsync(RTMClient client, long roomId, byte mtype)
    {
        bool status = client.SendRoomMessage((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send text message to room " + roomId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send text message to room " + roomId + " in sync failed, errorCode is " + errorCode);
        }, roomId, mtype, textMessage);

        if (!status)
            Debug.Log("Perpare send text message to room " + roomId + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info

        status = client.SendRoomMessage((long messageId, int errorCode) => {
            if (errorCode == com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Send binary message to room " + roomId + " in sync successed, messageId is " + messageId);
            else
                Debug.Log("Send binary message to room " + roomId + " in sync failed, errorCode is " + errorCode);
        }, roomId, mtype, binaryMessage);

        if (!status)
            Debug.Log("Perpare send binary message to room " + roomId + " in async failed.");
        else
            Thread.Sleep(1000);     //-- Waiting callback desipay result info
    }
}