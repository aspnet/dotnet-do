using System.Collections.Generic;

namespace DotNetDo.BuildSystem.ManagedCode
{
    public class DotNetBuildTaskOptions
    {
        public IEnumerable<string> ProjectGlobs { get; }

        public DotNetBuildTaskOptions(IEnumerable<string> projectGlobs)
        {
            ProjectGlobs = projectGlobs;
        }
    }
}