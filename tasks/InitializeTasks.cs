using System;
using DotNetDo;
using DotNetDo.BuildSystem;
using DotNetDo.BuildSystem.ManagedCode;

namespace tasks
{
    public class InitializeTasks : TaskCollection
    {
        [Task(RunBefore = BuildLifecycle.InitializeCore)]
        public void SetBuildVersion(DotNetBuildTaskOptions buildOptions)
        {
            var appveyorBuildNumber = Environment.GetEnvironmentVariable("APPVEYOR_BUILD_NUMBER");
            if (!string.IsNullOrEmpty(appveyorBuildNumber))
            {
                var buildNumber = int.Parse(appveyorBuildNumber);
                buildOptions.VersionSuffix = $"alpha-{buildNumber:0000}";
            }
        }
    }
}
