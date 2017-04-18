using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Utilities;
using Neptuo.Productivity.VisualStudio;
using Neptuo.Productivity.VisualStudio.Options;
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
            ConfigurationPage configuration = VsPackage.Instance.GetConfiguration();
            if (configuration.DirectoryBrowser == DirectoryBrowserState.Disabled)
                return false;

            if (Directory.Exists(path))
            {
                if (configuration.DirectoryBrowser == DirectoryBrowserState.Ask)
                {
                    int result = VsShellUtilities.ShowMessageBox(
                        ServiceProvider.GlobalProvider,
                        $"Open path '{path}' in file explorer?",
                        "Go to Source",
                        OLEMSGICON.OLEMSGICON_INFO,
                        OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST
                    );

                    if (result != 6)
                        return false;
                }

                Process.Start(path);
                return true;
            }

            return false;
        }
    }
}
