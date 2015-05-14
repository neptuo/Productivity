using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Options
{
    public class IsUnderscoreNamespaceRemoverUsedChangedEvent
    {
        public bool NewValue { get; private set; }

        public IsUnderscoreNamespaceRemoverUsedChangedEvent(bool newValue)
        {
            NewValue = newValue;
        }
    }
}
