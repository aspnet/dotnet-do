using Microsoft.Extensions.DependencyInjection;

namespace DotNetDo.Helpers
{
    public static class TaskRunnerBuilderExtensions
    {
        public static ITaskRunnerBuilder UseDefaultHelpers(this ITaskRunnerBuilder self)
        {
            return self.UseServices(
                ServiceDescriptor.Singleton<ICommandExecutor, DefaultCommandExecutor>(),
                ServiceDescriptor.Singleton<FileSystemHelper, FileSystemHelper>(),
                ServiceDescriptor.Singleton<CommandHelper, CommandHelper>());
        }
    }
}
