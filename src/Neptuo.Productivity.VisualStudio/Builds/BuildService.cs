using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.Collections.ObjectModel;
using Neptuo.ComponentModel;
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

        public BuildService(DTE dte, OleMenuCommandService commandService, EventHandler menuHandler)
        {
            this.watcher = new BuildWatcher();
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
            BuildAction action = BuildAction.Build;
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
                default:
                    return;
            }

            BuildScope scope = BuildScope.Project;
            switch (Scope)
            {
                case vsBuildScope.vsBuildScopeBatch:
                case vsBuildScope.vsBuildScopeProject:
                    scope = BuildScope.Project;
                    break;
                case vsBuildScope.vsBuildScopeSolution:
                    scope = BuildScope.Solution;
                    break;
                default:
                    return;
            }

            currentProgress = watcher.StartNew(scope, action);
        }

        private void OnBuildProjConfigBegin(string projectName, string projectConfig, string platform, string solutionConfig)
        {
            Project project = dte.Solution.Projects.OfType<Project>().First(p => p.UniqueName == projectName);
            BuildProjectModel model = CreateProjectModel(project);
            currentProgress.StartProject(model);
        }

        private void OnBuildProjConfigDone(string project, string projectConfig, string platform, string solutionConfig, bool success)
        {
            currentProgress.DoneProject(project, success);
        }

        private BuildProjectModel CreateProjectModel(Project p)
        {
            return new BuildProjectModel(p.UniqueName, p.FullName);
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
        }
    }
}
