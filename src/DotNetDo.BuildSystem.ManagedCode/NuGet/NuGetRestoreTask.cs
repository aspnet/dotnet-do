using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetDo.BuildSystem.ManagedCode.NuGet
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class NuGetRestoreTask : TaskCollection
    {
        [Task(RunBefore = BuildLifecycle.InitializeCore)]
        public void NuGetRestore(ILogger log, NuGetRestoreTaskOptions options)
        {
            foreach(var directory in options.DirectoriesToRestore)
            {
                log.LogInformation($"Restoring NuGet Packages in: {directory}");
            }
        }
    }
}
