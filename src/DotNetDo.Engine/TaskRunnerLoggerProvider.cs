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
        public static readonly int CategoryMaxLength = 7;
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
                    LogCore("START", "", state.ToString(), ConsoleColor.White, ConsoleColor.White, start: true);
                    return new DisposableAction(() => LogCore("STOP", "", state.ToString(), ConsoleColor.White, ConsoleColor.White, start: false));
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return _filter(_categoryName, logLevel);
            }

            public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
            {
                var categoryColor = ConsoleColor.White;
                var messageColor = ConsoleColor.White;
                var category = "LOG";
                switch (logLevel)
                {
                    case LogLevel.Debug:
                        categoryColor = ConsoleColor.Magenta;
                        messageColor = ConsoleColor.Magenta;
                        category = "DEBUG";
                        break;
                    case LogLevel.Verbose:
                        categoryColor = ConsoleColor.DarkGray;
                        messageColor = ConsoleColor.DarkGray;
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
                LogCore(category, string.Empty, formatter(state, exception), categoryColor, messageColor, start: null);
            }

            private void LogStartAction(TaskRunnerActivity action)
            {
                LogCore(action.Type, string.Empty, action.Name, categoryColor: ConsoleColor.Green, messageColor: ConsoleColor.White, start: true);
            }

            private void LogEndAction(TaskRunnerActivity action)
            {
                var message = action.Name;
                if(!string.IsNullOrEmpty(action.Conclusion))
                {
                    message = $"{message} {action.Conclusion}";
                }
                var categoryColor = action.Success ? ConsoleColor.Green : ConsoleColor.Red;
                var messageColor = action.Success ? ConsoleColor.White : ConsoleColor.Red;
                LogCore(action.Type, action.Success ? "OK" : "FAIL", message, categoryColor, messageColor, start: false);
            }

            private void LogCore(string category, string status, string message, ConsoleColor categoryColor, ConsoleColor messageColor, bool? start)
            {
                // Split the message by lines
                var startString = start == null ? " " : (start == true ? ">" : "<");
                foreach (var line in message.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    _console.Write($"[{category.PadRight(CategoryMaxLength)}{startString}] ", background: null, foreground: categoryColor);
                    _console.Write($"[{_provider.GetTimeOffset().ToString(@"hh\:mm\:ss\.ffffff")}] ", background: null, foreground: ConsoleColor.Blue);
                    _console.Write($"[{status.PadRight(StatusMaxLength)}] ", background: null, foreground: ConsoleColor.Yellow);
                    _console.WriteLine(line, background: null, foreground: messageColor);
                }
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
