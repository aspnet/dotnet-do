using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotNetDo
{
    public static class LoggerActivityExtensions
    {
        public static TaskRunnerActivity BeginActivity(this ILogger self, string type, string name)
        {
            var activity = new TaskRunnerActivity(type, name);
            var scope = self.BeginScopeImpl(activity);
            activity.AttachToScope(scope);
            return activity;
        }
    }
}
