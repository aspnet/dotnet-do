using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDo.Engine
{
    public class TaskAttribute : Attribute
    {
        public string Name { get; set; }

        public string RunBefore { get; set; }

        public IEnumerable<string> DependsOn { get; set; }

        public TaskAttribute()
        {
            DependsOn = Enumerable.Empty<string>();
        }

        public TaskAttribute(params string[] dependsOn)
        {
            DependsOn = dependsOn;
        }
    }
}
