﻿using Neptuo.Events;
using Neptuo.Models.Domains;
using Neptuo.Models.Keys;
using Neptuo.Productivity.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Productivity
{
    public class BuildProjectModel : IDomainModel<ProjectKey>
    {
        private readonly IEventDispatcher events;

        public ProjectKey Key { get; private set; }
        public string Name { get; private set; }
        public long? ElapsedMilliseconds { get; private set; }
        public bool? IsSuccessful { get; private set; }

        internal BuildProjectModel(IEventDispatcher events, Int32Key buildKey, string name)
        {
            Ensure.NotNull(events, "events");
            this.events = events;
            Key = ProjectKey.Create(buildKey, name, "Project");
            Name = name;
            _ = events.PublishAsync(new ProjectBuildStarted(Key, Name));
        }

        public void Finish(long elapsedMilliseconds, bool isSuccessful)
        {
            ElapsedMilliseconds = elapsedMilliseconds;
            IsSuccessful = isSuccessful;
            _ = events.PublishAsync(new ProjectBuildFinished(this));
        }
    }
}
