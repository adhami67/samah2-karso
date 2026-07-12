using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRIT.Framework.Utility.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActivityLogAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
