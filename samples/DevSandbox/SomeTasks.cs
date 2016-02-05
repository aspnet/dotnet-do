using DotNetDo.Engine;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DevSandbox
{
    public class SomeTasks : TaskCollection
    {
        [Task(nameof(Prepare), nameof(Compile), nameof(Test), nameof(Package), nameof(Publish))]
        public void Default(ILogger log)
        {
            log.LogInformation("The default task has run!");
        }
        
        [Task]
        public void Clean(ILogger log)
        {
            log.LogInformation("The clean task has run!");
        }

        [Task]
        public void Prepare(ILogger log)
        {
            log.LogInformation("The prepare task has run!");
        }

        [Task]
        public void Compile(ILogger log)
        {
            log.LogInformation("The compile task has run!");
        }

        [Task]
        public void Test(ILogger log)
        {
            log.LogInformation("The test task has run!");
        }

        [Task]
        public void Package(ILogger log)
        {
            log.LogInformation("The package task has run!");
        }

        [Task]
        public void Publish(ILogger log)
        {
            log.LogInformation("The publish task has run!");
        }
    }
}
