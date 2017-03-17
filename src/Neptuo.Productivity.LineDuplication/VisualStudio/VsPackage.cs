using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.Commands;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace Neptuo.Productivity.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("Neptuo.Productivity.LineDuplication", "Productivity tools by Neptuo: Line duplication.", "1.0", IconResourceID = 400)]
    [Guid(MyConstants.PackageString)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed partial class VsPackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();

            DTE dte = (DTE)GetService(typeof(DTE));
            IMenuCommandService commandService = (IMenuCommandService)GetService(typeof(IMenuCommandService));
            LineDuplicationCommand.Initialize(this, dte, commandService);
        }
    }
}
