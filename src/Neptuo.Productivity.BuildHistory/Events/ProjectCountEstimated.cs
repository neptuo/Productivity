using Neptuo;
using Neptuo.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Events
{
    public class ProjectCountEstimated : IBuildKeyAware
    {
        public Int32Key Key { get; private set; }
        public int EstimatedProjectCount { get; private set; }

        public ProjectCountEstimated(Int32Key key, int estimatedProjectCount)
        {
            Ensure.Condition.NotEmptyKey(key);
            Ensure.Positive(estimatedProjectCount, "estimatedProjectCount");
            Key = key;
            EstimatedProjectCount = estimatedProjectCount;
        }
    }
}
