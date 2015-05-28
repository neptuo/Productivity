using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds
{
    public class BuildProjectProgress
    {
        private readonly BuildProjectModel model;
        private readonly Stopwatch timer;

        public string ProjectName
        {
            get { return model.Name; }
        }

        public BuildProjectProgress(BuildProjectModel model)
        {
            Ensure.NotNull(model, "model");
            this.model = model;

            timer = new Stopwatch();
            timer.Start();
        }

        public void Finish(bool isSuccessful)
        {
            model.ElapsedMilliseconds = timer.ElapsedMilliseconds;
            model.IsSuccessful = isSuccessful;
        }
    }
}
