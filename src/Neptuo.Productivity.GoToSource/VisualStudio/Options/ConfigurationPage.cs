using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class ConfigurationPage : DialogPage
    {
        [DisplayName("Use standard Go to Definition if path not found.")]
        [Description("Enable this feature to replace standard GoToDefinition with GoToSource. If enabled, standard GoToDefinition is executed if processing as path not succeeded.")]
        public bool IsGoToDefinitionExecuted { get; set; }

        [DisplayName("Open directory in FileExplorer.")]
        [Description("If enabled, opens paths to directories in FileExplorer.")]
        [DefaultValue(DirectoryBrowserState.Ask)]
        public DirectoryBrowserState DirectoryBrowser { get; set; }
    }
}
