using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds.Events
{
    public class ProjectCountEstimated : IBuildModelAware
    {
        public BuildModel Model { get; private set; }

        public int EstimatedProjectCount
        {
            get { return Model.EstimatedProjectCount; }
        }

        public ProjectCountEstimated(BuildModel model)
        {
            Ensure.NotNull(model, "model");
            Model = model;
        }
    }
}
