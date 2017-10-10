using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration(ProductInfo.Name, ProductInfo.Description, VersionInfo.Version, IconResourceID = 400)]
    [Guid(PackageGuids.PackageString)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public class VsPackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();

            DTE dte = (DTE)GetService(typeof(DTE));
            IMenuCommandService commandService = (IMenuCommandService)GetService(typeof(IMenuCommandService));
            AddNewItemCommand.Initialize(dte, commandService);
        }
    }
}
