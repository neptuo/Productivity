using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.Commands;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Neptuo.Productivity.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(ProductInfo.Name, ProductInfo.Description, VersionInfo.Version, IconResourceID = 400)]
    [Guid(PackageGuids.PackageString)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed partial class VsPackage : AsyncPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);
            await InitializeCommandsAsync(cancellationToken);
        }

        private async Task InitializeCommandsAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            DTE dte = (DTE)await GetServiceAsync(typeof(DTE));
            IMenuCommandService commandService = (IMenuCommandService)await GetServiceAsync(typeof(IMenuCommandService));
            LineDuplicationCommand.Initialize(this, dte, commandService);
        }
    }
}
