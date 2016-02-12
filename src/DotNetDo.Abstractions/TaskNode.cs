using System;
using System.Collections.Generic;

namespace DotNetDo
{
    public class TaskNode
    {
        public TaskDefinition Definition { get; }

        public IList<TaskNode> Dependencies { get; }

        public TaskNode(TaskDefinition definition)
        {
            Definition = definition;
            Dependencies = new List<TaskNode>();
        }
    }
}