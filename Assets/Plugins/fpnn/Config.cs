﻿using System;
namespace com.fpnn
{
    public class Config
    {
        public static readonly string Version = "2.0.9";

        //----------------[ Nested Structure ]-----------------------//
        public struct TaskThreadPoolConfig
        {
            public int initThreadCount;
            public int perfectThreadCount;
            public int maxThreadCount;
            public int maxQueueLengthLimitation;
            public int tempLatencySeconds;
        }

        //----------------[ Customized Fields ]-----------------------//

        public TaskThreadPoolConfig taskThreadPoolConfig;
        public int globalConnectTimeoutSeconds;
        public int globalQuestTimeoutSeconds;
        public int maxPayloadSize;
        public common.ErrorRecorder errorRecorder;
        public bool dropAllUnexecutedTaskWhenExiting;
        public bool closeConnectionsWhenBackground;

        public Config()
        {
            taskThreadPoolConfig.initThreadCount = 1;
            taskThreadPoolConfig.perfectThreadCount = 8;
            taskThreadPoolConfig.maxThreadCount = 16;
            taskThreadPoolConfig.maxQueueLengthLimitation = 0;
            taskThreadPoolConfig.tempLatencySeconds = 60;

            globalConnectTimeoutSeconds = 5;
            globalQuestTimeoutSeconds = 5;
            maxPayloadSize = 1024 * 1024 * 4;        //-- 4MB

            closeConnectionsWhenBackground = false;
#if UNITY_EDITOR
            dropAllUnexecutedTaskWhenExiting = true;
#else
            dropAllUnexecutedTaskWhenExiting = false;
#endif
        }
    }
}
