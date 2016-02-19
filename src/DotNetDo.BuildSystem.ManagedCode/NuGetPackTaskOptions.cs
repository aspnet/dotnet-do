using System.Collections.Generic;

namespace DotNetDo.BuildSystem.ManagedCode
{
    public class NuGetPackTaskOptions
    {
        public string OutputRoot { get; set; }
        public string VersionSuffix { get; set; }
        public IEnumerable<string> ProjectGlobs { get; set; }

        public NuGetPackTaskOptions(string outputRoot, IEnumerable<string> projectGlobs)
        {
            OutputRoot = outputRoot;
            ProjectGlobs = projectGlobs;
        }
    }
}
