using System;

namespace DotNetDo.Engine
{
    internal class TaskFailedException : Exception
    {
        public string FailedTask { get; }

        public TaskFailedException(string failedTask)
        {
            FailedTask = failedTask;
        }

        public TaskFailedException(string failedTask, string message) : base(message)
        {
            FailedTask = failedTask;
        }

        public TaskFailedException(string failedTask, string message, Exception innerException) : base(message, innerException)
        {
            FailedTask = failedTask;
        }
    }
}