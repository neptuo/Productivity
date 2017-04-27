using EnvDTE;
using Neptuo.Events;
using Neptuo.Observables.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class BuildService : DisposableBase
    {
        private readonly BuildWatcher watcher;
        private readonly DTE dte;
        private readonly BuildEvents events;

        private BuildProgress currentProgress;

        public ObservableCollection<BuildModel> History
        {
            get { return watcher.History; }
        }

        public BuildService(DTE dte, IEventDispatcher events)
        {
            Ensure.NotNull(dte, "dte");
            Ensure.NotNull(events, "events");

            this.watcher = new BuildWatcher(new BuildModelActivator(events));
            this.dte = dte;
            this.events = dte.Events.BuildEvents;
            WireUpBuildEvents();
        }

        private void WireUpBuildEvents()
        {
            events.OnBuildBegin += OnBuildBegin;
            events.OnBuildDone += OnBuildDone;
            events.OnBuildProjConfigBegin += OnBuildProjConfigBegin;
            events.OnBuildProjConfigDone += OnBuildProjConfigDone;
        }

        private void OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
        {
            BuildType action = BuildType.Unknown;
            switch (Action)
            {
                case vsBuildAction.vsBuildActionBuild:
                    action = BuildType.Build;
                    break;
                case vsBuildAction.vsBuildActionClean:
                    action = BuildType.Clean;
                    break;
                case vsBuildAction.vsBuildActionRebuildAll:
                    action = BuildType.Rebuild;
                    break;
            }
            
            int? projectsToBuild = null;
            BuildScope scope = BuildScope.Unknown;
            switch (Scope)
            {
                case vsBuildScope.vsBuildScopeProject:
                    scope = BuildScope.Project;
                    break;
                case vsBuildScope.vsBuildScopeSolution:
                    scope = BuildScope.Solution;
                    projectsToBuild = GetSolutionBuildProjectCount();
                    break;
            }

            currentProgress = watcher.StartNew(scope, action);

            if (projectsToBuild == null)
                currentProgress.Model.EstimateUncountableProjectCount();
            else
                currentProgress.Model.EstimateProjectCount(projectsToBuild.Value);
        }

        private int GetSolutionBuildProjectCount()
        {
            int projectsToBuild = 0;
            foreach (SolutionContext context in dte.Solution.SolutionBuild.ActiveConfiguration.SolutionContexts)
            {
                if (context.ShouldBuild)
                    projectsToBuild++;
            }

            return projectsToBuild;
        }

        private void OnBuildProjConfigBegin(string projectName, string projectConfig, string platform, string solutionConfig)
        {
            if (currentProgress != null)
                currentProgress.StartProject(projectName);
        }

        private void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            if (currentProgress != null)
                currentProgress.DoneProject(project, success);
        }

        private void OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            if (currentProgress != null)
                currentProgress.Finish();

            currentProgress = null;
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            events.OnBuildBegin -= OnBuildBegin;
            events.OnBuildDone -= OnBuildDone;
            events.OnBuildProjConfigBegin -= OnBuildProjConfigBegin;
            events.OnBuildProjConfigDone -= OnBuildProjConfigDone;
        }
    }
}
