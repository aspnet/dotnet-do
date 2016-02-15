using System.Diagnostics;

namespace DotNetDo.Helpers
{
    public interface ICommandExecutor
    {
        CommandResult Execute(CommandSpec spec, ProcessStartInfo startInfo, bool expectFailure);
    }
}