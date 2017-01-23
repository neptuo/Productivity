using EnvDTE;
using Neptuo.Models.Domains;
using Neptuo.Productivity.VisualStudio.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class BuildCancelService : DisposableBase, IVsService
    {
        private readonly DTE dte;
        private readonly BuildEvents events;
        private readonly IConfiguration configuration;

        public BuildCancelService(DTE dte, IConfiguration configuration)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(configuration, "configuration");
            this.dte = dte;
            this.events = dte.Events.BuildEvents;
            this.configuration = configuration;

            events.OnBuildProjConfigDone += OnBuildProjConfigDone;
        }

        private void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            if (!success)
            {
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

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            events.OnBuildProjConfigDone -= OnBuildProjConfigDone;
        }
    }
}
