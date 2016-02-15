namespace DotNetDo.Helpers
{
    public class CommandHelper
    {
        private readonly ICommandExecutor _executor;

        public CommandHelper(ICommandExecutor executor)
        {
            _executor = executor;
        }
        
        public Command Create(CommandSpec spec)
        {
            return new Command(spec, _executor);
        }

        public Command Create(string command, params string[] args)
        {
            return Create(new CommandSpec(command, args));
        }

        public CommandResult Exec(CommandSpec spec)
        {
            var result = new Command(spec, _executor).Execute();
            result.EnsureSuccessful();
            return result;
        }

        public CommandResult Exec(string command, params string[] args)
        {
            return Exec(new CommandSpec(command, args));
        }
    }
}