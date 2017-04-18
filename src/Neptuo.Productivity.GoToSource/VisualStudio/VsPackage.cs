using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.Commands;
using Neptuo.Productivity.VisualStudio.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    /// <summary>
    /// VS package.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration(ProductInfo.Name, ProductInfo.Description, VersionInfo.Version, IconResourceID = 400)]
    [Guid(PackageGuids.PackageString)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(ConfigurationPage), "Neptuo Productivity", "Go To Source", 0, 0, true)]
    public class VsPackage : Package
    {
        private static VsPackage instance;

        public static VsPackage Instance
        {
            get { return instance; }
        }

        protected override void Initialize()
        {
            base.Initialize();

            DTE dte = (DTE)GetService(typeof(DTE));
            IMenuCommandService commandService = (IMenuCommandService)GetService(typeof(IMenuCommandService));
            GoToSourceCommand.Initialize(this, dte, commandService);
        }

        public ConfigurationPage GetConfiguration()
        {
            return (ConfigurationPage)GetDialogPage(typeof(ConfigurationPage));
        }
    }
}
