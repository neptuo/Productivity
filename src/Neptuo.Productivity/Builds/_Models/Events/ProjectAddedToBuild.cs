using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds.Events
{
    public class ProjectAddedToBuild : IBuildModelAware
    {
        public BuildModel Model { get; private set; }
        public BuildProjectModel ProjectModel { get; private set; }

        public ProjectAddedToBuild(BuildModel model, BuildProjectModel projectModel)
        {
            Ensure.NotNull(model, "model");
            Ensure.NotNull(projectModel, "projectModel");
            Model = model;
            ProjectModel = projectModel;
        }
    }
}
