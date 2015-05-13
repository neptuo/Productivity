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

        public BuildModel Model { get; private set; }
        public event Action<BuildProgress> OnFinished;

        public BuildProgress(BuildScope scope, BuildAction action, IEnumerable<BuildProjectModel> projects)
        {
            Model = new BuildModel(scope, action, DateTime.Now);
            Model.Projects.AddRange(projects);

            timer = new Stopwatch();
            timer.Start();
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
