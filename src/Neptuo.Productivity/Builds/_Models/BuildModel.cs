using Neptuo.Collections.ObjectModel;
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
    public class BuildModel
    {
        private readonly IEventDispatcher events;
        private readonly List<BuildProjectModel> projects;

        public BuildScope Scope { get; private set; }
        public BuildAction Action { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public long? ElapsedMilliseconds { get; private set; }
        public int EstimatedProjectCount { get; private set; }

        public IEnumerable<BuildProjectModel> Projects
        {
            get { return projects; }
        }

        internal BuildModel(IEventDispatcher events, BuildScope scope, BuildAction action)
            : this(events, scope, action, DateTime.Now)
        { }

        internal BuildModel(IEventDispatcher events, BuildScope scope, BuildAction action, DateTime startedAt)
        {
            Ensure.NotNull(events, "events");
            this.events = events;
            projects = new List<BuildProjectModel>();

            Scope = scope;
            Action = action;
            StartedAt = startedAt;

            events.PublishAsync(new BuildStarted(this));
        }

        public BuildProjectModel AddProject(string name, string path)
        {
            BuildProjectModel model = new BuildProjectModel(events, this, name, path);
            projects.Add(model);
            events.PublishAsync(new ProjectAddedToBuild(this, model));
            return model;
        }

        public void EstimateProjectCount(int projectCount)
        {
            Ensure.Positive(projectCount, "projectCount");
            EstimatedProjectCount = projectCount;
            events.PublishAsync(new ProjectCountEstimated(this));
        }

        public void Finish(long elapsedMilliseconds)
        {
            FinishedAt = DateTime.Now;
            ElapsedMilliseconds = elapsedMilliseconds;
            events.PublishAsync(new BuildFinished(this));
        }
    }
}
