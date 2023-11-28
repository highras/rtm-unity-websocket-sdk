using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static com.fpnn.rtm.RTMClient;

#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif

namespace com.fpnn.rtm
{
    public class RTMControlCenter : MonoBehaviour
    {
        private static Dictionary<Int64, RTMClient> rtmClients = new Dictionary<long, RTMClient>();
        private static Dictionary<Int64, Dictionary<Int64, RTMClient>> pidUidClients = new Dictionary<Int64, Dictionary<Int64, RTMClient>>();
        private static Dictionary<RTMClient, Int64> reloginClients = new Dictionary<RTMClient, Int64>();

        private static Dictionary<string, Dictionary<WebSocketClient, long>> fileClients = new Dictionary<string, Dictionary<WebSocketClient, long>>();

        private static bool routineRunning;
        private static GameObject rtmGameObject;
        public static RTMCallbackQueue callbackQueue;
        private static RTMControlCenter instance;
        private static readonly string objectName = "RTMCONTROLCENTER";

        static RTMControlCenter()
        {
            routineRunning = false;
        }
        
        public static RTMControlCenter Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject(objectName);
                    instance = go.AddComponent<RTMControlCenter>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        private void Start()
        {
            StartCoroutine(RoutineFunc());
        }

        //===========================[ Session Functions ]=========================//
        internal void RegisterSession(long uid, RTMClient client)
        {
            rtmClients.Add(uid, client);
        }

        internal void UnregisterSession(Int64 uid)
        {
            rtmClients.Remove(uid);
        }

        internal void CloseSession(Int64 uid)
        {
            RTMClient client = null;
            rtmClients.TryGetValue(uid, out client);

            if (client != null)
                client.AsyncClose();
        }

        internal ClientStatus GetClientStatus(Int64 uid)
        { 
            RTMClient client = null;
            rtmClients.TryGetValue(uid, out client);

            if (client != null)
                return client.Status;
            else
                return ClientStatus.Closed;
        }
        internal void AddClient(Int64 projectId, Int64 uid, RTMClient client)
        { 
            pidUidClients.TryGetValue(projectId, out Dictionary<Int64, RTMClient> clients);
            if (clients == null)
            {
                clients = new Dictionary<long, RTMClient>{ { uid, client } };
                pidUidClients.Add(projectId, clients);
            }
            else
            {
                clients.TryGetValue(uid, out RTMClient rtmClient);
                if (rtmClient == null)
                    clients.Add(uid, client);
                else
                    throw new Exception("duplicated RTMClient pid = " + projectId.ToString() + ", uid = " + uid.ToString());
            }
        }

        internal RTMClient FetchClient(Int64 projectId, Int64 uid)
        {
            RTMClient client = null;
            pidUidClients.TryGetValue(projectId, out Dictionary<Int64, RTMClient> clients);
            if (clients == null)
                return null;
            clients.TryGetValue(uid, out client);
            return client;
        }

        //===========================[ Relogin Functions ]=========================//
        internal void DelayRelogin(RTMClient client, long triggeredMs)
        {
            try
            {
                reloginClients.Add(client, triggeredMs);
            }
            catch (ArgumentException)
            {
                //-- Do nothing.
            }
        }

        private void ReloginCheck()
        {
            HashSet<RTMClient> clients = new HashSet<RTMClient>();
            long now = ClientEngine.GetCurrentMilliseconds();

            foreach (KeyValuePair<RTMClient, Int64> kvp in reloginClients)
            {
                if (kvp.Value <= now)
                    clients.Add(kvp.Key);
            }

            foreach (RTMClient client in clients)
                reloginClients.Remove(client);

            foreach (RTMClient client in clients)
            {
                callbackQueue.PostAction(() => {
                    if (client.CheckRelogin())
                        client.StartRelogin();
                    else
                    {
                        reloginClients.Add(client, now);
                    }
                });
            }
        }


        //===========================[ File Gate Client Functions ]=========================//
        internal void ActiveFileGateClient(string endpoint, WebSocketClient client)
        {
            if (fileClients.TryGetValue(endpoint, out Dictionary<WebSocketClient, long> clients))
            {
                if (clients.ContainsKey(client))
                    clients[client] = ClientEngine.GetCurrentSeconds();
                else
                    clients.Add(client, ClientEngine.GetCurrentSeconds());
            }
            else
            {
                clients = new Dictionary<WebSocketClient, long>
                {
                    { client, ClientEngine.GetCurrentSeconds() }
                };
                fileClients.Add(endpoint, clients);
            }
        }

        internal WebSocketClient FecthFileGateClient(string endpoint)
        {
            if (fileClients.TryGetValue(endpoint, out Dictionary<WebSocketClient, long> clients))
            {
                foreach (KeyValuePair<WebSocketClient, long> kvp in clients)
                    return kvp.Key;
            }

            return null;
        }

        private void CheckFileGateClients()
        {
            HashSet<string> emptyEndpoints = new HashSet<string>();

            long threshold = ClientEngine.GetCurrentSeconds() - RTMConfig.fileGateClientHoldingSeconds;

            foreach (KeyValuePair<string, Dictionary<WebSocketClient, long>> kvp in fileClients)
            {
                HashSet<WebSocketClient> unactivedClients = new HashSet<WebSocketClient>();

                foreach (KeyValuePair<WebSocketClient, long> subKvp in kvp.Value)
                {
                    if (subKvp.Value <= threshold)
                        unactivedClients.Add(subKvp.Key);
                }

                foreach (WebSocketClient client in unactivedClients)
                {
                    kvp.Value.Remove(client);
                }

                if (kvp.Value.Count == 0)
                    emptyEndpoints.Add(kvp.Key);
            }

            foreach (string endpoint in emptyEndpoints)
                fileClients.Remove(endpoint);
        }

        //===========================[ Init & Routine Functions ]=========================//
        public void Init()
        {
            Init(null);
        }

        public void Init(RTMConfig config)
        {
            InitCallbackQueue();
            if (config == null)
                return;

            RTMConfig.Config(config);
        }

        private void InitCallbackQueue()
        {
            rtmGameObject = new GameObject(RTMConfig.RTMGameObjectName);
            callbackQueue = rtmGameObject.AddComponent<RTMCallbackQueue>();
            GameObject.DontDestroyOnLoad(rtmGameObject);
            rtmGameObject.hideFlags = HideFlags.HideInHierarchy;
        }
        
        IEnumerator RoutineFunc()
        {
            routineRunning = true;
            while (routineRunning)
            {
                HashSet<RTMClient> clients;
                try
                {
                    clients = new HashSet<RTMClient>();
                }
                catch (Exception e)
                {
                    RTMConfig.errorRecorder?.RecordError(e);
                    continue;
                }

                foreach (KeyValuePair<Int64, RTMClient> kvp in rtmClients)
                    clients.Add(kvp.Value);

                foreach (RTMClient client in clients)
                    if (client.ConnectionIsAlive() == false)
                        client.AsyncClose(false);

                CheckFileGateClients();
                ReloginCheck();
                yield return new WaitForSeconds(1.0f);
            }
        }

        public void Close()
        {
            if (!routineRunning)
                return;

            routineRunning = false;

            HashSet<RTMClient> clients = new HashSet<RTMClient>();

            foreach (KeyValuePair<Int64, RTMClient> kvp in rtmClients)
                clients.Add(kvp.Value);

            foreach (RTMClient client in clients)
                client.AsyncClose(true);

            rtmClients.Clear();
            pidUidClients.Clear();
            reloginClients.Clear();
            fileClients.Clear();
        }
    }
}