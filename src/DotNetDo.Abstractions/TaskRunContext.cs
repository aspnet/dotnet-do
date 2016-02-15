using System.Collections.Generic;
using System.IO;

namespace DotNetDo
{
    public class TaskRunContext
    {
        public string DoProjectPath { get; }

        public string DoRoot => Path.GetDirectoryName(Path.GetDirectoryName(DoProjectPath));
        public string WorkingDirectory => Directory.GetCurrentDirectory();

        public IDictionary<string, object> Items { get; } = new Dictionary<string, object>();

        public TaskRunContext(string doProjectPath)
        {
            DoProjectPath = doProjectPath;
        }
    }
}
