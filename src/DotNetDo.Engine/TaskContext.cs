using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDo.Engine
{
    public class TaskContext
    {
        public string TasksProjectPath { get; }
        public string WorkingDirectory { get; }
        public string ProjectDirectory => Path.GetDirectoryName(Path.GetDirectoryName(TasksProjectPath));

        public TaskContext()
        {
            WorkingDirectory = Directory.GetCurrentDirectory();
            TasksProjectPath = Environment.GetEnvironmentVariable("DOTNET_DO_PROJECT");
        }
    }
}
