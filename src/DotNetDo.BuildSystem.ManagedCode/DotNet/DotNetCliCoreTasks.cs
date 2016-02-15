using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotNetDo.BuildSystem.ManagedCode.DotNet
{
    public class DotNetCliCoreTasks : TaskCollection
    {
        [Task(RunBefore = BuildLifecycle.PreInitialize)]
        public void CheckDotNetCli(ILogger log)
        {
            if (!DotNetCli.Default.Exists())
            {
                throw new FileNotFoundException($"The .NET Core CLI could not be located in expected path: {DotNetCli.Default.BasePath}", DotNetCli.Default.DriverPath);
            }

            if (log.IsEnabled(LogLevel.Trace))
            {
                log.LogTrace("Using .NET Core CLI Path: {0}", DotNetCli.Default.BasePath);
                log.LogTrace("Using .NET Core CLI Version: {0}", DotNetCli.Default.GetVersion());
            }
        }
    }
}
