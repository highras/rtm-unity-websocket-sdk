using UnityEngine;
using com.fpnn.rtm;

class Data : Main.ITestCase
{
    private RTMClient client;

    public void Start(string endpoint, long pid, long uid, string token)
    {
        var processor = new example.common.RTMExampleQuestProcessor();
        processor.SessionClosedCallback = (int errorCode) =>
        {
            Debug.Log("=========== User relogin ===========");

            client.Login((long projectId, long uid, bool ok, int errorCode) =>
            {
                if (ok)
                {
                    Debug.Log("=========== Begin get user data after relogin ===========");

                    GetData(client, "key 1");
                    GetData(client, "key 2");

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
                Debug.Log("RTM login success.");
                Debug.Log("=========== Begin set user data ===========");

                SetData(client, "key 1", "value 1");
                SetData(client, "key 2", "value 2");

                Debug.Log("=========== Begin get user data ===========");

                GetData(client, "key 1");
                GetData(client, "key 2");

                Debug.Log("=========== Begin delete one of user data ===========");

                DeleteData(client, "key 2");

                Debug.Log("=========== Begin get user data after delete action ===========");

                GetData(client, "key 1");
                GetData(client, "key 2");

                Debug.Log("=========== User logout ===========");

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

    static void SetData(RTMClient client, string key, string value)
    {
        client.DataSet((errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Set user data with key " + key + " in sync failed.");
            else
                Debug.Log("Set user data with key " + key + " in sync success.");
        }, key, value);
    }

    static void GetData(RTMClient client, string key)
    {
        client.DataGet((string value, int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Get user data with key " + key + " in sync failed, error code is " + errorCode);
            else
                Debug.Log("Get user data with key " + key + " in sync success, value is " + (value ?? "null"));
        }, key);
    }

    static void DeleteData(RTMClient client, string key)
    {
        client.DataDelete((int errorCode) =>
        {
            if (errorCode != com.fpnn.ErrorCode.FPNN_EC_OK)
                Debug.Log("Delete user data with key " + key + " in sync failed.");
            else
                Debug.Log("Delete user data with key " + key + " in sync success.");
        }, key);
    }
}