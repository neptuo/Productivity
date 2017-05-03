using Neptuo.Events;
using Neptuo.Models.Keys;
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
                    quickMainViewModel = new QuickMainViewModel(EventManager, QuickConfiguration);
                    quickMainViewModel.Builds.Add(new QuickBuildViewModel(
                        EventManager,
                        Int32Key.Create(1, "Build"),
                        BuildScope.Project,
                        BuildType.Rebuild,
                        DateTime.Today.AddHours(1)
                    ));
                    quickMainViewModel.Builds.Last().ElapsedMilliseconds = 3550;
                    quickMainViewModel.Builds.Last().IsSuccessful = false;
                    quickMainViewModel.Builds.Add(new QuickBuildViewModel(
                        EventManager,
                        Int32Key.Create(2, "Build"),
                        BuildScope.Solution,
                        BuildType.Build,
                        DateTime.Today.AddHours(5)
                    ));
                    quickMainViewModel.Builds.Last().ElapsedMilliseconds = 76343;
                    quickMainViewModel.Builds.Last().IsSuccessful = true;
                }

                return quickMainViewModel;
            }
        }
    }
}
