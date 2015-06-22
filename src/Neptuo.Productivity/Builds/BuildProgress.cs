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

        public BuildProgress(BuildModel model)
        {
            Ensure.NotNull(model, "model");
            Model = model;

            projectBuilds = new List<BuildProjectProgress>();
            timer = new Stopwatch();
            timer.Start();
        }

        public void StartProject(string name)
        {
            projectBuilds.Add(new BuildProjectProgress(Model.AddProject(name)));
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
            Model.Finish(timer.ElapsedMilliseconds);

            if (OnFinished != null)
                OnFinished(this);
        }
    }
}
