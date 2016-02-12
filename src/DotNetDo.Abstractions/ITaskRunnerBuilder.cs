using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace DotNetDo
{
    public interface ITaskRunnerBuilder
    {
        ITaskRunnerBuilder UseServices(Action<IServiceCollection> registrar);
        ITaskRunner Build();
    }

    public static class TaskRunnerBuilderExtensions
    {
        public static int Execute(this ITaskRunnerBuilder self, IEnumerable<string> args) => self.Build().Execute(args);
        public static ITaskRunnerBuilder UseServices(this ITaskRunnerBuilder self, params ServiceDescriptor[] descriptors)
        {
            return self.UseServices(services =>
            {
                foreach (var descriptor in descriptors)
                {
                    services.Add(descriptor);
                }
            });
        }
    }
}
