using System.Collections.Generic;

namespace DotNetDo.BuildSystem.ManagedCode
{
    public class NuGetRestoreTaskOptions
    {
        public IEnumerable<string> TargetDirectories { get; }

        public NuGetRestoreTaskOptions(IEnumerable<string> targetDirectories)
        {
            TargetDirectories = targetDirectories;
        }
    }
}
