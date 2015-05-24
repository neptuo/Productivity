using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.UI.Builds
{
    public class BuildHistorWindowCreated
    {
        public BuildHistoryWindow Window { get; private set; }

        public BuildHistorWindowCreated(BuildHistoryWindow window)
        {
            Ensure.NotNull(window, "window");
            Window = window;
        }
    }
}
