using System;
using System.Collections.Generic;

namespace DotNetDo.Engine
{
    public class TaskDefinition
    {
        public string Name { get; }

        public string Source { get; }

        public IEnumerable<string> DependsOn { get; }
        
        public string RunBefore { get; }

        public Func<TaskInvocation, TaskResult> Implementation { get; }

        public TaskDefinition(string name, string source, IEnumerable<string> dependsOn, string runBefore, Func<TaskInvocation, TaskResult> implementation)
        {
            Name = name;
            Source = source;
            DependsOn = dependsOn;
            RunBefore = runBefore;
            Implementation = implementation;
        }
    }
}