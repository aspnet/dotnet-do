using System;
using System.Diagnostics;

namespace DotNetDo
{
    public class TaskRunnerActivity : IDisposable
    {
        private IDisposable _scopeToken;

        public string Type { get; }
        public string Name { get; }
        public string Conclusion { get; set; }
        public bool Success { get; set; }

        public TaskRunnerActivity(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public void AttachToScope(IDisposable loggerScope)
        {
            Debug.Assert(_scopeToken == null, "Tried to attach to logger scope while already attached to one");
            _scopeToken = loggerScope;
        }

        public void Dispose()
        {
            _scopeToken?.Dispose();
        }
    }
}