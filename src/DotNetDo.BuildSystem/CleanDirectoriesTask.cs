using System.Collections.Generic;
using System.Linq;
using DotNetDo.Helpers;

namespace DotNetDo.BuildSystem
{
    public class CleanDirectoriesTask : TaskCollection
    {
        // Take an IEnumerable<options> because if this task is registered multiple times it will result in multiple options
        // being present in the container and we want to get them all
        [Task(RunBefore = BuildLifecycle.CleanCore)]
        public void CleanDirectories(IEnumerable<CleanDirectoriesTaskOptions> optionses, FileSystemHelper fs)
        {
            foreach(var dir in optionses.SelectMany(o => o.Directories))
            {
                fs.RmRf(dir);
            }
        }
    }
}