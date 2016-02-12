using DotNetDo.Engine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DevSandbox
{
    public class SomeTasks : TaskCollection
    {
        [Task(nameof(Prepare))]
        public void Default(ILogger log) { }

        [Task]
        public void Prepare(ILogger log, TaskContext context)
        {
        }
    }
}
