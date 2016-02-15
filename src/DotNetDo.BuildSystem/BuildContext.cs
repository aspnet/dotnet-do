using System.Collections.Generic;

namespace DotNetDo.BuildSystem
{
    // TODO: This is eventually going to want some extra stuff
    public class BuildContext
    {
        public TaskRunContext TaskRunContext { get; }

        public string ProjectRoot => TaskRunContext.DoRoot;
        public IDictionary<string, object> Items => TaskRunContext.Items;

        public BuildContext(TaskRunContext taskRunContext)
        {
            TaskRunContext = taskRunContext;
        }
    }
}
