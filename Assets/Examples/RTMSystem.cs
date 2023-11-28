using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using com.fpnn.rtm;
using example.common;

class RTMSystem : Main.ITestCase
{
    private RTMClient client;

    public void Start(string endpoint, long pid, long uid, string token)
    {
        client = RTMClient.getInstance(endpoint, pid, uid, new RTMExampleQuestProcessor());

        client.Login((long projectId, long uid, bool ok, int errorCode) =>
        {
            if (ok)
            {
                AddAttributesDemo(client);
                GetAttributesDemo(client);
                        
                GetDevicePushOption(client);
                AddDevicePushOption(client, MessageCategory.P2PMessage, 12345);
                AddDevicePushOption(client, MessageCategory.GroupMessage, 223344, new HashSet<byte>());
                
                AddDevicePushOption(client, MessageCategory.P2PMessage, 34567, null);
                AddDevicePushOption(client, MessageCategory.GroupMessage, 445566, new HashSet<byte>() { 23, 35, 56, 67, 78, 89 });
                
                GetDevicePushOption(client);
                
                RemoveDevicePushOption(client, MessageCategory.GroupMessage, 223344, new HashSet<byte>() { 23, 35, 56, 67, 78, 89 });
                RemoveDevicePushOption(client, MessageCategory.GroupMessage, 445566, new HashSet<byte>());
                
                GetDevicePushOption(client);
                
                RemoveDevicePushOption(client, MessageCategory.P2PMessage, 12345);
                RemoveDevicePushOption(client, MessageCategory.P2PMessage, 34567);
                RemoveDevicePushOption(client, MessageCategory.GroupMessage, 223344);
                RemoveDevicePushOption(client, MessageCategory.GroupMessage, 445566, new HashSet<byte>() { 23, 35, 56, 67, 78, 89 });
                
                GetDevicePushOption(client);
                
                Debug.Log("============== Demo completed ================");
            }
            else
            {
                Debug.Log("RTM login failed, error code: " + errorCode);
                client = null;
            }
        }, token, new Dictionary<string, string>() {
            { "attr1", "demo 123" },
            { "attr2", " demo 234" },
        });
    }

    public void Stop() { }

    static void AddAttributesDemo(RTMClient client)
    {
        client.AddAttributes((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Add attributes in sync failed.");
            else
                Debug.Log("Add attributes in sync success.");     
        }, new Dictionary<string, string>() {
            { "key1", "value1" },
            { "key2", "value2" }
        });
    }

    static void GetAttributesDemo(RTMClient client)
    {
        client.GetAttributes((Dictionary<string, string> attributes, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log("Get attributes in sync failed. error code " + errorCode);
                return;
            }

            Debug.Log("Attributes has " + attributes.Count + " items.");

            foreach (KeyValuePair<string, string> kvp in attributes)
                Debug.Log("Key " + kvp.Key  + ", value " + kvp.Value);            
        });
    }

    static void AddDevicePushOption(RTMClient client, MessageCategory messageCategory, long targetId, HashSet<byte> mTypes = null)
    {
        Debug.Log($"===== [ AddDevicePushOption ] =======");

        client.AddDevicePushOption((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Add device push option in sync failed.");
            else
                Debug.Log("Add device push option in sync success.");            
        }, messageCategory, targetId, mTypes);
    }

    static void RemoveDevicePushOption(RTMClient client, MessageCategory messageCategory, long targetId, HashSet<byte> mTypes = null)
    {
        Debug.Log($"===== [ RemoveDevicePushOption ] =======");

        client.RemoveDevicePushOption((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Remove device push option in sync failed.");
            else
                Debug.Log("Remove device push option in sync success.");            
        }, messageCategory, targetId, mTypes);
    }

    static void PrintDevicePushOption(string categroy, Dictionary<long, HashSet<byte>> optionDictionary)
    {
        Debug.Log($"===== {categroy} has {optionDictionary.Count} items. =======");
        foreach (KeyValuePair<long, HashSet<byte>> kvp in optionDictionary)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("ID: ").Append(kvp.Key).Append(", count: ").Append(kvp.Value.Count);
            if (kvp.Value.Count > 0)
            {
                sb.Append(": {");
                foreach (byte mType in kvp.Value)
                    sb.Append($" {mType},");

                sb.Append("}");
            }

            Debug.Log(sb);
        }
    }

    static void GetDevicePushOption(RTMClient client)
    {
        Debug.Log($"===== [ GetDevicePushOption ] =======");

        client.GetDevicePushOption((Dictionary<long, HashSet<byte>> p2pDictionary,
            Dictionary<long, HashSet<byte>> groupDictionary, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
            {
                Debug.Log($"Get device push option in sync failed. error code {errorCode}");
                return;
            }

            PrintDevicePushOption("P2P", p2pDictionary);
            PrintDevicePushOption("Group", groupDictionary);            
        });
    }
}