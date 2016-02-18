using System.Collections.Generic;

namespace DotNetDo.BuildSystem.ManagedCode
{
    public class DotNetBuildTaskOptions
    {
        public IEnumerable<string> ProjectGlobs { get; set; }
        public string VersionSuffix { get; set; }

        public DotNetBuildTaskOptions(IEnumerable<string> projectGlobs)
        {
            ProjectGlobs = projectGlobs;
        }
    }
}
