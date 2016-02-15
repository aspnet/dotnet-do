using System.Collections.Generic;

namespace DotNetDo.BuildSystem
{
    public class CleanDirectoriesTaskOptions
    {
        public IEnumerable<string> Directories { get; }

        public CleanDirectoriesTaskOptions(IEnumerable<string> directories)
        {
            Directories = directories;
        }
    }
}