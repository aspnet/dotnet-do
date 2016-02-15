using System.Linq;
using DotNetDo.BuildSystem.ManagedCode.DotNet;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetDo.BuildSystem.ManagedCode
{
    public static class ManagedCodeTaskRunnerBuilderExtensions
    {
        public static ITaskRunnerBuilder UseDotNetPack(this ITaskRunnerBuilder self) => UseDotNetPack(self, new NuGetPackTaskOptions("artifacts", new[] { "src/*/project.json" }));

        public static ITaskRunnerBuilder UseDotNetPack(this ITaskRunnerBuilder self, string outputPath, params string[] projectGlobs) => UseDotNetPack(self, new NuGetPackTaskOptions(outputPath, projectGlobs));

        public static ITaskRunnerBuilder UseDotNetPack(this ITaskRunnerBuilder self, NuGetPackTaskOptions options)
        {
            self.UseDotNetCliCore();
            return self.UseServices(
                ServiceDescriptor.Singleton(options),
                ServiceDescriptor.Singleton<ITaskProvider>(new DotNetPackTask()));
        }

        public static ITaskRunnerBuilder UseDotNetBuild(this ITaskRunnerBuilder self) => UseDotNetBuild(self, new DotNetBuildTaskOptions(new[] { "src/*/project.json" }));

        public static ITaskRunnerBuilder UseDotNetBuild(this ITaskRunnerBuilder self, params string[] projectGlobs) => UseDotNetBuild(self, new DotNetBuildTaskOptions(projectGlobs));

        public static ITaskRunnerBuilder UseDotNetBuild(this ITaskRunnerBuilder self, DotNetBuildTaskOptions options)
        {
            self.UseDotNetCliCore();
            return self.UseServices(
                ServiceDescriptor.Singleton(options),
                ServiceDescriptor.Singleton<ITaskProvider>(new DotNetBuildTask()));
        }

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
            if (!self.HasService<DotNetCliCoreTasks>())
            {
                self.UseServices(ServiceDescriptor.Singleton<ITaskProvider>(new DotNetCliCoreTasks()));
            }
        }
    }
}
