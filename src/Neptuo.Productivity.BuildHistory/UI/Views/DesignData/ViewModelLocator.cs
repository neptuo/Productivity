using Neptuo.Events;
using Neptuo.Models.Keys;
using Neptuo.Productivity.Events;
using Neptuo.Productivity.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.UI.Views.DesignData
{
    internal class ViewModelLocator
    {
        public static DefaultEventManager EventManager { get; } = new DefaultEventManager();
        public static QuickConfiguration QuickConfiguration { get; } = new QuickConfiguration();

        private static QuickMainViewModel quickMainViewModel;
        public static QuickMainViewModel QuickMainViewModel
        {
            get
            {
                if (quickMainViewModel == null)
                {
                    void Update(QuickBuildViewModel vm)
                    {
                        vm.UpdateBuildState();
                        vm.UpdateElapsed();
                    }

                    quickMainViewModel = new QuickMainViewModel(EventManager, QuickConfiguration);

                    var model = new BuildModel(EventManager, BuildScope.Project, BuildType.Rebuild, DateTime.Today.AddHours(3));
                    model.EstimateProjectCount(3);
                    model.AddProject("A");
                    model.Projects.Last().Finish(4000, true);
                    model.AddProject("B");
                    model.Projects.Last().Finish(1000, true);
                    model.AddProject("C");
                    model.Projects.Last().Finish(1050, true);
                    model.Finish(5050);

                    model = new BuildModel(EventManager, BuildScope.Project, BuildType.Build, DateTime.Today.AddHours(5));
                    model.EstimateProjectCount(2);
                    model.AddProject("A");
                    model.Projects.Last().Finish(9550, false);
                    model.Finish(9550);

                    model = new BuildModel(EventManager, BuildScope.Project, BuildType.Rebuild, DateTime.Today.AddHours(7).AddMinutes(30));
                    model.EstimateProjectCount(2);
                    model.AddProject("A");
                }

                return quickMainViewModel;
            }
        }
    }
}
