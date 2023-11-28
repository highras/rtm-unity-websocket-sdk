using System.Threading;
using com.fpnn.proto;
using System;
using UnityWebSocket;

namespace com.fpnn
{
    internal class AdvancedAnswerInfo
    {
        public Quest quest;
        public TCPConnection connection;

        private static ThreadLocal<AdvancedAnswerInfo> instance = new ThreadLocal<AdvancedAnswerInfo>(() => { return new AdvancedAnswerInfo(); });

        public static void Reset(TCPConnection conn, Quest quest)
        {
            AdvancedAnswerInfo ins = instance.Value;
            ins.quest = quest;
            ins.connection = conn;
        }

        public static TCPConnection TakeConnection()
        {
            AdvancedAnswerInfo ins = instance.Value;
            TCPConnection conn = ins.connection;
            ins.connection = null;
            return conn;
        }

        public static AdvancedAnswerInfo Get()
        {
            return instance.Value;
        }

        public static bool Answered()
        {
            AdvancedAnswerInfo ins = instance.Value;
            bool answered = ins.connection == null;
            ins.connection = null;
            ins.quest = null;
            return answered;
        }
    }
    internal class WebSocketAdvancedAnswerInfo
    {
        public Quest quest;
        public WebSocketClient websocket;
    
        private static WebSocketAdvancedAnswerInfo instance = new WebSocketAdvancedAnswerInfo();
    
        public static void Reset(WebSocketClient websocket, Quest quest)
        {
            instance.quest = quest;
            instance.websocket = websocket;
        }
    
        public static WebSocketClient TakeConnection()
        {
            WebSocketClient websocket = instance.websocket;
            instance.websocket = null;
            return websocket;
        }
    
        public static WebSocketAdvancedAnswerInfo Get()
        {
            return instance;
        }
    
        public static bool Answered()
        {
            bool answered = instance.websocket == null;
            instance.websocket = null;
            instance.quest = null;
            return answered;
        }
    }

    public class AsyncAnswer
    {
        private bool sent;
        private object interLocker;
        private Quest quest;
        private TCPConnection connection;

        private AsyncAnswer()
        {
            sent = false;
            interLocker = new object();
        }

        ~AsyncAnswer()
        {
            if (!sent)
            {
                Answer answer = new Answer(quest);
                answer.FillErrorInfo(ErrorCode.FPNN_EC_CORE_UNKNOWN_ERROR, "No answer created by logic.");
                SendAnswer(answer);
            }
        }

        public static AsyncAnswer Create()
        {
            AdvancedAnswerInfo info = AdvancedAnswerInfo.Get();
            if (info.connection == null)
                return null;

            AsyncAnswer async = new AsyncAnswer();
            async.connection = info.connection;
            async.quest = info.quest;

            info.connection = null;
            return async;
        }

        public bool SendAnswer(Answer answer)
        {
            lock (interLocker)
            {
                if (sent)
                    return false;
                else
                    sent = true;
            }

            connection.SendAnswer(answer);
            return true;
        }
    }

    public static class AdvanceAnswer
    {
        public static bool SendAnswer(Answer answer)
        {
            TCPConnection conn = AdvancedAnswerInfo.TakeConnection();
            if (conn != null)
            {
                conn.SendAnswer(answer);
                return true;
            }
            else
                return false;
        }

        public static bool WebSocketSendAnswer(Answer answer)
        {
            WebSocketClient websocket = WebSocketAdvancedAnswerInfo.TakeConnection();
            if (websocket != null)
            {
                websocket.SendAnswer(answer);
                return true;
            }
            else
                return false;    
        }
    }
}