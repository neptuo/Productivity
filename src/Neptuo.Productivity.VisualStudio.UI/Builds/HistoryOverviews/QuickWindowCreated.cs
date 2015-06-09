using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.UI.Builds.HistoryOverviews
{
    public class QuickWindowCreated
    {
        public QuickWindow Window { get; private set; }

        public QuickWindowCreated(QuickWindow window)
        {
            Ensure.NotNull(window, "window");
            Window = window;
        }
    }
}
