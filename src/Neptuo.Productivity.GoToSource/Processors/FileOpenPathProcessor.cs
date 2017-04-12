using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Processors
{
    [Export(typeof(IPathProcessor))]
    [Name(ProcessorName.FileOpen)]
    [Order(After = ProcessorName.ProjectItem)]
    public class FileOpenPathProcessor : IPathProcessor
    {
        public bool TryRun(string path)
        {
            if (Path.IsPathRooted(path))
            {
                DTE dte = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
                dte.ExecuteCommand("File.OpenFile", path);
                return true;
            }

            return false;
        }
    }
}
