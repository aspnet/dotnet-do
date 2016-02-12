using System.Collections.Generic;

namespace DotNetDo
{
    public interface ITaskProvider
    {
        IEnumerable<TaskDefinition> GetTasks();
    }
}