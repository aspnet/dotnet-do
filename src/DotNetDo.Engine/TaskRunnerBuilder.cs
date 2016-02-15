using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddSingleton(CreateContext());

            // Maybe there's an easier way to do this? I just want to add LoggerFactory AND configure Console logging without
            // having to put the configuration in DefaultTaskRunner...
            var loggerFactory = new LoggerFactory();

            // TODO: Add a real filter
            loggerFactory.AddProvider(new TaskRunnerLoggerProvider((s, l) => true));

            services.AddSingleton<ILoggerFactory>(loggerFactory);
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
        }

        private static TaskRunContext CreateContext()
        {
            var doProjectPath = Environment.GetEnvironmentVariable("DOTNET_DO_PROJECT");
            if (string.IsNullOrEmpty(doProjectPath))
            {
                doProjectPath = InferProjectPath();
            }
            return new TaskRunContext(doProjectPath);
        }

        private static string InferProjectPath()
        {
#if NET451
            var appBase = AppDomain.CurrentDomain.BaseDirectory;
#else
            var appBase = AppContext.BaseDirectory;
#endif

            // Search up for a project.json file
            var candidate = appBase;
            while (candidate != null && !File.Exists(Path.Combine(candidate, "project.json")))
            {
                candidate = Path.GetDirectoryName(candidate);
            }
            if (candidate == null)
            {
                throw new FileNotFoundException($"Failed to locate 'project.json' file above app base: {appBase}. Try setting DOTNET_DO_PROJECT");
            }
            return Path.Combine(candidate, "project.json");
        }
    }
}
