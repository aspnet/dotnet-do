using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDo.Engine
{
    public class TaskRunnerLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new TaskRunnerLogger(categoryName);
        }

        public void Dispose()
        {
        }

        private class TaskRunnerLogger : ILogger
        {
            private string _categoryName;

            public TaskRunnerLogger(string categoryName)
            {
                _categoryName = categoryName;
            }

            public IDisposable BeginScopeImpl(object state)
            {
                var action = state as TaskRunnerActivity;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
            }

            public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
            }
        }
    }
}
