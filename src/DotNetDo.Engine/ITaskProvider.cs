using System.Collections.Generic;

namespace DotNetDo.Engine
{
    public interface ITaskProvider
    {
        IEnumerable<TaskDefinition> GetTargets();
    }
}