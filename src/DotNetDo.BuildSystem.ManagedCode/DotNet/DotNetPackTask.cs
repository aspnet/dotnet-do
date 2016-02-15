using System.IO;
using System.Threading.Tasks;
using DotNetDo.Helpers;
using Microsoft.Extensions.Logging;

namespace DotNetDo.BuildSystem.ManagedCode.DotNet
{
    public class DotNetPackTask : TaskCollection
    {
        public static readonly string Name = nameof(PackNuGetPackages);

        [Task(RunBefore = BuildLifecycle.PackageCore)]
        public void PackNuGetPackages(ILogger log, NuGetPackTaskOptions options, CommandHelper cmd, FileSystemHelper fs)
        {
            foreach(var project in fs.Files(options.ProjectGlobs))
            {
                log.LogTrace("Packing {0}", project.FullName);
                cmd.Exec(DotNetCli.Default.Pack(project.FullName, 
                    "--output", options.OutputRoot,
                    "--no-build"));
            }
        }
    }
}
