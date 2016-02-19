using System.IO;
using System.Collections.Generic;
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

                var args = new List<string>();
                args.Add(project.FullName);
                args.Add("--output");
                args.Add(options.OutputRoot);
                args.Add("--no-build");
                if (!string.IsNullOrEmpty(options.VersionSuffix))
                {
                    args.Add("--version-suffix");
                    args.Add(options.VersionSuffix);
                }

                cmd.Exec(DotNetCli.Default.Pack(args));
            }
        }
    }
}
