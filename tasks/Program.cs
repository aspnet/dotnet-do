using DotNetDo;
using DotNetDo.BuildSystem;
using DotNetDo.BuildSystem.ManagedCode;

namespace tasks
{
    public class Program
    {
        public static int Main(string[] args) => TaskRunnerBuilder.CreateDefault()
            .UseStandardBuildSystem()
            .UseCleanDirectories("artifacts")
            .UseDotNetRestore("src", "test")
            .UseDotNetBuild("src/*/project.json")
            .UseDotNetPack("artifacts", "src/*/project.json", "!src/dotnet-do/project.json")
            .UseTasksFrom<InitializeTasks>()
            .Execute(args);
    }
}
