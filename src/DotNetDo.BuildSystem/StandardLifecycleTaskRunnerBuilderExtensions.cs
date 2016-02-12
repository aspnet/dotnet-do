using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDo.BuildSystem
{
    public static class StandardLifecycleTaskRunnerBuilderExtensions
    {
        public static ITaskRunnerBuilder UseStandardLifecycle(this ITaskRunnerBuilder self)
        {
            self.UseServices(s => s.Add(ServiceDescriptor.Instance(typeof(ITaskProvider), new StandardLifecycleTaskProvider())));
        }
    }
}
