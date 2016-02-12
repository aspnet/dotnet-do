using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetDo
{
    public interface ITaskRunnerBuilder
    {
        ITaskRunnerBuilder UseServices(Action<IServiceCollection> registrar);
    }
}
