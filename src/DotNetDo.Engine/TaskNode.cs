using System;
using System.Collections.Generic;

namespace DotNetDo.Engine
{
    public class TaskNode
    {
        private bool _hasRun = false;

        public TaskDefinition Definition { get; }

        public IList<TaskNode> Dependencies { get; }

        public TaskResult Result { get; private set; }

        public TaskNode(TaskDefinition definition)
        {
            Definition = definition;
            Dependencies = new List<TaskNode>();
        }

        public TaskResult Execute(TaskInvocation invocation)
        {
            if (!_hasRun)
            {
                _hasRun = true;
                Result = Definition.Implementation(invocation);
            }
            return Result;
        }
    }
}