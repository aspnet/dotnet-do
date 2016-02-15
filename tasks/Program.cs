using DotNetDo;
using DotNetDo.BuildSystem;
using DotNetDo.BuildSystem.ManagedCode;

namespace tasks
{
    public class Program
    {
        public static int Main(string[] args) => TaskRunnerBuilder.CreateDefault()
            .UseBuildLifecycle()
            .UseDotNetRestore("src", "test")
            .Execute(args);
    }
}
