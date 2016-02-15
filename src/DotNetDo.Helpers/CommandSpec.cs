using System.Collections.Generic;
using System.Linq;

namespace DotNetDo.Helpers
{
    public struct CommandSpec
    {
        public string Command { get; }
        public IEnumerable<string> Arguments { get; }

        // Naive argument escaping
        public string EscapedArguments => Arguments == null ? string.Empty : string.Join(" ", Arguments.Select(a => $"\"{a}\""));

        public CommandSpec(string command, IEnumerable<string> args) : this()
        {
            Command = command;
            Arguments = args ?? Enumerable.Empty<string>();
        }

        public override string ToString()
        {
            return $"{Command} {EscapedArguments}";
        }
    }
}
