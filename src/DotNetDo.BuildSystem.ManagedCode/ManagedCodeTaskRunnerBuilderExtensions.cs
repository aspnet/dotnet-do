using System.Linq;
using DotNetDo.BuildSystem.ManagedCode.DotNet;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetDo.BuildSystem.ManagedCode
{
    public static class ManagedCodeTaskRunnerBuilderExtensions
    {
        public static ITaskRunnerBuilder UseDotNetRestore(this ITaskRunnerBuilder self) => UseDotNetRestore(self, new NuGetRestoreTaskOptions(Enumerable.Empty<string>()));

        public static ITaskRunnerBuilder UseDotNetRestore(this ITaskRunnerBuilder self, params string[] directoriesToRestore) => UseDotNetRestore(self, new NuGetRestoreTaskOptions(directoriesToRestore));

        public static ITaskRunnerBuilder UseDotNetRestore(this ITaskRunnerBuilder self, NuGetRestoreTaskOptions options)
        {
            self.UseDotNetCliCore();
            return self.UseServices(
                ServiceDescriptor.Singleton(options),
                ServiceDescriptor.Singleton<ITaskProvider>(new DotNetRestoreTask()));
        }

        private static void UseDotNetCliCore(this ITaskRunnerBuilder self)
        {
            if(!self.HasService(typeof(DotNetCliCoreTasks)))
            {
                self.UseServices(ServiceDescriptor.Singleton<ITaskProvider>(new DotNetCliCoreTasks()));
            }
        }
    }
}
