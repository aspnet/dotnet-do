using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.PlatformAbstractions;
using DotNetDo.Helpers;
using System.Linq;

namespace DotNetDo
{
    public class TaskRunnerBuilder : ITaskRunnerBuilder
    {
        private ServiceCollection _services = new ServiceCollection();
        public TaskRunnerBuilder()
        {
        }

        public static ITaskRunnerBuilder CreateDefault()
        {
            return new TaskRunnerBuilder().UseServices(ConfigureDefaultServices);
        }

        public ITaskRunnerBuilder UseServices(Action<IServiceCollection> registrar)
        {
            registrar(_services);
            return this;
        }

        public bool HasService(Type serviceType)
        {
            return _services.Any(s => s.ServiceType.Equals(serviceType));
        }

        public ITaskRunner Build()
        {
            var serviceProvider = _services.BuildServiceProvider();
            var taskRunner = serviceProvider.GetRequiredService<ITaskRunner>();
            return taskRunner;
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddSingleton<ITaskRunner, DefaultTaskRunner>();
            services.AddSingleton<ITaskManager, DefaultTaskManager>();
            services.AddSingleton(new TaskContext());
        
            // Maybe there's an easier way to do this? I just want to add LoggerFactory AND configure Console logging without
            // having to put the configuration in DefaultTaskRunner...
            var loggerFactory = new LoggerFactory();

            // TODO: Add a real filter
            loggerFactory.AddProvider(new TaskRunnerLoggerProvider((s, l) => true));

            services.AddSingleton<ILoggerFactory>(loggerFactory);
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.UseDefaultHelpers();
        }
    }
}
