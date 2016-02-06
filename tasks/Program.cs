using DotNetDo.Engine;

namespace DevSandbox
{
    public class Program
    {
        public static int Main(string[] args) => TaskRunnerBuilder.CreateDefault()
            .UseAllTasksFromAssemblyContaining<Program>()
            .Execute(args);
    }
}
