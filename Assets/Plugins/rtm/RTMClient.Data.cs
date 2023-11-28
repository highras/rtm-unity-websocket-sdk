using System;
using com.fpnn.proto;
namespace com.fpnn.rtm
{
    public partial class RTMClient
    {
        //===========================[ Data Get ]=========================//
        //-- Action<value, errorCode>
        public bool DataGet(Action<string, int> callback, string key, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("dataget");
            quest.Param("key", key);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => {

                string value = null;
                if (errorCode == fpnn.ErrorCode.FPNN_EC_OK)
                {
                    try
                    { value = answer.Get<string>("val", null); }
                    catch (Exception)
                    {
                        errorCode = fpnn.ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE;
                    }
                }
                callback(value, errorCode);
            }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(null, fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Data Set ]=========================//
        public bool DataSet(DoneDelegate callback, string key, string value, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("dataset");
            quest.Param("key", key);
            quest.Param("val", value);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }

        //===========================[ Data Delete ]=========================//
        public bool DataDelete(DoneDelegate callback, string key, int timeout = 0)
        {
            WebSocketClient client = GetCoreClient();
            if (client == null)
            {
                if (RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                    RTMControlCenter.callbackQueue.PostAction(() =>
                    {
                        callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                    });

                return false;
            }

            Quest quest = new Quest("datadel");
            quest.Param("key", key);

            bool asyncStarted = client.SendQuest(quest, (Answer answer, int errorCode) => { callback(errorCode); }, timeout);

            if (!asyncStarted && RTMConfig.triggerCallbackIfAsyncMethodReturnFalse)
                RTMControlCenter.callbackQueue.PostAction(() =>
                {
                    callback(fpnn.ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                });

            return asyncStarted;
        }
    }
}
