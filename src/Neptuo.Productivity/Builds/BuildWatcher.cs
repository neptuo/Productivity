using Neptuo.Activators;
using Neptuo.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds
{
    public class BuildWatcher
    {
        private readonly IActivator<BuildModel, BuildModelActivatorContext> buildFactory;

        public ObservableCollection<BuildModel> History { get; private set; }

        public BuildWatcher(IActivator<BuildModel, BuildModelActivatorContext> buildFactory)
        {
            Ensure.NotNull(buildFactory, "buildFactory");
            this.buildFactory = buildFactory;
            History = new ObservableCollection<BuildModel>();
        }

        public BuildProgress StartNew(BuildScope scope, BuildAction action)
        {
            BuildModel model = buildFactory.Create(new BuildModelActivatorContext(scope, action));
            BuildProgress result = new BuildProgress(model);

            History.Insert(0, result.Model);
            return result;
        }
    }
}
