using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Neptuo.Productivity.Builds;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class BuildService
    {
        private readonly BuildWatcher watcher;
        private readonly DTE dte;
        private BuildEvents events;
        private BuildProgress currentProgress;

        public BuildService(DTE dte)
        {
            Ensure.NotNull(dte, "dte");
            this.watcher = new BuildWatcher();
            this.dte = dte;
        }

        public void WireUpBuildEvents(BuildEvents events)
        {
            Ensure.NotNull(events, "events");
            this.events = events;
            events.OnBuildBegin += OnBuildBegin;
            events.OnBuildDone += OnBuildDone;
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

            List<BuildProjectModel> projects = new List<BuildProjectModel>();
            BuildScope scope = BuildScope.Project;

            switch (Scope)
            {
                case vsBuildScope.vsBuildScopeBatch:
                case vsBuildScope.vsBuildScopeProject:
                    scope = BuildScope.Project;
                    projects.AddRange(GetSelectedProjects(dte));
                    break;
                case vsBuildScope.vsBuildScopeSolution:
                    scope = BuildScope.Solution;
                    projects.AddRange(dte.Solution.Projects.OfType<Project>().Select(CreateProjectModel));
                    break;
                default:
                    return;
            }

            currentProgress = watcher.StartNew(scope, action, projects);
        }

        private IEnumerable<BuildProjectModel> GetSelectedProjects(DTE dte)
        {
            IEnumerable<Project> projects = dte.SelectedItems.OfType<Project>();
            if (projects.Any())
                return projects.Select(CreateProjectModel);

            IEnumerable<Document> documents = dte.SelectedItems.OfType<Document>();
            if(documents.Any())
                return documents.Select(d => CreateProjectModel(d.ProjectItem.Collection.ContainingProject));

            return Enumerable.Empty<BuildProjectModel>();
        }

        private BuildProjectModel CreateProjectModel(Project p)
        {
            return new BuildProjectModel(p.Name, p.FullName);
        }

        private void OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            if (currentProgress != null)
                currentProgress.Finish();

            currentProgress = null;
        }
    }
}
