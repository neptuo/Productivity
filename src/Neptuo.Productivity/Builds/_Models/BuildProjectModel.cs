using Neptuo.ComponentModel;
using Neptuo.Pipelines.Events;
using Neptuo.Productivity.Builds.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds
{
    public class BuildProjectModel
    {
        private readonly IEventDispatcher events;
        private readonly BuildModel model;

        public string Name { get; private set; }
        public string Path { get; private set; }
        public long? ElapsedMilliseconds { get; private set; }
        public bool? IsSuccessful { get; private set; }

        internal BuildProjectModel(IEventDispatcher events, BuildModel model, string name, string path)
        {
            Ensure.NotNull(events, "events");
            Ensure.NotNull(model, "model");
            this.events = events;
            this.model = model;
            Name = name;
            Path = path;
        }

        public void Finish(long elapsedMilliseconds, bool isSuccessful)
        {
            ElapsedMilliseconds = elapsedMilliseconds;
            IsSuccessful = isSuccessful;
            events.PublishAsync(new ProjectBuildFinished(model, this));
        }
    }
}
