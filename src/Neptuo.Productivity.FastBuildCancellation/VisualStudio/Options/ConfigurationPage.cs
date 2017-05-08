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
        [DisplayName("On window after build is cancelled")]
        [Description("Opens selected window after (any) build is cancelled.")]
        public BuildCancelWindow OpenWindowAfterBuildCancel { get; set; }
    }
}
