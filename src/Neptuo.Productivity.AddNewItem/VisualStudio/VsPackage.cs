using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.Commands;
using Neptuo.Productivity.VisualStudio.Views;
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
    [ProvideToolWindow(typeof(AddNewItemWindow), Width = 400, Height = 200)]
    public class VsPackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();

            IMenuCommandService commandService = (IMenuCommandService)GetService(typeof(IMenuCommandService));
            AddNewItemCommand.Initialize(this, commandService);
        }
    }
}
