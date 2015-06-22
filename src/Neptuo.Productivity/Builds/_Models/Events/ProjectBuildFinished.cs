using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds.Events
{
    public class ProjectBuildFinished
    {
        public ProjectKey Key { get; private set; }
        public BuildProjectModel Model { get; private set; }

        public ProjectBuildFinished(BuildProjectModel model)
        {
            Ensure.NotNull(model, "model");
            Key = model.Key;
            Model = model;
        }
    }
}
