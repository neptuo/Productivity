using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Neptuo.Productivity.Processors
{
    [Export(typeof(IPathProcessor))]
    [Name(Name)]
    [Order(After = FileOpenPathProcessor.Name)]
    public class DirectoryExplorerPathProcessor : IPathProcessor
    {
        public const string Name = "Directory opener in File Explorer";
        
        public bool TryRun(string path)
        {

            if (Directory.Exists(path))
            {
                int result = VsShellUtilities.ShowMessageBox(
                    ServiceProvider.GlobalProvider,
                    $"Open path '{path}' in file explorer?",
                    "Go to Source",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST
                );

                if (result == 6)
                {
                    Process.Start(path);
                    return true;
                }
            }

            return false;
        }
    }
}
