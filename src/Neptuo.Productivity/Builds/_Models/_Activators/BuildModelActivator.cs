using Neptuo.Activators;
using Neptuo.Pipelines.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds
{
    public class BuildModelActivator : IActivator<BuildModel, BuildModelActivatorContext>
    {
        private readonly IEventDispatcher events;

        public BuildModelActivator(IEventDispatcher events)
        {
            Ensure.NotNull(events, "events");
            this.events = events;
        }

        public BuildModel Create(BuildModelActivatorContext context)
        {
            return new BuildModel(events, context.Scope, context.Action);
        }
    }

    public class BuildModelActivatorContext
    {
        public BuildScope Scope { get; private set; }
        public BuildAction Action { get; private set; }

        public BuildModelActivatorContext(BuildScope scope, BuildAction action)
        {
            Scope = scope;
            Action = action;
        }
    }
}
