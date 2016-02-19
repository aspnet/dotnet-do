using System;
using System.Collections.Generic;
using DotNetDo.Helpers;
using Microsoft.Extensions.Logging;

namespace DotNetDo.BuildSystem.ManagedCode.DotNet
{
    public class DotNetBuildTask : TaskCollection
    {
        public static readonly string Name = nameof(BuildDotNetProjects);

        [Task(RunBefore = BuildLifecycle.PostInitialize)]
        public void SetVersionSuffix(ILogger log, DotNetBuildTaskOptions buildOptions, NuGetPackTaskOptions packOptions)
        {
            if (string.IsNullOrEmpty(buildOptions.VersionSuffix))
            {
                var versionSuffix = Environment.GetEnvironmentVariable("DOTNET_BUILD_VERSION");
                if (!string.IsNullOrEmpty(versionSuffix))
                {
                    buildOptions.VersionSuffix = versionSuffix;
                }
            }

            if (string.IsNullOrEmpty(buildOptions.VersionSuffix))
            {
                // Generate a timestamp based version
                var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                buildOptions.VersionSuffix = $"t-{timestamp}";
            }

            packOptions.VersionSuffix = buildOptions.VersionSuffix;
            log.LogInformation($"Version Suffix: {buildOptions.VersionSuffix}");
        }

        [Task(RunBefore = BuildLifecycle.CompileCore)]
        public void BuildDotNetProjects(ILogger log, DotNetBuildTaskOptions options, CommandHelper cmd, FileSystemHelper fs)
        {
            foreach(var project in fs.Files(options.ProjectGlobs))
            {
                log.LogTrace("Building {0}", project.FullName);

                var args = new List<string>();
                args.Add(project.FullName);
                if (!string.IsNullOrEmpty(options.VersionSuffix))
                {
                    args.Add("--version-suffix");
                    args.Add(options.VersionSuffix);
                }
                cmd.Exec(DotNetCli.Default.Build(args));
            }
        }
    }
}
