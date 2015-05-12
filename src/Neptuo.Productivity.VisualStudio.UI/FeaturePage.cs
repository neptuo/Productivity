using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.VisualStudio.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.UI
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class FeaturePage : DialogPage, IConfiguration
    {
        private bool isUnderscoreNamespaceRemoverUsed;

        [Category(MyConstants.Feature.FriendlyNamespaces)]
        [DisplayName("Use namespace underscore remover.")]
        [Description("Removes parts of the C# namespace that starts with '_'.")]
        public bool IsUnderscoreNamespaceRemoverUsed
        {
            get { return isUnderscoreNamespaceRemoverUsed; }
            set { isUnderscoreNamespaceRemoverUsed = value; }
        }

        public FeaturePage()
        {

        }
    }
}
