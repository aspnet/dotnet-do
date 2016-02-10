using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNetDo.Engine
{
    public class TaskRunnerBuilder
    {
        private ServiceCollection _services = new ServiceCollection();
        public TaskRunnerBuilder()
        {
        }

        public static TaskRunnerBuilder CreateDefault()
        {
            return new TaskRunnerBuilder().UseServices(ConfigureDefaultServices);
        }

        public TaskRunnerBuilder UseServices(Action<IServiceCollection> registrar)
        {
            registrar(_services);
            return this;
        }

        public int Execute(IEnumerable<string> commandLineArgs)
        {
            var serviceProvider = _services.BuildServiceProvider();
            var taskRunner = serviceProvider.GetRequiredService<ITaskRunner>();
            return taskRunner.Execute(commandLineArgs);
        }

        private static void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddSingleton<ITaskRunner, DefaultTaskRunner>();
            services.AddSingleton<ITaskManager, DefaultTaskManager>();
            services.AddInstance(new TaskContext());
        
            // Maybe there's an easier way to do this? I just want to add LoggerFactory AND configure Console logging without
            // having to put the configuration in DefaultTaskRunner...
            var loggerFactory = new LoggerFactory();

            // TODO: Add a real filter
            loggerFactory.AddProvider(new TaskRunnerLoggerProvider((s, l) => true));

            services.AddInstance<ILoggerFactory>(loggerFactory);
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }
    }
}
