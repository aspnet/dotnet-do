using DotNetDo.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetDo.BuildSystem
{
    public static class BuildLifecycleTaskRunnerBuilderExtensions
    {
        public static ITaskRunnerBuilder UseStandardBuildSystem(this ITaskRunnerBuilder self)
        {
            return self.UseServices(ServiceDescriptor.Singleton<ITaskProvider, BuildLifecycleTaskProvider>())
                .UseDefaultHelpers();
        }

        public static ITaskRunnerBuilder UseCleanDirectories(this ITaskRunnerBuilder self, params string[] directories)
        {
            return self.UseServices(
                ServiceDescriptor.Singleton<ITaskProvider, CleanDirectoriesTask>(),
                ServiceDescriptor.Singleton(new CleanDirectoriesTaskOptions(directories)));
        }
    }
}
