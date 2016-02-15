using DotNetDo.Helpers;
using Microsoft.Extensions.Logging;

namespace DotNetDo.BuildSystem.ManagedCode.DotNet
{
    public class DotNetBuildTask : TaskCollection
    {
        public static readonly string Name = nameof(BuildDotNetProjects);

        [Task(RunBefore = BuildLifecycle.CompileCore)]
        public void BuildDotNetProjects(ILogger log, DotNetBuildTaskOptions options, CommandHelper cmd, FileSystemHelper fs)
        {
            foreach(var project in fs.Files(options.ProjectGlobs))
            {
                log.LogTrace("Building {0}", project.FullName);
                cmd.Exec(DotNetCli.Default.Build(project.FullName));
            }
        }
    }
}