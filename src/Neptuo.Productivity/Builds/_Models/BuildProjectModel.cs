using Neptuo.ComponentModel;
using Neptuo.DomainModels;
using Neptuo.Pipelines.Events;
using Neptuo.Productivity.Builds.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity.Builds
{
    public class BuildProjectModel : IDomainModel<ProjectKey>
    {
        private readonly IEventDispatcher events;

        public ProjectKey Key { get; private set; }
        public string Name { get; private set; }
        public string Path { get; private set; }
        public long? ElapsedMilliseconds { get; private set; }
        public bool? IsSuccessful { get; private set; }

        internal BuildProjectModel(IEventDispatcher events, Int32Key buildKey, string name, string path)
        {
            Ensure.NotNull(events, "events");
            this.events = events;
            Key = ProjectKey.Create(buildKey, name, "Project");
            Name = name;
            Path = path;
            events.PublishAsync(new ProjectBuildStarted(Key, Name, Path));
        }

        public void Finish(long elapsedMilliseconds, bool isSuccessful)
        {
            ElapsedMilliseconds = elapsedMilliseconds;
            IsSuccessful = isSuccessful;
            events.PublishAsync(new ProjectBuildFinished(this));
        }
    }
}
