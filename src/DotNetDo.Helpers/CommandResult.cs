using System;

namespace DotNetDo.Helpers
{
    public struct CommandResult
    {
        public CommandSpec Command { get; }
        public int ExitCode { get; }
        public string StdOut { get; }
        public string StdErr { get; }

        public CommandResult(CommandSpec command, int exitCode, string stdout, string stderr) : this()
        {
            ExitCode = exitCode;
            StdOut = stdout;
            StdErr = stderr;
        }

        public void EnsureSuccessful()
        {
            if(ExitCode != 0)
            {
                throw new CommandFailedException(this);
            }
        }
    }
}