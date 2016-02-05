using System;

namespace DotNetDo.Engine
{
    public class TaskResult
    {
        public TaskNode Task { get; }
        public bool Success { get; }
        public object ReturnValue { get; }
        public Exception Exception { get; }

        public TaskResult(TaskNode task, bool success, object returnValue, Exception exception)
        {
            Task = task;
            Success = success;
            ReturnValue = returnValue;
            Exception = exception;
        }
    }
}