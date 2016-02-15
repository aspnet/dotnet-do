using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DotNetDo
{
    public class DefaultTaskManager : ITaskManager
    {
        private readonly ILogger<DefaultTaskManager> _log;
        private readonly IServiceProvider _services;

        private readonly IDictionary<string, TaskResult> _results = new Dictionary<string, TaskResult>();

        public IReadOnlyDictionary<string, TaskNode> Tasks { get; }
        public IReadOnlyDictionary<string, TaskResult> Results { get; }
        
        public DefaultTaskManager(ILogger<DefaultTaskManager> log, IServiceProvider services, IEnumerable<ITaskProvider> taskProviders)
        {
            _log = log;
            _services = services;

            var tasks = RegisterTasks(taskProviders.SelectMany(p => p.GetTasks()));
            Tasks = new ReadOnlyDictionary<string, TaskNode>(tasks);
            Results = new ReadOnlyDictionary<string, TaskResult>(_results);
        }

        private IDictionary<string, TaskNode> RegisterTasks(IEnumerable<TaskDefinition> tasks)
        {
            var graph = new Dictionary<string, TaskNode>(StringComparer.OrdinalIgnoreCase);
            foreach (var task in tasks)
            {
                TaskNode oldTask;
                if (graph.TryGetValue(task.Name, out oldTask))
                {
                    _log.LogTrace("Overriding task {0} defined in {1} by the definition in {2}", task.Name, oldTask.Definition.Source, task.Source);
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
            if(!task.Dependencies.Any() && task.Definition.Implementation == null)
            {
                _log.LogTrace($"Skipping empty task: {task.Definition.Name}");
                return new TaskResult(task, success: true, returnValue: null, exception: null);
            }

            using (var activity = _log.BeginActivity("TASK", task.Definition.Name))
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
                result = ExecuteTask(new TaskInvocation(task, _services, dependencyResults));

                activity.Success = result.Success;

                if (result.Exception != null)
                {
                    activity.Conclusion = $"failed with {result.Exception.GetType().Name}: {result.Exception.Message}";
                }

                return result;
            }
        }

        private TaskResult ExecuteTask(TaskInvocation taskInvocation)
        {
            TaskResult result;
            if (_results.TryGetValue(taskInvocation.Task.Definition.Name, out result))
            {
                _log.LogTrace("Returning cached result for task: {0}", taskInvocation.Task.Definition.Name);
                return result;
            }

            var implementation = taskInvocation.Task.Definition.Implementation;
            if (implementation == null)
            {
                _log.LogTrace("Skipping empty task: {0}", taskInvocation.Task.Definition.Name);
                result = new TaskResult(taskInvocation.Task, success: true, returnValue: null, exception: null);
            }
            else
            {
                _log.LogTrace("Executing task: {0}", taskInvocation.Task.Definition.Name);
                result = implementation(taskInvocation);
            }
            _results[taskInvocation.Task.Definition.Name] = result;
            return result;
        }

        private Func<string, bool> ChainPredicate(Func<string, bool> circularDependencyGuard, string name)
        {
            return other =>
                string.Equals(name, other, StringComparison.OrdinalIgnoreCase) ||
                circularDependencyGuard(other);
        }
    }
}