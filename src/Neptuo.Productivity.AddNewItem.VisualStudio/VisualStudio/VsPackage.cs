using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.VisualStudio.Commands;
using Neptuo.Productivity.VisualStudio.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
    public class VsPackage : AsyncPackage
    {
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);
            await InitializeCommandsAsync(cancellationToken);
        }

        private async Task InitializeCommandsAsync(CancellationToken cancellationToken)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            XmlTemplateServiceFactory templateServiceFactory = new XmlTemplateServiceFactory();

            DTE dte = await GetServiceAsync<DTE>();
            IMenuCommandService commandService = await GetServiceAsync<IMenuCommandService>();
            AddNewItemCommand.Initialize(this, commandService);
        }

        public async Task<T> GetComponentServiceAsync<T>()
            where T : class
        {
            IComponentModel componentModel = await GetServiceAsync<SComponentModel, IComponentModel>();
            return componentModel.GetService<T>();
        }

        public async Task<T> GetServiceAsync<T>() => (T)await GetServiceAsync(typeof(T));
        public async Task<TContract> GetServiceAsync<TService, TContract>() => (TContract)await GetServiceAsync(typeof(TService));
    }
}
