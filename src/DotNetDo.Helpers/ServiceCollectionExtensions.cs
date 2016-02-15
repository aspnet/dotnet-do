using Microsoft.Extensions.DependencyInjection;

namespace DotNetDo.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static void UseDefaultHelpers(this IServiceCollection self)
        {
            self.AddSingleton<ICommandExecutor, DefaultCommandExecutor>();
            self.AddSingleton<CommandHelper>();
        }
    }
}
