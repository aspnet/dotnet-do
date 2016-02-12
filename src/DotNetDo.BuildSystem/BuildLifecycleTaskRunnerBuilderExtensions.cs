using Microsoft.Extensions.DependencyInjection;

namespace DotNetDo.BuildSystem
{
    public static class BuildLifecycleTaskRunnerBuilderExtensions
    {
        public static ITaskRunnerBuilder UseBuildLifecycle(this ITaskRunnerBuilder self)
        {
            return self.UseServices(s => s.Add(ServiceDescriptor.Instance(typeof(ITaskProvider), new BuildLifecycleTaskProvider())));
        }
    }
}
