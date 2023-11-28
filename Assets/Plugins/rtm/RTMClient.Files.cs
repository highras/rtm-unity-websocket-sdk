using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using com.fpnn.proto;
using com.fpnn.common;

namespace com.fpnn.rtm
{
    public partial class RTMClient
    {
        private enum FileTokenType
        {
            P2P,
            Group,
            Room,
            Upload,
        }

        private class SendFileInfo
        {
            public FileTokenType actionType;

            public long xid;
            public byte mtype;
            public byte[] fileContent;
            public string filename;
            public string fileExtension;
            public string userAttrs;
            public long messageId;
            
            public string token;
            public string endpoint;
            public int remainTimeout;
            public long lastActionTimestamp;
            public MessageIdDelegate callback;
            public SendMessageDelegate callbackMtime;
            public Action<string, uint, int> uploadCallback;
            public Dictionary<string, object> rtmAttrs;
        }

        //===========================[ File Token ]=========================//
        //-- Action<token, endpoint, errorCode>
        private bool FileToken(Action<string, string, int> callback, FileTokenType tokenType, long xid, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
                return false;

            Quest quest = new Quest("filetoken");
            switch (tokenType)
            {
                case FileTokenType.P2P:
                    quest.Param("cmd", "sendfile");
                    quest.Param("to", xid);
                    break;

                case FileTokenType.Group:
                    quest.Param("cmd", "sendgroupfile");
                    quest.Param("gid", xid);
                    break;

                case FileTokenType.Room:
                    quest.Param("cmd", "sendroomfile");
                    quest.Param("rid", xid);
                    break;
                case FileTokenType.Upload:
                    quest.Param("cmd", "uploadfile");
                    break;
            }

            return client.SendQuest(quest, (Answer answer, int errorCode) => {

                string token = "";
                string endpoint = "";
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        token = answer.Want<string>("token");
                        endpoint = answer.Want<string>("endpoint");
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(token, endpoint, errorCode);
            }, timeout);
        }

        //===========================[ IPv4 Convert IPv6 Utilies ]=========================//
        private string ConvertIPv4ToIPv6(string ipv4)
        {
            string[] parts = ipv4.Split(new Char[] { '.' });
            if (parts.Length != 4)
                return string.Empty;

            foreach (string part in parts)
            {
                int partInt = Int32.Parse(part);
                if (partInt > 255 || partInt < 0)
                    return string.Empty;
            }

            string part7 = Convert.ToString(Int32.Parse(parts[0]) * 256 + Int32.Parse(parts[1]), 16);
            string part8 = Convert.ToString(Int32.Parse(parts[2]) * 256 + Int32.Parse(parts[3]), 16);
            return "64:ff9b::" + part7 + ":" + part8;
        }

        private bool ConvertIPv4EndpointToIPv6IPPort(string ipv4endpoint, out string ipv6, out int port)
        {
            int idx = ipv4endpoint.LastIndexOf(':');
            if (idx == -1)
            {
                ipv6 = string.Empty;
                port = 0;

                return false;
            }

            string ipv4 = ipv4endpoint.Substring(0, idx);
            string portString = ipv4endpoint.Substring(idx + 1);
            port = Convert.ToInt32(portString, 10);

            ipv6 = ConvertIPv4ToIPv6(ipv4);
            if (ipv6.Length == 0)
                return false;

            return true;
        }

        //===========================[ File Utilies ]=========================//
        private void UpdateTimeout(ref int timeout, ref long lastActionTimestamp)
        {
            long currMsec = ClientEngine.GetCurrentMilliseconds();

            timeout -= (int)((currMsec - lastActionTimestamp) / 1000);

            lastActionTimestamp = currMsec;
        }

        private string ExtraFileExtension(string filename)
        {
            int idx = filename.LastIndexOf('.');
            if (idx == -1)
                return null;

            return filename.Substring(idx + 1);
        }

        private string GetMD5(string str, bool upper)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(str);
            return GetMD5(inputBytes, upper);
        }

        private string GetMD5(byte[] bytes, bool upper)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(bytes);
            string f = "x2";

            if (upper)
            {
                f = "X2";
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString(f));
            }

            return sb.ToString();
        }

        private string BuildFileAttrs(SendFileInfo info)
        {
            string fileMD5 = GetMD5(info.fileContent, false);
            string sign = GetMD5(fileMD5 + ":" + info.token, false);

            if (info.rtmAttrs == null)
                info.rtmAttrs = new Dictionary<string, object>();

            Dictionary<string, object> rtmAttrs = info.rtmAttrs;
            rtmAttrs.Add("sign", sign);

            if (info.filename != null && info.filename.Length > 0)
            {
                rtmAttrs.Add("filename", info.filename);

                if (info.fileExtension == null || info.fileExtension.Length == 0)
                    info.fileExtension = ExtraFileExtension(info.filename);
            }
            if (info.fileExtension != null && info.fileExtension.Length > 0)
                rtmAttrs.Add("ext", info.fileExtension);

            Dictionary<string, object> fileAttrs = new Dictionary<string, object>();
            fileAttrs.Add("rtm", rtmAttrs);

            if (info.userAttrs == null || info.userAttrs.Length == 0)
                fileAttrs.Add("custom", "");
            else
            {
                try
                {
                    Dictionary<string, object> userDict = Json.ParseObject(info.userAttrs);
                    if (userDict != null)
                        fileAttrs.Add("custom", userDict);
                    else
                        fileAttrs.Add("custom", info.userAttrs);
                }
                catch (JsonException)
                {
                    fileAttrs.Add("custom", info.userAttrs);
                }
            }

            return Json.ToString(fileAttrs);
        }

        private Quest BuildSendFileQuest(out long messageId, SendFileInfo info)
        {
            Quest quest = null;
            switch (info.actionType)
            {
                case FileTokenType.P2P:
                    quest = new Quest("sendfile");
                    quest.Param("to", info.xid);
                    break;

                case FileTokenType.Group:
                    quest = new Quest("sendgroupfile");
                    quest.Param("gid", info.xid);
                    break;

                case FileTokenType.Room:
                    quest = new Quest("sendroomfile");
                    quest.Param("rid", info.xid);
                    break;
                case FileTokenType.Upload:
                    quest = new Quest("uploadfile");
                    quest.Param("uid", uid);
                    break;
            }

            quest.Param("pid", projectId);
            quest.Param("from", uid);
            quest.Param("token", info.token);
            quest.Param("mtype", info.mtype);
            messageId = info.messageId;
            if (info.messageId == 0)
                messageId = MidGenerator.Gen();
            quest.Param("mid", messageId);

            quest.Param("file", info.fileContent);
            quest.Param("attrs", BuildFileAttrs(info));
            quest.Param("endpoint", info.endpoint);

            return quest;
        }

        private int SendFileWithClient(SendFileInfo info, WebSocketClient client)
        {
            UpdateTimeout(ref info.remainTimeout, ref info.lastActionTimestamp);
            if (info.remainTimeout <= 0)
                return fpnn.ErrorCode.FPNN_EC_CORE_TIMEOUT;

            long messageId = 0;
            Quest quest = BuildSendFileQuest(out messageId, info);
            bool success = client.SendQuest(quest, (Answer answer, int errorCode) => {

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        if (info.callbackMtime == null)
                            info.callback(messageId, fpnn.ErrorCode.FPNN_EC_OK);
                        else
                        { 
                            long mtime = answer.Want<long>("mtime");
                            info.callbackMtime(messageId, mtime, fpnn.ErrorCode.FPNN_EC_OK);
                        }

                        RTMControlCenter.Instance.ActiveFileGateClient(client.Endpoint(), client);
                        return;
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }

                if (info.callbackMtime == null)
                    info.callback(0, errorCode);
                else
                    info.callbackMtime(0, 0, errorCode);
            }, info.remainTimeout);

            if (success)
                return fpnn.ErrorCode.FPNN_EC_OK;
            else
                return fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION;
        }

        private int UploadFileWithClient(SendFileInfo info, WebSocketClient client)
        {
            UpdateTimeout(ref info.remainTimeout, ref info.lastActionTimestamp);
            if (info.remainTimeout <= 0)
                return fpnn.ErrorCode.FPNN_EC_CORE_TIMEOUT;

            long messageId = 0;
            Quest quest = BuildSendFileQuest(out messageId, info);
            bool success = client.SendQuest(quest, (Answer answer, int errorCode) => {

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    {
                        string url = answer.Want<string>("url");
                        uint size = answer.Want<uint>("size");
                        info.uploadCallback(url, size, fpnn.ErrorCode.FPNN_EC_OK);

                        RTMControlCenter.Instance.ActiveFileGateClient(client.Endpoint(), client);
                        return;
                    }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }

                info.uploadCallback(null, 0, errorCode);
            }, info.remainTimeout);

            if (success)
                return fpnn.ErrorCode.FPNN_EC_OK;
            else
                return fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION;
        }
        
        private int SendFileWithoutClient(SendFileInfo info, bool originalEndpoint)
        {
            string[] ep = rtmGate.Endpoint().Split(":");
            
            string fileGateEndpoint = "wss://fileproxy-" + ep[1].Split("//")[1] + ":13462/service/websocket";
            // fileGateEndpoint = "wss://rtm-wss-test-nx.livedata.top:13362/service/websocket";
            WebSocketClient client = WebSocketClient.Create(fileGateEndpoint, true);
            if (errorRecorder != null)
                client.SetErrorRecorder(errorRecorder);

            client.SetConnectionConnectedDelegate(() => {
                int errorCode = fpnn.ErrorCode.FPNN_EC_OK;

                RTMControlCenter.Instance.ActiveFileGateClient(fileGateEndpoint, client);
                errorCode = SendFileWithClient(info, client);

                if (errorCode != fpnn.ErrorCode.FPNN_EC_OK)
                {
                    if (info.callbackMtime == null)
                        info.callback(0, errorCode);
                    else
                        info.callbackMtime(0, 0, errorCode);
                }

                client.SetConnectionConnectedDelegate(null);
            });

            client.AsyncConnect();

            return fpnn.ErrorCode.FPNN_EC_OK;
        }
        private int UploadFileWithoutClient(SendFileInfo info, bool originalEndpoint)
        {
            string[] ep = rtmGate.Endpoint().Split(":");
            string fileGateEndpoint = "wss://fileproxy-" + ep[1].Split("//")[1] + ":13462/service/websocket";
            // fileGateEndpoint = "wss://rtm-wss-test-nx.livedata.top:13362/service/websocket";
            WebSocketClient client = WebSocketClient.Create(fileGateEndpoint, true);
            if (errorRecorder != null)
                client.SetErrorRecorder(errorRecorder);

            client.SetConnectionConnectedDelegate(() => {
                int errorCode = fpnn.ErrorCode.FPNN_EC_OK;

                RTMControlCenter.Instance.ActiveFileGateClient(fileGateEndpoint, client);
                errorCode = UploadFileWithClient(info, client);
                

                if (errorCode != fpnn.ErrorCode.FPNN_EC_OK)
                    info.uploadCallback(null, 0, errorCode);

                client.SetConnectionConnectedDelegate(null);
            });

            client.AsyncConnect();

            return fpnn.ErrorCode.FPNN_EC_OK;
        }

        private void GetFileTokenCallback(SendFileInfo info, string token, string endpoint, int errorCode)
        {
            if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
            {
                info.token = token;
                info.endpoint = endpoint;

                WebSocketClient fileClient = RTMControlCenter.Instance.FecthFileGateClient(info.endpoint);
                if (fileClient != null)
                    errorCode = SendFileWithClient(info, fileClient);
                else
                    errorCode = SendFileWithoutClient(info, true);

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                    return;
            }
            else
            {
                if (info.callbackMtime == null)
                    info.callback(0, errorCode);
                else
                    info.callbackMtime(0, 0, errorCode);
            }
        }

        private void GetFileTokenUploadCallback(SendFileInfo info, string token, string endpoint, int errorCode)
        {
            if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
            {
                info.token = token;
                info.endpoint = endpoint;

                WebSocketClient fileClient = RTMControlCenter.Instance.FecthFileGateClient(info.endpoint);
                if (fileClient != null)
                    errorCode = UploadFileWithClient(info, fileClient);
                else
                    errorCode = UploadFileWithoutClient(info, true);

                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                    return;
            }
            else
                info.uploadCallback(null, 0, errorCode);
        }

        //===========================[ Real Send File ]=========================//
        private bool RealSendFile(MessageIdDelegate callback, FileTokenType tokenType, long targetId, byte mtype,
            byte[] fileContent, string filename, string fileExtension, string attrs, Dictionary<string, object> rtmAttrs, long messageId, int timeout)
        {
            if (mtype < 40 || mtype > 50)
            {
                if (errorRecorder != null)
                    errorRecorder.RecordError("Send file require mtype between [40, 50], current mtype is " + mtype);

                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(0, ErrorCode.RTM_EC_INVALID_MTYPE);
                    });

                return false;
            }

            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            SendFileInfo info = new SendFileInfo
            {
                actionType = tokenType,
                xid = targetId,
                mtype = mtype,
                fileContent = fileContent,
                filename = filename,
                fileExtension = fileExtension,
                userAttrs = attrs,
                messageId = messageId,
                remainTimeout = timeout,
                lastActionTimestamp = ClientEngine.GetCurrentMilliseconds(),
                callback = callback
            };
            info.rtmAttrs = rtmAttrs;

            bool asyncStarted = FileToken((string token, string endpoint, int errorCode) => {
                GetFileTokenCallback(info, token, endpoint, errorCode);
            }, tokenType, info.xid, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool RealSendFile(SendMessageDelegate callback, FileTokenType tokenType, long targetId, byte mtype,
            byte[] fileContent, string filename, string fileExtension, string attrs, Dictionary<string, object> rtmAttrs, long messageId, int timeout)
        {
            if (mtype < 40 || mtype > 50)
            {
                if (errorRecorder != null)
                    errorRecorder.RecordError("Send file require mtype between [40, 50], current mtype is " + mtype);

                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(0, 0, ErrorCode.RTM_EC_INVALID_MTYPE);
                    });

                return false;
            }

            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            SendFileInfo info = new SendFileInfo
            {
                actionType = tokenType,
                xid = targetId,
                mtype = mtype,
                fileContent = fileContent,
                filename = filename,
                fileExtension = fileExtension,
                userAttrs = attrs,
                messageId = messageId,
                remainTimeout = timeout,
                lastActionTimestamp = ClientEngine.GetCurrentMilliseconds(),
                callbackMtime = callback
            };
            info.rtmAttrs = rtmAttrs;

            bool asyncStarted = FileToken((string token, string endpoint, int errorCode) => {
                GetFileTokenCallback(info, token, endpoint, errorCode);
            }, tokenType, info.xid, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(0, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        private bool RealUploadFile(Action<string, uint, int> callback, FileTokenType tokenType, byte mtype,
            byte[] fileContent, string filename, string fileExtension, string attrs, Dictionary<string, object> rtmAttrs, int timeout)
        {
            if (mtype < 40 || mtype > 50)
            {
                if (errorRecorder != null)
                    errorRecorder.RecordError("Send file require mtype between [40, 50], current mtype is " + mtype);

                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(null, 0, ErrorCode.RTM_EC_INVALID_MTYPE);
                    });

                return false;
            }

            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(null, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            SendFileInfo info = new SendFileInfo
            {
                actionType = tokenType,
                xid = 0,
                mtype = mtype,
                fileContent = fileContent,
                filename = filename,
                fileExtension = fileExtension,
                userAttrs = attrs,
                remainTimeout = timeout,
                lastActionTimestamp = ClientEngine.GetCurrentMilliseconds(),
                uploadCallback = callback
            };
            info.rtmAttrs = rtmAttrs;

            bool asyncStarted = FileToken((string token, string endpoint, int errorCode) => {
                GetFileTokenUploadCallback(info, token, endpoint, errorCode);
            }, tokenType, info.xid, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, 0, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Send File ]=========================//
        public bool SendFile(MessageIdDelegate callback, long peerUid, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120)
        {
            return RealSendFile(callback, FileTokenType.P2P, peerUid, (byte)type, fileContent, filename, fileExtension, attrs, null, 0, timeout);
        }

        public bool SendFile(SendMessageDelegate callback, long peerUid, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120)
        {
            return RealSendFile(callback, FileTokenType.P2P, peerUid, (byte)type, fileContent, filename, fileExtension, attrs, null, 0, timeout);
        }

        //===========================[ Sned Group File ]=========================//
        public bool SendGroupFile(MessageIdDelegate callback, long groupId, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120)
        {
            return RealSendFile(callback, FileTokenType.Group, groupId, (byte)type, fileContent, filename, fileExtension, attrs, null, 0, timeout);
        }

        public bool SendGroupFile(SendMessageDelegate callback, long groupId, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120)
        {
            return RealSendFile(callback, FileTokenType.Group, groupId, (byte)type, fileContent, filename, fileExtension, attrs, null, 0, timeout);
        }

        //===========================[ Sned Room File ]=========================//
        public bool SendRoomFile(MessageIdDelegate callback, long roomId, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120)
        {
            return RealSendFile(callback, FileTokenType.Room, roomId, (byte)type, fileContent, filename, fileExtension, attrs, null, 0, timeout);
        }

        public bool SendRoomFile(SendMessageDelegate callback, long roomId, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120)
        {
            return RealSendFile(callback, FileTokenType.Room, roomId, (byte)type, fileContent, filename, fileExtension, attrs, null, 0, timeout);
        }

        //===========================[ Upload File ]=========================//
        public bool UploadFile(Action<string, uint, int> callback, MessageType type, byte[] fileContent, string filename, string fileExtension = "", string attrs = "", int timeout = 120)
        {
            return RealUploadFile(callback, FileTokenType.Upload, (byte)type, fileContent, filename, fileExtension, attrs, null, timeout);
        }
    }
}
