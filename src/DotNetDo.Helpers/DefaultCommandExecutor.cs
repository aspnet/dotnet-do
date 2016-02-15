using System;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;

namespace DotNetDo.Helpers
{
    public class DefaultCommandExecutor : ICommandExecutor
    {
        public readonly ILogger _log;

        public DefaultCommandExecutor(ILogger<DefaultCommandExecutor> log)
        {
            _log = log;
        }

        public CommandResult Execute(CommandSpec spec, ProcessStartInfo startInfo, bool expectFailure)
        {
            var p = new Process();
            p.StartInfo = startInfo;

            StringBuilder stdout = new StringBuilder();
            if (p.StartInfo.RedirectStandardOutput)
            {
                p.OutputDataReceived += (sender, args) => stdout.AppendLine(args.Data);
            }

            StringBuilder stderr = new StringBuilder();
            if (p.StartInfo.RedirectStandardError)
            {
                p.ErrorDataReceived += (sender, args) => stderr.AppendLine(args.Data);
            }

            using (var activity = _log.BeginActivity("EXEC", FormatStartInfo(p.StartInfo)))
            {
                p.Start();

                if (p.StartInfo.RedirectStandardOutput)
                {
                    p.BeginOutputReadLine();
                }

                if (p.StartInfo.RedirectStandardError)
                {
                    p.BeginErrorReadLine();
                }

                p.WaitForExit();

                activity.Success = (!expectFailure && p.ExitCode == 0) || (expectFailure && p.ExitCode == 0);
                activity.Conclusion = FormatConclusion(p.ExitCode, activity.Success);

                return new CommandResult(spec, p.ExitCode, stdout.ToString(), stderr.ToString());
            }
        }

        private string FormatConclusion(int exitCode, bool success)
        {
            string expectation = "";
            if(!success && exitCode != 0)
            {
                expectation = " but was expected to exit with 0";
            }
            else if(!success && exitCode == 0)
            {
                expectation = " but was expected to return a non-zero exit code";
            }
            return $"exited with exit code {exitCode}{expectation}";
        }

        private string FormatStartInfo(ProcessStartInfo startInfo)
        {
            return $"{startInfo.FileName} {startInfo.Arguments}";
        }
    }
}
