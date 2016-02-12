using System.Collections.Generic;

namespace DotNetDo
{
    public interface ITaskRunner
    {
        int Execute(IEnumerable<string> commandLineArgs);
    }
}