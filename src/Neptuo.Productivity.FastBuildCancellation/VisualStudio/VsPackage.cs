using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neptuo.Productivity.VisualStudio.Options;
using System;
using System.Collections.Generic;
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
    [ProvideOptionPage(typeof(ConfigurationPage), "Neptuo Productivity", "Fast Build Cancellation", 0, 0, true)]
    public class VsPackage : Package
    {
        private DTE dte;
        private BuildEvents events;

        protected override void Initialize()
        {
            base.Initialize();

            dte = (DTE)GetService(typeof(DTE));
            events = dte.Events.BuildEvents;

            events.OnBuildProjConfigDone += OnProjectBuildCompleted;
        }

        private void OnProjectBuildCompleted(string project, string projectConfig, string platform, string solutionConfig, bool isSuccess)
        {
            if (!isSuccess)
            {
                ConfigurationPage configuration = (ConfigurationPage)GetDialogPage(typeof(ConfigurationPage));

                dte.ExecuteCommand("Build.Cancel");
                switch (configuration.OpenWindowAfterBuildCancel)
                {
                    case BuildCancelWindow.Output:
                        dte.ExecuteCommand("View.Output");
                        break;
                    case BuildCancelWindow.ErrorList:
                        dte.ExecuteCommand("View.ErrorList");
                        break;
                    case BuildCancelWindow.None:
                    default:
                        break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                events.OnBuildProjConfigDone -= OnProjectBuildCompleted;
        }
    }
}
