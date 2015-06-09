using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds.Events
{
    public class BuildStarted : IBuildModelAware
    {
        public BuildModel Model { get; private set; }

        public BuildStarted(BuildModel model)
        {
            Ensure.NotNull(model, "model");
            Model = model;
        }
    }
}
