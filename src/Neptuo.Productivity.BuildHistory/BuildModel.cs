using Neptuo.Events;
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
    public class BuildModel : IDomainModel<Int32Key>
    {
        private static readonly object nextIDLock = new object();
        private static int nextID = 1;

        private readonly IEventDispatcher events;
        private readonly List<BuildProjectModel> projects;

        public Int32Key Key { get; private set; }
        public BuildScope Scope { get; private set; }
        public BuildType Action { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public long? ElapsedMilliseconds { get; private set; }
        public int? EstimatedProjectCount { get; private set; }

        public IEnumerable<BuildProjectModel> Projects
        {
            get { return projects; }
        }

        internal BuildModel(IEventDispatcher events, BuildScope scope, BuildType action)
            : this(events, scope, action, DateTime.Now)
        { }

        internal BuildModel(IEventDispatcher events, BuildScope scope, BuildType action, DateTime startedAt)
        {
            Ensure.NotNull(events, "events");
            this.events = events;
            projects = new List<BuildProjectModel>();

            lock (nextIDLock)
            {
                Key = Int32Key.Create(nextID, "Build");
                nextID++;
            }

            Scope = scope;
            Action = action;
            StartedAt = startedAt;

            _ = events.PublishAsync(new BuildStarted(Key, Scope, Action, StartedAt));
        }

        public BuildProjectModel AddProject(string name)
        {
            BuildProjectModel model = new BuildProjectModel(events, Key, name);
            projects.Add(model);
            return model;
        }

        public void EstimateProjectCount(int projectCount)
        {
            Ensure.Positive(projectCount, "projectCount");
            EstimatedProjectCount = projectCount;
            _ = events.PublishAsync(new ProjectCountEstimated(Key, projectCount));
        }

        public void EstimateUncountableProjectCount()
        { }

        public void Finish(long elapsedMilliseconds)
        {
            FinishedAt = DateTime.Now;
            ElapsedMilliseconds = elapsedMilliseconds;
            _ = events.PublishAsync(new BuildFinished(this));
        }
    }
}
