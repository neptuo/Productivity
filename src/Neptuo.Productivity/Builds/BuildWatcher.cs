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
        public ObservableCollection<BuildModel> History { get; private set; }

        public BuildWatcher()
        {
            History = new ObservableCollection<BuildModel>();
        }

        public BuildProgress StartNew(BuildScope scope, BuildAction action, IEnumerable<BuildProjectModel> projects)
        {
            Ensure.NotNull(projects, "projects");
            BuildProgress result = new BuildProgress(scope, action, projects);
            History.Insert(0, result.Model);
            return result;
        }
    }
}
