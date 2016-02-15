using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
