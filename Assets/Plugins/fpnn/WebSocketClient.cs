using System;
using System.Net;
using System.Threading;
using com.fpnn.proto;
using UnityEditor;
using UnityWebSocket;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ErrorEventArgs = UnityWebSocket.ErrorEventArgs;

namespace com.fpnn
{
    public delegate Answer WebSocketQuestProcessDelegate(Quest quest);
    public class WebSocketClient
    {
        public delegate void ConnectionConnectedDelegate();
        public delegate void ConnectionCloseDelegate();

        public enum ClientStatus
        {
            Closed,
            Closing,
            Connecting,
            Connected
        }
        private class AnswerCallbackUnit
        {
            public IAnswerCallback callback;
            public UInt32 seqNum;
            public Int64 timeoutTime;
        }       
        
        private Dictionary<Int64, HashSet<AnswerCallbackUnit>> callbackTimeoutMap = new Dictionary<long, HashSet<AnswerCallbackUnit>>();
        private Dictionary<UInt32, AnswerCallbackUnit> callbackSeqNumMap = new Dictionary<uint, AnswerCallbackUnit>();

        //----------------[ fields ]-----------------------//
        
        // private object interLocker;
        // private readonly DnsEndPoint dnsEndpoint;
        // public volatile int ConnectTimeout;
        // public volatile int QuestTimeout;
        // public volatile bool AutoConnect;
        
        // private ClientStatus status;
        private WebSocket websocket;
        private WebSocketReceiver receiver;
        private ConnectionConnectedDelegate connectConnectedDelegate;
        private ConnectionCloseDelegate connectionCloseDelegate;
        private IQuestProcessor questProcessor;
        
        private common.ErrorRecorder errorRecorder;
        
        //----------------[ Constructor ]-----------------------//
        public WebSocketClient(string endpoint, bool autoConnect = true)
        {
            // interLocker = new object();
            // dnsEndpoint = new DnsEndPoint(host, port);
            // ConnectTimeout = 0;
            // QuestTimeout = 0;
            // AutoConnect = autoConnect;
            // status = ClientStatus.Closed;
            websocket = new WebSocket(endpoint);
            receiver = new WebSocketReceiver();

            websocket.OnOpen += (object sender, OpenEventArgs args) =>
            {
                connectConnectedDelegate?.Invoke();
            };

            websocket.OnClose += (object sender, CloseEventArgs args) =>
            {
                connectionCloseDelegate?.Invoke();
            };

            websocket.OnError += (object sender, ErrorEventArgs args) =>
            {
                errorRecorder?.RecordError("websocket OnError message = " + args.Message);
            };

            websocket.OnMessage += OnMessage;
        
            errorRecorder = ClientEngine.errorRecorder;
        }
        
        public static WebSocketClient Create(string endpoint, bool autoConnect = true)
        {
            int idx = endpoint.LastIndexOf(':');
            if (idx == -1)
                throw new ArgumentException("Invalid endpoint: " + endpoint);
        
            return new WebSocketClient(endpoint, autoConnect);
        }
        
        public void SetConnectionConnectedDelegate(ConnectionConnectedDelegate ccd)
        {
            connectConnectedDelegate = ccd;
        }

        public void SetConnectionCloseDelegate(ConnectionCloseDelegate cwcd)
        {
            connectionCloseDelegate = cwcd;
        } 
        public void SetErrorRecorder(common.ErrorRecorder recorder)
        {
            errorRecorder = recorder;
        }
        
        public void SetQuestProcessor(IQuestProcessor processor)
        {
            questProcessor = processor;
        }
        
        //----------------[ Properties methods ]-----------------------//
        
        public string Endpoint()
        {
            return websocket.Address;
        }
        
        public ClientStatus Status()
        {
            switch (websocket.ReadyState)
            {
                case WebSocketState.Connecting:
                    return ClientStatus.Connecting;
                case WebSocketState.Open:
                    return ClientStatus.Connected;
                case WebSocketState.Closed:
                    return ClientStatus.Closed;
                case WebSocketState.Closing:
                    return ClientStatus.Closing;
                default:
                    return ClientStatus.Closed;
            }
        }
        
        public bool IsConnected()
        {
            // lock (interLocker)
            {
                return websocket.ReadyState == WebSocketState.Open;
            }
        }
        
        //----------------[ Connect Operations ]-----------------------//
        private void RealConnect()
        {
            if (Status() != ClientStatus.Closed)
                return;
            
            websocket.ConnectAsync();
        }
        
        public void AsyncConnect()
        {
            RealConnect();
        }
        
        public void AsyncReconnect()
        {
            Close();
            AsyncConnect();
        }
       
        public void Close()
        {
            websocket.CloseAsync();
        }
        
        //----------------[ Operations ]-----------------------//
        public bool SendQuest(Quest quest, IAnswerCallback callback, int timeout = 0)
        {
            // if (AutoConnect)
            //     AsyncConnect();     //-- Auto check and reconnect if necessary.
            byte[] raw;
            try
            {
                raw = quest.Raw();
            }
            catch (Exception ex)
            {
                if (callback != null)
                    callback.OnException(null, ErrorCode.FPNN_EC_PROTO_UNKNOWN_ERROR);
        
                if (errorRecorder != null)
                    errorRecorder.RecordError("Send quest cannelled. Quest.Raw() exception.", ex);

                return false;
            }

            if (Status() == ClientStatus.Closed)
            {
                if (callback != null)
                    callback.OnException(null, ErrorCode.FPNN_EC_CORE_INVALID_CONNECTION);
                        
                if (errorRecorder != null)
                    errorRecorder.RecordError("Send Quest " + quest.Method() + " on closed connection.");
                return false;
            }

            websocket.SendAsync(raw);
            
            if (callback != null)
            {
                if (timeout == 0)
                    timeout = ClientEngine.globalQuestTimeoutSeconds;
        
                TimeSpan span = DateTime.UtcNow - ClientEngine.originDateTime;
                Int64 seconds = (Int64)Math.Floor(span.TotalSeconds) + timeout;
        
                AnswerCallbackUnit unit = new AnswerCallbackUnit();
                unit.callback = callback;
                unit.seqNum = quest.SeqNum();
                unit.timeoutTime = seconds;
        
                callbackSeqNumMap.Add(quest.SeqNum(), unit);
        
                if (callbackTimeoutMap.TryGetValue(seconds, out HashSet<AnswerCallbackUnit> cbSet))
                {
                    cbSet.Add(unit);
                }
                else
                {
                    cbSet = new HashSet<AnswerCallbackUnit>();
                    cbSet.Add(unit);
                    callbackTimeoutMap.Add(seconds, cbSet);
                }
            }
        
            return true;
        }
        
        public bool SendQuest(Quest quest, AnswerDelegate callback, int timeout = 0)
        {
            AnswerDelegateCallback cb = new AnswerDelegateCallback(callback);
            return SendQuest(quest, cb, timeout);
        }

        private void OnMessage(object sender, MessageEventArgs args)
        {
            receiver.AddBuffer(args.RawData);
            
            Quest quest;
            Answer answer;
         
            bool loop = false;
            do
            {
                try
                {
                    loop = receiver.Done(out quest, out answer);
                }
                catch (ReceiverErrorMessageException ex)
                {
                    CloseByException("Processing received data from " + websocket.Address + " error: " + ex.Message + ". Connection will be closed.", null, false);
                    return;
                }
                catch (Exception ex)
                {
                    CloseByException("Processing received data from " + websocket.Address + " exception. Connection will be closed.", ex, false);
                    return;
                }
                                    
                if (answer != null)
                    DealAnswer(answer);
                else if (quest != null)
                    DealQuest(quest);     
            } while (loop);
        }
        
        private void CloseByException(string message, Exception ex, bool socketDisposed)
        {
            if (errorRecorder != null)
            {
                if (ex != null)
                    errorRecorder.RecordError(message, ex);
                else
                    errorRecorder.RecordError(message);
            }
        
            websocket.CloseAsync();
        }
        
        private void DealQuest(Quest quest)
        {
            if (questProcessor != null)
            {
                WebSocketQuestProcessDelegate process = questProcessor.GetWebSocketQuestProcessDelegate(quest.Method());
                if (process != null)
                {
                    RunQuestProcessor(quest, process);
                }
                else
                {
                    if (quest.IsTwoWay())
                    {
                        Answer answer = new Answer(quest);
                        answer.FillErrorInfo(ErrorCode.FPNN_EC_CORE_UNKNOWN_METHOD, "This method is not supported by client.");
                        SendAnswer(answer);
                    }
                }
            }
            else
            {
                if (quest.IsTwoWay())
                {
                    Answer answer = new Answer(quest);
                    answer.FillErrorInfo(ErrorCode.FPNN_EC_CORE_INVALID_PACKAGE, "Client without quest processor.");
                    SendAnswer(answer);
                }
            }
        }
        
        private void DealAnswer(Answer answer)
        {
            AnswerCallbackUnit unit = null;
            UInt32 seq = answer.SeqNum();
            if (callbackSeqNumMap.TryGetValue(seq, out unit))
            {
                callbackSeqNumMap.Remove(seq);
        
                if (callbackTimeoutMap.TryGetValue(unit.timeoutTime, out HashSet<AnswerCallbackUnit> cbSet))
                {
                    cbSet.Remove(unit);
                    if (cbSet.Count == 0)
                        callbackTimeoutMap.Remove(unit.timeoutTime);
                }
            }
            
            unit?.callback.OnAnswer(answer);
        }
        
        private void RunQuestProcessor(Quest quest, WebSocketQuestProcessDelegate process)
        {
            Answer answer = null;
            bool asyncAnswered = false;
            WebSocketAdvancedAnswerInfo.Reset(this, quest);
        
            try
            {
                answer = process(quest);
            }
            catch (Exception ex)
            {
                if (errorRecorder != null)
                    errorRecorder.RecordError("Run quest process for method: " + quest.Method(), ex);
            }
            finally
            {
                asyncAnswered = AdvancedAnswerInfo.Answered();
            }
        
            if (quest.IsTwoWay() && !asyncAnswered)
            {
                if (answer == null)
                {
                    answer = new Answer(quest);
                    answer.FillErrorInfo(ErrorCode.FPNN_EC_CORE_UNKNOWN_ERROR, "Two way quest " + quest.Method() + " lose an answer.");
                }
                SendAnswer(answer);
            }
            else
            {
                if (answer != null)
                    if (errorRecorder != null)
                    {
                        if (quest.IsOneWay())
                            errorRecorder.RecordError("Answer created for one way quest: " + quest.Method());
                        else
                            errorRecorder.RecordError("Answer created reduplicated for two way quest: " + quest.Method());
                    }
            }
        }
        
        public void SendAnswer(Answer answer)
        {
            byte[] raw;
        
            try
            {
                raw = answer.Raw();
            }
            catch (Exception ex)
            {
                errorRecorder?.RecordError("Send answer cannelled. Answer.Raw() exception.", ex);
                return;
            }

            if (Status() != ClientStatus.Connected)
            {
                errorRecorder?.RecordError("Send answer on closed connection.");
                return;
            }
            websocket.SendAsync(raw);
        }
    }
}