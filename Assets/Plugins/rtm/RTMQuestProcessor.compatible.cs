using System;
using com.fpnn.common;

namespace com.fpnn.rtm
{
    public interface IRTMMasterProcessor: IQuestProcessor
    {
        void SetErrorRecorder(ErrorRecorder recorder);
        void SetConnectionId(Int64 connId);
        void SetUid(Int64 uid);
        void BeginCheckPingInterval();
        bool ConnectionIsAlive();
        void SessionClosed(int ClosedByErrorCode);

        bool ReloginWillStart(int lastErrorCode, int retriedCount);
        void ReloginCompleted(bool successful, bool retryAgain, int errorCode, int retriedCount);
    }
}
