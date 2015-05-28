using EnvDTE;
using Neptuo.ComponentModel;
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

        public BuildCancelService(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.dte = dte;
            this.events = dte.Events.BuildEvents;

            events.OnBuildProjConfigDone += OnBuildProjConfigDone;
        }

        private void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            if (!success)
            {
                dte.ExecuteCommand("Build.Cancel");
                dte.ExecuteCommand("View.Output");
            }
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            events.OnBuildProjConfigDone -= OnBuildProjConfigDone;
        }
    }
}
