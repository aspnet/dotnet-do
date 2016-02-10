using DotNetDo.Engine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DevSandbox
{
    public class SomeTasks : TaskCollection
    {
        [Task(nameof(Prepare), nameof(Compile), nameof(Test), nameof(Package), nameof(Publish))]
        public void Default(ILogger log) { }

        public void Prepare(ILogger log, TaskContext context)
        {
        }
    }
}
