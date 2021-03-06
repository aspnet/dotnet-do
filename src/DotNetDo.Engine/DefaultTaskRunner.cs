﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDo
{
    public class DefaultTaskRunner : ITaskRunner
    {
        public static readonly string DefaultTask = "Default";

        private readonly ILogger<DefaultTaskRunner> _log;
        private readonly ILoggerFactory _loggerFactory;
        private ITaskManager _tasks;

        public DefaultTaskRunner(ILogger<DefaultTaskRunner> log, ILoggerFactory loggerFactory, ITaskManager tasks)
        {
            _log = log;
            _tasks = tasks;
            _loggerFactory = loggerFactory;
        }

        public int Execute(IEnumerable<string> commandLineArgs)
        {
            try
            {
                var args = ParseArguments(commandLineArgs);

                _loggerFactory.AddProvider(new TaskRunnerLoggerProvider((s, l) => args.Verbose ? true : l >= LogLevel.Information));

                var tasks = args.Tasks;
                if (!tasks.Any())
                {
                    tasks = new[] { DefaultTask };
                }

                foreach (var task in tasks)
                {
                    var result = _tasks.ExecuteTask(task);
                    if(!result.Success)
                    {
                        _log.LogError("Task '{0}' failed: '{1}'", result.Task.Definition.Name, result.Exception.Message);
                        _log.LogTrace("Exception details: {0}", result.Exception);
                        return 1;
                    }
                }
                _log.LogInformation("All tasks completed!");
                return 0;
            }
            catch (Exception ex)
            {
                _log.LogError($"Error: {ex.ToString()}", ex);
                return 1;
            }
        }

        private Args ParseArguments(IEnumerable<string> commandLineArgs)
        {
            var args = new Args();
            var enumerator = commandLineArgs.GetEnumerator();
            while (enumerator.MoveNext()) // Using a while loop so we can advance the iterator when we hit args with values
            {
                var key = enumerator.Current;
                switch (key)
                {
                    case "-d":
                    case "--debug":
                        WaitForDebugger();
                        break;
                    case "-v":
                    case "--verbose":
                        args.Verbose = true;
                        break;
                    default:
                        // Not a switch, add it to the list of targets to run
                        args.Tasks.Add(key);
                        break;
                }
            }

            return args;
        }

        private void WaitForDebugger()
        {
            // Why are we using Console directly? Because it only makes sense to write this to the console and we need to read a line
            Console.WriteLine("Waiting for the debugger. Press ENTER to continue.");
            Console.WriteLine($"Process ID: {Process.GetCurrentProcess().Id}");
            Console.ReadLine();
        }

        private class Args
        {
            public IList<string> Tasks { get; } = new List<string>();
            public bool Verbose { get; internal set; }
        }
    }
}
