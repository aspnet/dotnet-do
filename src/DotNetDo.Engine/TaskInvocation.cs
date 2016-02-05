using System;
using System.Collections.Generic;

namespace DotNetDo.Engine
{
    public class TaskInvocation
    {
        public IServiceProvider Services { get; }

        public IReadOnlyDictionary<string, TaskResult> DependencyResults { get; }
        public TaskNode Task { get; }

        public TaskInvocation(TaskNode task, IServiceProvider services, Dictionary<string, TaskResult> dependencyResults)
        {
            Task = task;
            Services = services;
            DependencyResults = dependencyResults;
        }
    }
}