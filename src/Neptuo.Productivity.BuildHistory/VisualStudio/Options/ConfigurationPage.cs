using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.UI.ViewModels;
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
    public class ConfigurationPage : DialogPage, IQuickConfiguration
    {
        [DisplayName("Number of builds in build quick history overview.")]
        [Description("Defines number of builds that will be displayed in quick overview window.")]
        [DefaultValue(3)]
        public int QuickOverviewCount { get; set; }
    }
}
