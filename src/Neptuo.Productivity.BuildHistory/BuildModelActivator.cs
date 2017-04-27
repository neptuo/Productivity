using Neptuo.Activators;
using Neptuo.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class BuildModelActivator : IFactory<BuildModel, BuildModelActivatorContext>
    {
        private readonly IEventDispatcher events;

        public BuildModelActivator(IEventDispatcher events)
        {
            Ensure.NotNull(events, "events");
            this.events = events;
        }

        public BuildModel Create(BuildModelActivatorContext context)
        {
            return new BuildModel(events, context.Scope, context.Type);
        }
    }

    public class BuildModelActivatorContext
    {
        public BuildScope Scope { get; private set; }
        public BuildType Type { get; private set; }

        public BuildModelActivatorContext(BuildScope scope, BuildType action)
        {
            Scope = scope;
            Type = action;
        }
    }
}
