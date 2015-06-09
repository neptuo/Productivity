using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds.Events
{
    public class BuildFinished : IBuildModelAware
    {
        public BuildModel Model { get; private set; }

        public BuildFinished(BuildModel model)
        {
            Ensure.NotNull(model, "model");
            Model = model;
        }
    }
}
