using DotNetDo;
using DotNetDo.BuildSystem;
using DotNetDo.BuildSystem.ManagedCode;

namespace DevSandbox
{
    public class Program
    {
        public static int Main(string[] args) => TaskRunnerBuilder.CreateDefault()
            .UseBuildLifecycle()
            .UseNuGetRestore()
            .Execute(args);
    }
}
