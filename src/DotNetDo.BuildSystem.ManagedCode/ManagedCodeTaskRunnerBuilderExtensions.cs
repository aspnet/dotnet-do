using DotNetDo.BuildSystem.ManagedCode.NuGet;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDo.BuildSystem.ManagedCode
{
    public static class ManagedCodeTaskRunnerBuilderExtensions
    {
        public static ITaskRunnerBuilder UseNuGetRestore(this ITaskRunnerBuilder self) => UseNuGetRestore(self, new NuGetRestoreTaskOptions());

        public static ITaskRunnerBuilder UseNuGetRestore(this ITaskRunnerBuilder self, params string[] directoriesToRestore) => UseNuGetRestore(self, new NuGetRestoreTaskOptions(directoriesToRestore));

        public static ITaskRunnerBuilder UseNuGetRestore(this ITaskRunnerBuilder self, NuGetRestoreTaskOptions options)
        {
            return self.UseServices(
                ServiceDescriptor.Instance(options),
                ServiceDescriptor.Instance<ITaskProvider>(new NuGetRestoreTask()));
        }
    }
}
