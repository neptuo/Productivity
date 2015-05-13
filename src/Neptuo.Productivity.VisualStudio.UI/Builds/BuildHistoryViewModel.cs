using Neptuo.ComponentModel;
using Neptuo.Productivity.Builds;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.UI.Builds
{
    public class BuildHistoryViewModel : ObservableObject
    {
        public ObservableCollection<BuildModel> Builds { get; private set; }

        public BuildHistoryViewModel(ObservableCollection<BuildModel> builds)
        {
            Ensure.NotNull(builds, "builds");
            Builds = builds;
        }
    }
}
