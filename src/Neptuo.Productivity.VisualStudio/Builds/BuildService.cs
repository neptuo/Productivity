using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.Collections.ObjectModel;
using Neptuo.ComponentModel;
using Neptuo.Pipelines.Events;
using Neptuo.Productivity.Builds;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class BuildService : DisposableBase, IVsService
    {
        private readonly BuildWatcher watcher;
        private readonly DTE dte;
        private readonly BuildEvents events;
        private readonly OleMenuCommandService commandService;

        private MenuCommand menuItem;
        private BuildProgress currentProgress;

        public ObservableCollection<BuildModel> History
        {
            get { return watcher.History; }
        }

        public BuildService(DTE dte, IEventDispatcher events, OleMenuCommandService commandService, EventHandler menuHandler)
        {
            this.watcher = new BuildWatcher(new BuildModelActivator(events));
            this.dte = dte;
            this.events = dte.Events.BuildEvents;
            this.commandService = commandService;
            WireUpBuildEvents();

            CommandID commandID = new CommandID(MyConstants.CommandSetGuid, MyConstants.CommandSet.BuildHistory);
            menuItem = new MenuCommand(menuHandler, commandID);
            commandService.AddCommand(menuItem);
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
            BuildAction action = BuildAction.Unknown;
            switch (Action)
            {
                case vsBuildAction.vsBuildActionBuild:
                    action = BuildAction.Build;
                    break;
                case vsBuildAction.vsBuildActionClean:
                    action = BuildAction.Clean;
                    break;
                case vsBuildAction.vsBuildActionRebuildAll:
                    action = BuildAction.Rebuild;
                    break;
            }

            BuildScope scope = BuildScope.Unknown;
            switch (Scope)
            {
                case vsBuildScope.vsBuildScopeBatch:
                case vsBuildScope.vsBuildScopeProject:
                    scope = BuildScope.Project;
                    break;
                case vsBuildScope.vsBuildScopeSolution:
                    scope = BuildScope.Solution;
                    break;
            }

            int projectsToBuild = 0;
            foreach (SolutionContext context in dte.Solution.SolutionBuild.ActiveConfiguration.SolutionContexts)
            {
                if (context.ShouldBuild)
                    projectsToBuild++;
            }

            currentProgress = watcher.StartNew(scope, action);
            currentProgress.Model.EstimateProjectCount(projectsToBuild);
        }

        private void OnBuildProjConfigBegin(string projectName, string projectConfig, string platform, string solutionConfig)
        {
            if (currentProgress != null)
            {
                Project project = dte.Solution.Projects.OfType<Project>().First(p => p.UniqueName == projectName);
                if (project != null)
                    currentProgress.StartProject(project.UniqueName, project.FullName);
            }
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

            commandService.RemoveCommand(menuItem);

            events.OnBuildBegin -= OnBuildBegin;
            events.OnBuildDone -= OnBuildDone;
            events.OnBuildProjConfigBegin -= OnBuildProjConfigBegin;
            events.OnBuildProjConfigDone -= OnBuildProjConfigDone;
        }
    }
}
