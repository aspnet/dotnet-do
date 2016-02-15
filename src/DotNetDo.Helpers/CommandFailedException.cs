using System;

namespace DotNetDo.Helpers
{
    public class CommandFailedException : Exception
    {
        public CommandResult Result { get; }

        public CommandFailedException(CommandResult result) : base(
            $"Command failed: {result.Command} (exit code: {result.ExitCode})")
        {
            Result = result;
        }
    }
}