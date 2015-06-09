using Neptuo.Collections.ObjectModel;
using Neptuo.ComponentModel;
using Neptuo.Pipelines.Events;
using Neptuo.Pipelines.Events.Handlers;
using Neptuo.Productivity.Builds;
using Neptuo.Productivity.Builds.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.VisualStudio.UI.Builds.HistoryOverviews
{
    public class QuickMainViewModel : ObservableObject, IEventHandler<BuildStarted>
    {
        private readonly IEventRegistry events;

        public ObservableCollection<QuickBuildViewModel> Builds { get; private set; }

        public QuickMainViewModel(IEventRegistry events)
        {
            Ensure.NotNull(events, "events");
            this.events = events;
            Builds = new ObservableCollection<QuickBuildViewModel>();

            events.Subscribe((IEventHandler<BuildStarted>)this);
        }

        public Task HandleAsync(BuildStarted payload)
        {
            Builds.Insert(0, new QuickBuildViewModel(events, payload.Model));

            while (Builds.Count > 3)
                Builds.RemoveAt(3);

            return Task.FromResult(true);
        }
    }
}
