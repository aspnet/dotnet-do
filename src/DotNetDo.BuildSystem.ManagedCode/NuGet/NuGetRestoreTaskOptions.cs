using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDo.BuildSystem.ManagedCode.NuGet
{
    public class NuGetRestoreTaskOptions
    {
        public IList<string> DirectoriesToRestore { get; }

        public NuGetRestoreTaskOptions() : this(new[] { "src", "test" }) { }

        public NuGetRestoreTaskOptions(IEnumerable<string> directoriesToRestore)
        {
            DirectoriesToRestore = directoriesToRestore.ToList();
        }
    }
}
