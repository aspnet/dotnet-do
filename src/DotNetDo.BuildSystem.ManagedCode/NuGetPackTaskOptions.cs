using System.Collections.Generic;

namespace DotNetDo.BuildSystem.ManagedCode
{
    public class NuGetPackTaskOptions
    {
        public string OutputRoot { get; } 
        public IEnumerable<string> ProjectGlobs { get; }

        public NuGetPackTaskOptions(string outputRoot, IEnumerable<string> projectGlobs)
        {
            OutputRoot = outputRoot;
            ProjectGlobs = projectGlobs;
        }
    }
}