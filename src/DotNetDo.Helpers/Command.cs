using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotNetDo.Helpers
{
    public class Command
    {
        private readonly CommandSpec _spec;
        private readonly ICommandExecutor _executor;

        public ProcessStartInfo StartInfo { get; }
        public bool ExpectFailure { get; set; }

        public Command(CommandSpec spec, ICommandExecutor executor)
        {
            _spec = spec;
            _executor = executor;

            StartInfo = new ProcessStartInfo();
            StartInfo.FileName = spec.Command;

            // Naive argument escaping
            StartInfo.Arguments = spec.EscapedArguments;
        }

        public Command CaptureStdErr()
        {
            StartInfo.RedirectStandardError = true;
            return this;
        }

        public Command CaptureStdOut()
        {
            StartInfo.RedirectStandardOutput = true;
            return this;
        }

        public Command WorkingDirectory(string dir)
        {
            StartInfo.WorkingDirectory = dir;
            return this;
        }

        public Command ShouldFail()
        {
            ExpectFailure = true;
            return this;
        }

        public Command Env(string name, string value)
        {
#if NET451
            StartInfo.EnvironmentVariables[name] = value;
#else
            StartInfo.Environment[name] = value;
#endif
            return this;
        }

        public Command Env(IEnumerable<KeyValuePair<string, string>> values)
        {
            foreach (var pair in values)
            {
                Env(pair.Key, pair.Value);
            }
            return this;
        }

        public CommandResult Execute()
        {
            return _executor.Execute(_spec, StartInfo, ExpectFailure);
        }
    }

}
