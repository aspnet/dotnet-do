using System.Collections.Generic;

namespace DotNetDo.Engine
{
    public interface ITaskRunner
    {
        int Execute(IEnumerable<string> commandLineArgs);
    }
}