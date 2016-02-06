using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DotNetDo.Engine
{
    public class DefaultTaskManager : ITaskManager
    {
        private readonly ILogger<DefaultTaskManager> _log;
        private readonly IServiceProvider _services;

        public IReadOnlyDictionary<string, TaskNode> Tasks { get; }
        
        public DefaultTaskManager(ILogger<DefaultTaskManager> log, IServiceProvider services, IEnumerable<ITaskProvider> taskProviders)
        {
            _log = log;
            _services = services;

            var tasks = RegisterTasks(taskProviders.SelectMany(p => p.GetTargets()));
            Tasks = new ReadOnlyDictionary<string, TaskNode>(tasks);
        }

        private IDictionary<string, TaskNode> RegisterTasks(IEnumerable<TaskDefinition> tasks)
        {
            var graph = new Dictionary<string, TaskNode>(StringComparer.OrdinalIgnoreCase);
            foreach (var task in tasks)
            {
                TaskNode oldTask;
                if (graph.TryGetValue(task.Name, out oldTask))
                {
                    _log.LogVerbose("Overriding task {0} defined in {1} by the definition in {2}", task.Name, oldTask.Definition.Source, task.Source);
                }

                graph[task.Name] = new TaskNode(task);
            }

            // Need a second pass to wire up the "RunBefore"s and stich up the graph
            foreach(var task in graph.Values)
            {
                foreach(var dep in task.Definition.DependsOn)
                {
                    TaskNode dependencyNode;
                    if(!graph.TryGetValue(dep, out dependencyNode))
                    {
                        throw new KeyNotFoundException($"Task '{task.Definition.Name}' depends on '{dep}', which does not exist.");
                    }
                    task.Dependencies.Add(dependencyNode);
                }

                if(!string.IsNullOrEmpty(task.Definition.RunBefore))
                {
                    TaskNode target;
                    if(!graph.TryGetValue(task.Definition.RunBefore, out target))
                    {
                        throw new InvalidOperationException($"Task '{task.Definition.Name}' is requesting to run before '{task.Definition.RunBefore}', which does not exist");
                    }

                    // Add in to the graph as a dependency of the specified target
                    target.Dependencies.Add(task);
                }
            }
            return graph;
        }

        public TaskResult ExecuteTask(string taskName)
        {
            TaskNode task;
            if (!Tasks.TryGetValue(taskName, out task))
            {
                throw new KeyNotFoundException($"Unknown task: {taskName}");
            }

            return ExecuteTask(task, ChainPredicate(_ => false, task.Definition.Name));
        }

        private TaskResult ExecuteTask(TaskNode task, Func<string, bool> circularDependencyGuard)
        {
            var activity = new TaskRunnerActivity("TASK", task.Definition.Name);
            using (_log.BeginScopeImpl(activity))
            {
                // Run dependencies
                TaskResult result;
                var dependencyResults = new Dictionary<string, TaskResult>();
                foreach (var dependency in task.Dependencies)
                {
                    if (circularDependencyGuard(dependency.Definition.Name))
                    {
                        throw new InvalidOperationException($"Circular dependency detected at '{dependency.Definition.Name}'");
                    }

                    result = ExecuteTask(dependency, ChainPredicate(circularDependencyGuard, dependency.Definition.Name));
                    if (!result.Success)
                    {
                        return result;
                    }
                    dependencyResults[dependency.Definition.Name] = result;
                }

                // Run the task itself
                result = task.Execute(new TaskInvocation(task, _services, dependencyResults));
                activity.Success = result.Success;
                return result;
            }
        }

        private Func<string, bool> ChainPredicate(Func<string, bool> circularDependencyGuard, string name)
        {
            return other =>
                string.Equals(name, other, StringComparison.OrdinalIgnoreCase) ||
                circularDependencyGuard(other);
        }
    }
}