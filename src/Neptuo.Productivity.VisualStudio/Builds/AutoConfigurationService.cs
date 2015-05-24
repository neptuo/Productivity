using EnvDTE;
using Neptuo.ComponentModel;
using Neptuo.Productivity.Builds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.Builds
{
    public class AutoConfigurationService : DisposableBase, IVsService
    {
        private readonly DTE dte;
        private readonly VsServiceContainer vsServices;

        private SolutionEvents solutionEvents;
        private DocumentEvents documentEvents;
        private BuildEvents buildEvents;

        public AutoConfigurationService(DTE dte, VsServiceContainer vsServices)
        {
            this.dte = dte;
            this.vsServices = vsServices;


            solutionEvents = dte.Events.SolutionEvents;
            solutionEvents.Opened += OnSolutionOpened;

            documentEvents = dte.Events.DocumentEvents;
            documentEvents.DocumentSaved += OnDocumentSaved;

            buildEvents = dte.Events.BuildEvents;
            buildEvents.OnBuildDone += OnBuildDone;
        }


        private void OnBuildDone(vsBuildScope scope, vsBuildAction action)
        {
            SolutionBuild build = dte.Solution.SolutionBuild;

            if (action != vsBuildAction.vsBuildActionClean && action != vsBuildAction.vsBuildActionDeploy)
            {
                BuildService buildService;
                if (vsServices.TryGetService(out buildService))
                {
                    BuildModel buildModel = buildService.History.FirstOrDefault();
                    if (buildModel != null)
                    {
                        string[] projectNames = buildModel.Projects.Select(p => p.Name).ToArray();

                        foreach (SolutionConfiguration configuration in build.SolutionConfigurations)
                        {
                            foreach (SolutionContext context in configuration.SolutionContexts)
                            {
                                if (projectNames.Contains(context.ProjectName))
                                    context.ShouldBuild = false;
                            }
                        }

                    }
                }
                else
                {
                    OnSolutionOpened();
                }
            }
        }

        private void OnSolutionOpened()
        {
            SolutionBuild build = dte.Solution.SolutionBuild;
            object[] startups = (object[])build.StartupProjects;
            string startupProjectName = (string)startups[0];

            foreach (SolutionConfiguration configuration in build.SolutionConfigurations)
            {
                foreach (SolutionContext context in configuration.SolutionContexts)
                {
                    if (startupProjectName == context.ProjectName)
                        context.ShouldBuild = true;
                    else
                        context.ShouldBuild = false;
                }
            }
        }

        private void OnDocumentSaved(Document document)
        {
            string projectName = document.ProjectItem.ContainingProject.UniqueName;
            SolutionBuild build = dte.Solution.SolutionBuild;

            foreach (SolutionConfiguration configuration in build.SolutionConfigurations)
            {
                foreach (SolutionContext context in configuration.SolutionContexts)
                {
                    if (context.ProjectName == projectName)
                    {
                        context.ShouldBuild = true;
                        return;
                    }
                }
            }
        }

        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();

            solutionEvents.Opened -= OnSolutionOpened;
            documentEvents.DocumentSaved -= OnDocumentSaved;
            buildEvents.OnBuildDone -= OnBuildDone;
        }

    }
}
