using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds
{
    public class BuildProgress
    {
        private readonly Stopwatch timer;
        private readonly List<BuildProjectProgress> projectBuilds;

        public BuildModel Model { get; private set; }
        public event Action<BuildProgress> OnFinished;

        public BuildProgress(BuildScope scope, BuildAction action)
        {
            Model = new BuildModel(scope, action, DateTime.Now);

            projectBuilds = new List<BuildProjectProgress>();
            timer = new Stopwatch();
            timer.Start();
        }

        public void StartProject(BuildProjectModel project)
        {
            projectBuilds.Add(new BuildProjectProgress(project));
            Model.Projects.Add(project);
        }

        public void DoneProject(string projectName, bool success)
        {
            BuildProjectProgress progress = projectBuilds.FirstOrDefault(p => p.ProjectName == projectName);
            if (progress != null)
            {
                progress.Finish(success);
                projectBuilds.Remove(progress);
            }
        }

        public void Finish()
        {
            timer.Stop();
            Model.FinishedAt = DateTime.Now;
            Model.ElapsedMilliseconds = timer.ElapsedMilliseconds;

            if (OnFinished != null)
                OnFinished(this);
        }
    }
}
