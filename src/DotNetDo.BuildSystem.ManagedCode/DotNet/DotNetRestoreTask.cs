using System.IO;
using System.Threading.Tasks;
using DotNetDo.Helpers;
using Microsoft.Extensions.Logging;

namespace DotNetDo.BuildSystem.ManagedCode.DotNet
{
    public class DotNetRestoreTask : TaskCollection
    {
        [Task(RunBefore = BuildLifecycle.InitializeCore)]
        public void RestoreNuGetPackages(ILogger log, NuGetRestoreTaskOptions options, CommandHelper exec)
        {
            foreach (var dir in options.TargetDirectories)
            {
                if (Directory.Exists(dir))
                {
                    log.LogInformation("Restoring packages in {0}", dir);
                    exec.Create(DotNetCli.Default.Restore())
                        .WorkingDirectory(dir)
                        .Execute()
                        .EnsureSuccessful();
                }
                else
                {
                    log.LogTrace("Skipping non-existant directory {0}", dir);
                }
            }
        }
    }
}
