using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Processors
{
    [Export(typeof(IPathProcessor))]
    [Name(ProcessorName.ProjectItem)]
    public class ProjectItemPathProcessor : IPathProcessor
    {
        public bool TryRun(string path)
        {
            DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            ProjectItem item = dte.Solution.FindProjectItem(path);
            if (item == null)
                return false;

            Window window = item.Open();
            window.Activate();
            return true;
        }
    }
}
