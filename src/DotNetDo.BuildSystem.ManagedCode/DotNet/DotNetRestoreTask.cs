using System.IO;
using System.Threading.Tasks;
using DotNetDo.Helpers;
using Microsoft.Extensions.Logging;

namespace DotNetDo.BuildSystem.ManagedCode.DotNet
{
    public class DotNetRestoreTask : TaskCollection
    {
        public static readonly string Name = nameof(RestoreNuGetPackages);

        [Task(RunBefore = BuildLifecycle.InitializeCore)]
        public void RestoreNuGetPackages(ILogger log, NuGetRestoreTaskOptions options, CommandHelper cmd)
        {
            foreach (var dir in options.TargetDirectories)
            {
                if (Directory.Exists(dir))
                {
                    log.LogTrace("Restoring packages in {0}", dir);
                    cmd.Create(DotNetCli.Default.Restore())
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
