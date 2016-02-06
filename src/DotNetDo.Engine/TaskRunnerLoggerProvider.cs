using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console.Internal;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDo.Engine
{
    public class TaskRunnerLoggerProvider : ILoggerProvider
    {
        public static readonly int CategoryMaxLength = 10;
        public static readonly int StatusMaxLength = 4;
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly ConcurrentDictionary<string, TaskRunnerLogger> _loggers = new ConcurrentDictionary<string, TaskRunnerLogger>();
        private DateTime? _startTime = null;

        public TaskRunnerLoggerProvider(Func<string, LogLevel, bool> filter)
        {
            _filter = filter;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, s => new TaskRunnerLogger(this, s, _filter));
        }

        public void Dispose()
        {
        }

        private TimeSpan GetTimeOffset()
        {
            var time = DateTime.UtcNow;
            if (_startTime == null)
            {
                _startTime = time;
                return TimeSpan.Zero;
            }
            else
            {
                return time - _startTime.Value;
            }
        }

        private class TaskRunnerLogger : ILogger
        {
            private string _categoryName;
            private Func<string, LogLevel, bool> _filter;
            private IConsole _console;
            private readonly TaskRunnerLoggerProvider _provider;

            public TaskRunnerLogger(TaskRunnerLoggerProvider provider, string categoryName, Func<string, LogLevel, bool> filter)
            {
                _categoryName = categoryName;
                _filter = filter;
                _provider = provider;

                if(PlatformServices.Default.Runtime.OperatingSystem.Equals("Windows", StringComparison.OrdinalIgnoreCase))
                {
                    _console = new WindowsLogConsole();
                } else
                {
                    _console = new AnsiLogConsole(new AnsiSystemConsole());
                }
            }

            public IDisposable BeginScopeImpl(object state)
            {
                var action = state as TaskRunnerActivity;
                if(action != null)
                {
                    LogStartAction(action);
                    return new DisposableAction(() => LogEndAction(action));
                }
                else
                {
                    LogCore("START", "", state.ToString(), ConsoleColor.White, start: true);
                    return new DisposableAction(() => LogCore("STOP", "", state.ToString(), ConsoleColor.White, start: false));
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return _filter(_categoryName, logLevel);
            }

            public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                var categoryColor = ConsoleColor.White;
                var category = "LOG";
                switch (logLevel)
                {
                    case LogLevel.Debug:
                        categoryColor = ConsoleColor.Magenta;
                        category = "DEBUG";
                        break;
                    case LogLevel.Verbose:
                        categoryColor = ConsoleColor.Gray;
                        category = "TRACE";
                        break;
                    case LogLevel.Information:
                        categoryColor = ConsoleColor.Green;
                        category = "INFO";
                        break;
                    case LogLevel.Warning:
                        categoryColor = ConsoleColor.Yellow;
                        category = "WARNING";
                        break;
                    case LogLevel.Error:
                        categoryColor = ConsoleColor.Red;
                        category = "ERROR";
                        break;
                    case LogLevel.Critical:
                        categoryColor = ConsoleColor.Red;
                        category = "FATAL";
                        break;
                    default:
                        break;
                }
                LogCore(category, string.Empty, formatter(state, exception), categoryColor, start: null);
            }

            private void LogStartAction(TaskRunnerActivity action)
            {
                LogCore(action.Type, string.Empty, action.Name, categoryColor: ConsoleColor.Green, start: true);
            }

            private void LogEndAction(TaskRunnerActivity action)
            {
                var message = action.Name;
                if(!string.IsNullOrEmpty(action.Conclusion))
                {
                    message = $"{message} {action.Conclusion}";
                }
                LogCore(action.Type, action.Success ? "OK" : "FAIL", message, categoryColor: action.Success ? ConsoleColor.Green : ConsoleColor.Red, start: false);
            }

            private void LogCore(string category, string status, string message, ConsoleColor categoryColor, bool? start)
            {
                var startString = start == null ? " " : (start == true ? ">" : "<");
                _console.Write($"[{category.PadRight(CategoryMaxLength)}{startString}] ", background: null, foreground: categoryColor);
                _console.Write($"[{_provider.GetTimeOffset().ToString(@"hh\:mm\:ss\.ffffff")}] ", background: null, foreground: ConsoleColor.Blue);
                _console.Write($"[{status.PadRight(StatusMaxLength)}] ", background: null, foreground: ConsoleColor.Yellow);
                _console.WriteLine(message, background: null, foreground: null);
            }

            private class AnsiSystemConsole : IAnsiSystemConsole
            {
                public void Write(string message)
                {
                    System.Console.Write(message);
                }

                public void WriteLine(string message)
                {
                    System.Console.WriteLine(message);
                }
            }

            private class DisposableAction : IDisposable
            {
                private Action _act;

                public DisposableAction(Action act)
                {
                    _act = act;
                } 

                public void Dispose()
                {
                    _act();
                }
            }
        }
    }
}
