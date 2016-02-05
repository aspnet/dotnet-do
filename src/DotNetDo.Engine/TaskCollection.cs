using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNetDo.Engine
{
    public static class TaskCollectionTaskRunnerBuilderExtensions
    {
        public static TaskRunnerBuilder UseTasksFrom<T>(this TaskRunnerBuilder self) => UseTasksFrom(self, typeof(T));

        public static TaskRunnerBuilder UseAllTasksFromAssemblyContaining<T>(this TaskRunnerBuilder self) => UseAllTasksFromAssemblyContaining(self, typeof(T));

        public static TaskRunnerBuilder UseTasksFrom(this TaskRunnerBuilder self, Type type)
        {
            return self.UseServices(s => s.AddSingleton(typeof(ITaskProvider), type));
        }

        public static TaskRunnerBuilder UseAllTasksFromAssemblyContaining(this TaskRunnerBuilder self, Type type)
        {
            return self.UseServices(s =>
            {
                foreach (var exportedType in type.GetTypeInfo().Assembly.GetExportedTypes())
                {
                    if (exportedType.GetTypeInfo().BaseType.Equals(typeof(TaskCollection)))
                    {
                        s.AddSingleton(typeof(ITaskProvider), exportedType);
                    }
                }
            });
        }
    }

    public abstract class TaskCollection : ITaskProvider
    {
        public IEnumerable<TaskDefinition> GetTargets() => from m in GetType().GetMethods()
                                                           let attr = m.GetCustomAttribute<TaskAttribute>()
                                                           where attr != null
                                                           select CreateTask(m, attr);

        private TaskDefinition CreateTask(MethodInfo method, TaskAttribute attr)
        {
            return new TaskDefinition(
                name: string.IsNullOrEmpty(attr.Name) ? method.Name : attr.Name,
                source: $"{method.DeclaringType.FullName}.{method.Name}",
                dependsOn: attr.DependsOn,
                runBefore: attr.RunBefore,
                implementation: invocation =>
                {
                    var taskManager = invocation.Services.GetRequiredService<ITaskManager>();

                    var parameters = method.GetParameters();
                    var arguments = new object[parameters.Length];
                    for (var index = 0; index < parameters.Length; index++)
                    {
                        // Special case for logger, we give them a logger of the appropriate category
                        if (parameters[index].ParameterType.Equals(typeof(ILogger)))
                        {
                            var factory = invocation.Services.GetRequiredService<ILoggerFactory>();
                            arguments[index] = factory.CreateLogger(invocation.Task.Definition.Name);
                        }
                        else
                        {
                            arguments[index] = invocation.Services.GetService(parameters[index].ParameterType);
                        }
                    }

                    object result = null;
                    Exception exception = null;
                    bool success = true;
                    try
                    {
                        result = method.Invoke(this, arguments);
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        exception = ex;
                    }

                    return new TaskResult(invocation.Task, success, result, exception);
                });
        }
    }
}
