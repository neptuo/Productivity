using Neptuo.Models.Keys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Events
{
    public class BuildStarted : IBuildKeyAware
    {
        public Int32Key Key { get; private set; }
        public BuildScope Scope { get; private set; }
        public BuildType Action { get; private set; }
        public DateTime StartedAt { get; private set; }

        public BuildStarted(Int32Key key, BuildScope scope, BuildType action, DateTime startedAt)
        {
            Ensure.Condition.NotEmptyKey(key);
            Key = key;
            Scope = scope;
            Action = action;
            StartedAt = startedAt;
        }
    }
}
