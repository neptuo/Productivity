using EnvDTE;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Processors
{
    [Export(typeof(IPathProcessor))]
    public class ProjectItemPathProcessor : IPathProcessor
    {
        [Import]
        public DTE Dte { get; }

        public bool TryRun(string path)
        {
            ProjectItem item = Dte.Solution.FindProjectItem(path);
            if (item == null)
                return false;

            Window window = item.Open();
            window.Activate();
            return true;
        }
    }
}
